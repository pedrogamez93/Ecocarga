using Cl.Gob.Energia.Ecocarga.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cl.Gob.Energia.Ecocarga.Api.Services.Interfaces
{
    public interface IConsumoVehicularService
    {
        Task<List<ConsumoVehicular>> ConsumirAPI();
        Task<TransactionResponse> ActualizarMarcasAutos(IEnumerable<ConsumoVehicular> vehiculosElectricos);
        Task<TransactionResponse> ActualizarAutos(IEnumerable<ConsumoVehicular> vehiculosElectricos);
        Task UpdateConsumoVehicular();
    }
}
