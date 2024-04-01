using Cl.Gob.Energia.Ecocarga.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cl.Gob.Energia.Ecocarga.Api.Services.Interfaces
{
    public interface IConsumoSecService
    {
        Task<List<ConsumoSec>> ConsumirAPI();
        Task UpdateConsumoSec();
    }
}
