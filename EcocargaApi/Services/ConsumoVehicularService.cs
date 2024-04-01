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

namespace Cl.Gob.Energia.Ecocarga.Api.Services
{
    public class ConsumoVehicularService : IConsumoVehicularService
    {
        private readonly EcocargaContext _context;
        private readonly IOptions<AppSettings> _settings;
        private readonly ILogger _logger;

        public ConsumoVehicularService(IOptions<AppSettings> settings, EcocargaContext context, ILogger<ConsumoVehicularService> logger)
        {
            _settings = settings;
            _context = context;
            _logger = logger;
        }

        public async Task UpdateConsumoVehicular()
        {
            _logger.LogInformation(string.Concat(DateTime.Now.ToString("dd MMMM yyyy hh:mm:ss tt"), "Inicio ejecucion programada actualización datos Consumo Vehicular"));
            try
            {
                TransactionResponse transactionResponse = new TransactionResponse();
                var vehiculosElectricos = await ConsumirAPI();

                if (vehiculosElectricos.Count > 0)
                {
                    transactionResponse = await ActualizarMarcasAutos(vehiculosElectricos);
                    if (!transactionResponse.Error)
                    {
                        transactionResponse = await ActualizarAutos(vehiculosElectricos);
                    }
                }
                else
                {
                    _logger.LogInformation("No hay información para actualizar disponible desde Consumo Vehicular.");
                }

                _logger.LogInformation("Actualización correcta datos Consumo Vehicular.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public async Task<List<ConsumoVehicular>> ConsumirAPI()
        {
            HttpClient http = new HttpClient();
            var urlApi = _settings.Value.ApiConsumoVehicular;
            List<ConsumoVehicular> vehiculosElectricos = new List<ConsumoVehicular>();
            try
            {
                http.DefaultRequestHeaders.Add("key", _settings.Value.KeyApiConsumoVehicular);
                var vehiculos = await http.GetStringAsync(urlApi);
                var arregloVehiculos = JsonConvert.DeserializeObject<IEnumerable<ConsumoVehicular>[]>(vehiculos);

                foreach (var arr in arregloVehiculos)
                {
                    arr.ToList().ForEach(v =>
                    {
                        if (v.Propulsion.ToUpper().Contains(Constantes.VEHICULO_TIPO_ELECTRICO))
                        {
                            vehiculosElectricos.Add(v);
                        }
                    });
                }

                return (vehiculosElectricos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return (vehiculosElectricos);
            }
        }

        public async Task<TransactionResponse> ActualizarMarcasAutos(IEnumerable<ConsumoVehicular> vehiculosElectricos)
        {
            TransactionResponse transactionResponse = new TransactionResponse();
            List<DataMarcaauto> marcas = new List<DataMarcaauto>();
            var RutaImagenNoDisponible = _settings.Value.PathImagenNoDisponibleMarcas;
            var marcasAgrupadas = vehiculosElectricos.GroupBy(x => x.Marca).ToList();

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var ma in marcasAgrupadas)
                    {
                        var marcasAgrupada = ma.First();
                        var marca = await _context.DataMarcaauto.FirstOrDefaultAsync(dma => dma.Nombre == marcasAgrupada.Marca);

                        if (marca == null)
                        {
                            var marcaNuevo = new DataMarcaauto()
                            {
                                Nombre = marcasAgrupada.Marca,
                                Imagen = RutaImagenNoDisponible
                            };

                            marcas.Add(marcaNuevo);
                        }
                    }

                    _context.AddRange(marcas);
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    transactionResponse.Error = true;
                    transactionResponse.Message = $"Error al actualizar Marcas de Autos, {ex.Message}";
                }
            }

            return transactionResponse;
        }

        public async Task<TransactionResponse> ActualizarAutos(IEnumerable<ConsumoVehicular> vehiculosElectricos)
        {
            TransactionResponse transactionResponse = new TransactionResponse();
            List<DataAuto> autosNuevos = new List<DataAuto>();
            List<DataAuto> autosactualizados = new List<DataAuto>();
            var RutaImagenNoDisponible = _settings.Value.PathImagenNoDisponibleModelos;

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var v in vehiculosElectricos)
                    {
                        var dataAuto = _context.DataAuto.FirstOrDefaultAsync(m => m.CodigoInformeTecnico == v.CodigoInformeTecnico);
                        var tipoConectorAcId = _context.DataDiccionarioTipoConector.FirstOrDefaultAsync(dtc => dtc.TipoConectorExterno == v.TipoDeConectorAc);
                        var tipoConectorDcId = _context.DataDiccionarioTipoConector.FirstOrDefaultAsync(dtc => dtc.TipoConectorExterno == v.TipoDeConectorDc);
                        var marca = _context.DataMarcaauto.FirstOrDefaultAsync(m => m.Nombre == v.Marca);
                        await Task.WhenAll(dataAuto, tipoConectorAcId, tipoConectorDcId, marca);

                        int found = v.CapacidadConvertidorVehiculoElectrico.IndexOf("- ");
                        string parse_Capacidad_convertidor_vehiculo_electrico = found > 0 ? v.CapacidadConvertidorVehiculoElectrico.Substring(found + 2) : v.CapacidadConvertidorVehiculoElectrico;


                        if (dataAuto.Result == null)
                        {
                            var dataAutoNuevo = new DataAuto()
                            {
                                Modelo = v.Modelo,
                                Traccion = v.Traccion,
                                IdPublico = Guid.NewGuid(),
                                MarcaId = marca.Result.Id,
                                TipoConectorAcId = tipoConectorAcId.Result.TipoConectorId,
                                TipoConectorDcId = tipoConectorDcId.Result == null ? null : (int?)tipoConectorDcId.Result.TipoConectorId,
                                CapacidadBateria = Convert.ToDouble(v.AcumulacionEnergiaBateria.Replace('.', ',')),
                                CapacidadInversorInternoAc = String.IsNullOrEmpty(parse_Capacidad_convertidor_vehiculo_electrico) ? 0 : Convert.ToDouble(parse_Capacidad_convertidor_vehiculo_electrico),
                                RendimientoElectrico = Convert.ToDouble(v.RendimientoPuroElectrico),
                                Imagen = RutaImagenNoDisponible,
                                CodigoInformeTecnico = v.CodigoInformeTecnico,
                            };
                            autosNuevos.Add(dataAutoNuevo);
                        }
                        else
                        {
                            dataAuto.Result.Modelo = v.Modelo;
                            dataAuto.Result.Traccion = v.Traccion;
                            dataAuto.Result.MarcaId = marca.Result.Id;
                            dataAuto.Result.TipoConectorAcId = tipoConectorAcId.Result.TipoConectorId;
                            dataAuto.Result.TipoConectorDcId = tipoConectorDcId.Result == null ? null : (int?)tipoConectorDcId.Result.TipoConectorId;
                            dataAuto.Result.CapacidadBateria = Convert.ToDouble(v.AcumulacionEnergiaBateria.Replace('.', ','));
                            dataAuto.Result.CapacidadInversorInternoAc = String.IsNullOrEmpty(parse_Capacidad_convertidor_vehiculo_electrico) ? 0 : Convert.ToDouble(parse_Capacidad_convertidor_vehiculo_electrico);
                            dataAuto.Result.RendimientoElectrico = Convert.ToDouble(v.RendimientoPuroElectrico);
                            dataAuto.Result.Imagen = RutaImagenNoDisponible;
                            autosactualizados.Add(dataAuto.Result);
                        }
                    }

                    _context.AddRange(autosNuevos);
                    _context.UpdateRange(autosactualizados);
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    transactionResponse.Error = true;
                    transactionResponse.Message = $"Error al actualizar Autos, {ex.Message}";
                }
            }

            return transactionResponse;
        }
    }
}
