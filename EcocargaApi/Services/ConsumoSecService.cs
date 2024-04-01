using Cl.Gob.Energia.Ecocarga.Api.Services.Interfaces;
using Cl.Gob.Energia.Ecocarga.Data.Models;
using Cl.Gob.Energia.Ecocarga.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Cl.Gob.Energia.Ecocarga.Api.Utils;
using static Cl.Gob.Energia.Ecocarga.Api.Utils.Estados;

namespace Cl.Gob.Energia.Ecocarga.Api.Services
{
    public class ConsumoSecService : IConsumoSecService
    {
        private readonly EcocargaContext _context;
        private readonly IOptions<AppSettings> _settings;
        private readonly ILogger _logger;
        private readonly IEmailSender _emailSender;
        private readonly EmailSettings _emailSettings;

        public ConsumoSecService(IOptions<AppSettings> settings, EcocargaContext context, ILogger<ConsumoSecService> logger, IEmailSender emailSender, IOptions<EmailSettings> emailSettings)
        {
            _settings = settings;
            _context = context;
            _logger = logger;
            _emailSender = emailSender;
            _emailSettings = emailSettings.Value;
        }
        public async Task<List<ConsumoSec>> ConsumirAPI()
        {
            HttpClient http = new HttpClient();
            var urlApi = _settings.Value.ApiSEC;
            List<ConsumoSec> cargadoresConsumoSec = new List<ConsumoSec>();
            try
            {
                var Cargadores = await http.GetStringAsync(urlApi);
                cargadoresConsumoSec = JsonConvert.DeserializeObject<List<ConsumoSec>>(Cargadores);
                return (cargadoresConsumoSec);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return cargadoresConsumoSec;
            }
        }
        public async Task UpdateConsumoSec()
        {
            var email = _emailSettings.Receiver;
            var subject = _emailSettings.Subject;
            _logger.LogInformation(string.Concat(DateTime.Now.ToString("dd MMMM yyyy hh:mm:ss tt") , "Inicio ejecucion programada actualización datos SEC"));
            try
            {
                var cargadoresConsumoSec = await ConsumirAPI();
                TransactionResponse transactionResponse = new TransactionResponse();
                if (cargadoresConsumoSec.Count > 0)
                {
                    transactionResponse = await ActualizarElectrolineras(cargadoresConsumoSec);
                    if (!transactionResponse.Error)
                    {
                        _logger.LogInformation("Actualización correcta datos SEC.");
                    }
                }
                else
                {
                    var message = "Estimados,<br/><br/>Se detecta un error en la ejecución del proceso, la descripción es la siguiente:<br/><br/>No hay información para actualizar disponible desde SEC, revisar estado del funcionamiento en el servicio " + _settings.Value.ApiSEC + " .<br/><br/>Saludos<br/>Equipo EcoCarga.";
                    await _emailSender.SendEmailAsync(email, subject, message);
                    _logger.LogInformation("No hay información para actualizar disponible desde SEC.");
                }
            }
            catch (Exception ex)
            {
                var message = "Estimados,<br/><br/>Se detecta un error en la ejecución del proceso, la descripción es la siguiente:<br/><br/>" + ex.Message + " .<br/><br/>Saludos<br/>Equipo EcoCarga.";
                await _emailSender.SendEmailAsync(email, subject, message);
                _logger.LogError(ex.Message);
            }
        }

        public async Task<TransactionResponse> ActualizarElectrolineras(List<ConsumoSec> ConsumoSec)
        {
            TransactionResponse transactionResponse = new TransactionResponse();
            List<DataElectrolinera> dataElectrolineraUpdate = new List<DataElectrolinera>();

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var cabeceras in ConsumoSec)
                    {
                        var listaDataElectrolineras = (await UpdateElectrolinerasAsync(cabeceras));

                            foreach (var item in listaDataElectrolineras)
                            {
                                dataElectrolineraUpdate.Add(item);
                            }
                    }

                    _context.DataElectrolinera.UpdateRange(dataElectrolineraUpdate);
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    transactionResponse.Error = true;
                    var email = _emailSettings.Receiver;
                    var subject = _emailSettings.Subject;
                    var message = "Estimados,<br/><br/>Se detecta un error en la ejecución del proceso, la descripción es la siguiente:<br/><br/>" + ex.Message + " .<br/><br/>Saludos<br/>Equipo EcoCarga.";
                    await _emailSender.SendEmailAsync(email, subject, message);
                    transactionResponse.Message = $"Error al actualizar electrolineras de SEC, {ex.Message}";
                }
            }
            return transactionResponse;
        }
        public async Task<List<DataElectrolinera>> UpdateElectrolinerasAsync(ConsumoSec electrolineraCabecera)
        {
            List<DataElectrolinera> electrolineras = new List<DataElectrolinera>();
            var email = _emailSettings.Receiver;
            var subject = _emailSettings.Subject;
            try
            {
                foreach (var electrolinera in electrolineraCabecera.Cargadores)
                {
                    var dataElectrolinera = await _context.DataElectrolinera
                                            .FirstOrDefaultAsync(e => e.IdElectrolineraSec == (electrolineraCabecera.Inscripcion + electrolinera.IdCargador.ToString()));
                    var compania = await _context.DataCompania.FirstOrDefaultAsync(c => c.RutPropietario == electrolineraCabecera.Rut_Propietario);

                    if (compania != null)
                    {
                        if (dataElectrolinera != null)
                        {
                            dataElectrolinera.Latitud = dataElectrolinera.CoordenadasActualizar ? electrolineraCabecera.Latitud : dataElectrolinera.Latitud;
                            dataElectrolinera.Longitud = dataElectrolinera.CoordenadasActualizar ? electrolineraCabecera.Longitud : dataElectrolinera.Longitud;
                            dataElectrolinera.Region = electrolineraCabecera.Region;
                            dataElectrolinera.Comuna = electrolineraCabecera.Comuna;
                            dataElectrolinera.Horario = string.IsNullOrEmpty(electrolineraCabecera.Horario) ? String.Empty : electrolineraCabecera.Horario;
                            dataElectrolinera.Direccion = electrolineraCabecera.Direccion;
                            dataElectrolinera.CompaniaId = compania.Id;
                            dataElectrolinera.Precio = 0;
                            dataElectrolinera.AceptaConexionAc = false;
                            dataElectrolinera.AceptaConexionDc = false;
                            dataElectrolinera.Nombre = electrolineraCabecera.Nombre + " - " + electrolinera.IdCargador.ToString();
                            dataElectrolinera.Marca = electrolinera.Marca;
                            dataElectrolinera.Modelo = electrolinera.Modelo;
                            dataElectrolinera.IdElectrolineraSec = electrolineraCabecera.Inscripcion + electrolinera.IdCargador.ToString();
                            dataElectrolinera.IdElectrolineraCliente = electrolineraCabecera.Inscripcion + "-" + electrolinera.IdCargador.ToString();
                            dataElectrolinera.Potencia = electrolinera.Potencia;
                            dataElectrolinera.CantidadPuntosCarga = electrolinera.CantidadConectores;

                            var listaDataCargadores = (await UpdateCargadoresAsync(electrolinera, dataElectrolinera.Id, dataElectrolinera.IdElectrolineraCliente));

                            foreach (var item in listaDataCargadores)
                            {
                                dataElectrolinera.AceptaConexionAc = dataElectrolinera.AceptaConexionAc == true ? dataElectrolinera.AceptaConexionAc : Estados.ObtenerCargas(item.TipoCorriente.ToUpper(), "AC");
                                dataElectrolinera.AceptaConexionDc = dataElectrolinera.AceptaConexionDc == true ? dataElectrolinera.AceptaConexionDc : Estados.ObtenerCargas(item.TipoCorriente.ToUpper(), "DC");
                                dataElectrolinera.DataCargador.Add(item);
                            }

                            electrolineras.Add(dataElectrolinera);
                        }
                        else
                        {
                            var dataElectrolineraNew = new DataElectrolinera()
                            {
                                Latitud = electrolineraCabecera.Latitud,
                                Longitud = electrolineraCabecera.Longitud,
                                Region = electrolineraCabecera.Region,
                                Comuna = electrolineraCabecera.Comuna,
                                Horario = string.IsNullOrEmpty(electrolineraCabecera.Horario) ? String.Empty : electrolineraCabecera.Horario,
                                Direccion = electrolineraCabecera.Direccion,
                                CompaniaId = compania.Id,
                                Precio = 0,
                                IdPublico = Guid.NewGuid(),
                                AceptaConexionAc = false,
                                AceptaConexionDc = false,
                                IdEstadoElectrolinera = (int)EstadosCargadores.AVAILABLE,
                                EstadoElectrolinera = "Disponible",
                                Nombre = electrolineraCabecera.Nombre + " - " + electrolinera.IdCargador.ToString(),
                                Marca = electrolinera.Marca,
                                Modelo = electrolinera.Modelo,
                                IdElectrolineraSec = electrolineraCabecera.Inscripcion + electrolinera.IdCargador.ToString(),
                                IdElectrolineraCliente = electrolineraCabecera.Inscripcion + "-" + electrolinera.IdCargador.ToString(),
                                Potencia = electrolinera.Potencia,
                                CantidadPuntosCarga = electrolinera.CantidadConectores,
                                CoordenadasActualizar = false,
                            };

                            var listaDataCargadores = (await UpdateCargadoresAsync(electrolinera, dataElectrolineraNew.Id, dataElectrolineraNew.IdElectrolineraCliente));

                            foreach (var item in listaDataCargadores)
                            {
                                dataElectrolineraNew.AceptaConexionAc = dataElectrolineraNew.AceptaConexionAc == true ? dataElectrolineraNew.AceptaConexionAc : Estados.ObtenerCargas(item.TipoCorriente.ToUpper(), "AC");
                                dataElectrolineraNew.AceptaConexionDc = dataElectrolineraNew.AceptaConexionDc == true ? dataElectrolineraNew.AceptaConexionDc : Estados.ObtenerCargas(item.TipoCorriente.ToUpper(), "DC");
                                dataElectrolineraNew.DataCargador.Add(item);
                            }

                            electrolineras.Add(dataElectrolineraNew);
                        }
                    }
                    else 
                    {
                        var message = "Estimados,<br/><br/>Se detecta un error en la ejecución del proceso, la descripción es la siguiente:<br/><br/>El propietario con rut " + electrolineraCabecera.Rut_Propietario + " no existe en plataforma.<br/><br/>Saludos<br/>Equipo EcoCarga.";
                        await _emailSender.SendEmailAsync(email, subject, message);
                    }
                }
            }
            catch (Exception ex)
            {
                var message = "Estimados,<br/><br/>Se detecta un error en la ejecución del proceso, la descripción es la siguiente:<br/><br/>" + ex.Message + ".<br/><br/>Saludos<br/>Equipo EcoCarga.";
                await _emailSender.SendEmailAsync(email, subject, message);
                _logger.LogError(ex.Message);
            }
            return electrolineras;
        }


        public async Task<List<DataCargador>> UpdateCargadoresAsync(CargadoresSec electrolineras, int? idElectrolinera, string IdElectrolineraCliente)
        {
            List<DataCargador> cargadores = new List<DataCargador>();
            var email = _emailSettings.Receiver;
            var subject = _emailSettings.Subject;
            try
            {
                foreach (var cargador in electrolineras.Conectores)
                {
                    var dataCargador = await _context.DataCargador
                                            .FirstOrDefaultAsync(e => e.IdCargadorSec == cargador.IdConector.ToString() && e.ElectrolineraId == idElectrolinera);

                    var dataTipoConector = await _context.DataDiccionarioTipoConector.FirstOrDefaultAsync(dtc => dtc.TipoConectorExterno == cargador.Tipo && dtc.EntidadExterna == "SEC");

                    if (dataTipoConector != null)
                    {
                        if (dataCargador != null)
                        {
                            dataCargador.Cable = cargador.Cable.ToLower().Equals("si") ? true : false;
                            dataCargador.Potencia = cargador.Potencia;
                            dataCargador.TipoCorriente = cargador.Carga.ToLower().Equals("ac") ? "ac" : "dc";
                            dataCargador.TipoConectorId = dataTipoConector.TipoConectorId;
                            dataCargador.IdCargadorCliente = IdElectrolineraCliente + "-" + cargador.IdConector.ToString();
                            cargadores.Add(dataCargador);
                        }
                        else
                        {
                            var dataCargadorNew = new DataCargador()
                            {
                                Cable = cargador.Cable.ToLower().Equals("si") ? true : false,
                                Potencia = cargador.Potencia,
                                TipoCorriente = cargador.Carga.ToLower().Equals("ac") ? "ac" : "dc",
                                IdEstadoCargador = (int)EstadosCargadores.AVAILABLE,
                                EstadoCargador = "Disponible",
                                IdCargadorSec = cargador.IdConector.ToString(),
                                IdCargadorCliente = IdElectrolineraCliente + "-" + cargador.IdConector.ToString(),
                                TipoConectorId = dataTipoConector.TipoConectorId,
                            };
                            cargadores.Add(dataCargadorNew);
                        }
                    }
                    else 
                    {
                        var message = "Estimados,<br/><br/>Se detecta un error en la ejecución del proceso, la descripción es la siguiente:<br/><br/>El conector " + cargador.Tipo + " no existe en plataforma.<br/><br/>Saludos<br/>Equipo EcoCarga.";
                        await _emailSender.SendEmailAsync(email, subject, message);
                    }
                }
            }
            catch (Exception ex)
            {
                var message = "Estimados,<br/><br/>Se detecta un error en la ejecución del proceso, la descripción es la siguiente:<br/><br/>" + ex.Message + ".<br/><br/>Saludos<br/>Equipo EcoCarga.";
                await _emailSender.SendEmailAsync(email, subject, message);
                _logger.LogError(ex.Message);
            }
            return cargadores;
        }
    }
}
