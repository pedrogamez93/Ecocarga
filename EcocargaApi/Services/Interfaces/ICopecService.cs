using Cl.Gob.Energia.Ecocarga.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cl.Gob.Energia.Ecocarga.Api.Services.Interfaces
{
    public interface ICopecService
    {
        Task<List<Copec>> ConsumirAPI();
        Task<TransactionResponse> ActualizarElectrolineras(List<Copec> cargadoresCopec);
        Task UpdateCopec();
    }
}
