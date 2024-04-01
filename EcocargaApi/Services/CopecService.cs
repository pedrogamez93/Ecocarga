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
    public class CopecService : ICopecService
    {
        private readonly EcocargaContext _context;
        private readonly IOptions<AppSettings> _settings;
        private readonly ILogger _logger;

        public CopecService(IOptions<AppSettings> settings, EcocargaContext context, ILogger<CopecService> logger)
        {
            _settings = settings;
            _context = context;
            _logger = logger;
        }
        public async Task<List<Copec>> ConsumirAPI()
        {
            HttpClient http = new HttpClient();
            var urlApi = _settings.Value.ApiCopec;
            List<Copec> cargadoresCopec = new List<Copec>();
            try
            {
                var Cargadores = await http.GetStringAsync(urlApi);
                cargadoresCopec = JsonConvert.DeserializeObject<List<Copec>>(Cargadores);
                return (cargadoresCopec);
            }
            catch
            {
                return cargadoresCopec;
            }
        }
        public async Task UpdateCopec()
        {
            _logger.LogInformation(string.Concat(DateTime.Now.ToString("dd MMMM yyyy hh:mm:ss tt"), "Inicio ejecucion programada actualización datos Copec"));
            try
            {
                var cargadoresCopec = await ConsumirAPI();
                TransactionResponse transactionResponse = new TransactionResponse();
                if (cargadoresCopec.Count > 0)
                {
                    transactionResponse = await ActualizarElectrolineras(cargadoresCopec);
                    if (!transactionResponse.Error)
                    {
                        _logger.LogInformation("Actualización correcta datos Copec.");
                    }
                }
                else
                {
                    _logger.LogInformation("No hay información para actualizar disponible desde Copec.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
        public async Task<TransactionResponse> ActualizarElectrolineras(List<Copec> cargadoresCopec)
        {
            TransactionResponse transactionResponse = new TransactionResponse();
            List<DataElectrolinera> dataElectrolineraUpdate = new List<DataElectrolinera>();

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var c in cargadoresCopec)
                    {
                        var dataElectrolinera = await _context.DataElectrolinera
                                                .FirstOrDefaultAsync(e => e.IdElectrolineraCliente == c.IdCargador);

                        if (dataElectrolinera != null)
                        {
                            dataElectrolinera.EstadoElectrolinera = Estados.ObtenerEstados(c.Status);
                            dataElectrolinera.IdEstadoElectrolinera = Enum.IsDefined(typeof(EstadosCargadores), c.Status) ?
                                                        (int)(EstadosCargadores)Enum.Parse(typeof(EstadosCargadores), c.Status) : 0;

                            dataElectrolinera.DataCargador = (await ActualizarCargadores(c.Conectores, dataElectrolinera.Id));

                            dataElectrolineraUpdate.Add(dataElectrolinera);
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
                    transactionResponse.Message = $"Error al actualizar electrolineras de Copec, {ex.Message}";
                }
            }
            return transactionResponse;
        }

        public async Task<List<DataCargador>> ActualizarCargadores(Conectores[] conectores, int idElectrolinera)
        {
            List<DataCargador> dataCargadorUpdate = new List<DataCargador>();
            try
            {
                foreach (var c in conectores)
                {
                    var dataCargador = await _context.DataCargador
                                            .FirstOrDefaultAsync(con => con.IdCargadorCliente == c.IdConector
                                            && con.ElectrolineraId == idElectrolinera);

                    if (dataCargador != null)
                    {
                        dataCargador.EstadoCargador = ObtenerEstados(c.Status);
                        dataCargador.IdEstadoCargador = Enum.IsDefined(typeof(EstadosCargadores), c.Status) ?
                                                        (int)(EstadosCargadores)Enum.Parse(typeof(EstadosCargadores), c.Status) : 0;

                        var dataTipoCobro = await _context.DataTipocobro
                                            .FirstOrDefaultAsync(tp => tp.CargadorId == dataCargador.Id);

                        if (dataTipoCobro != null)
                        {
                            dataTipoCobro.Precio = c.Precio;
                            dataTipoCobro.UnidadCobro = c.Unidad;
                            dataCargador.DataTipocobroCargador.ToArray()[0] = dataTipoCobro;
                        }

                        dataCargadorUpdate.Add(dataCargador);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return dataCargadorUpdate;
        }
    }
}
