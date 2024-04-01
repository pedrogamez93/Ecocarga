using Cl.Gob.Energia.Ecocarga.Web.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Cl.Gob.Energia.Ecocarga.Web.Services.Interfaces
{
    public interface IUserService
    {
        User GetUser();
        bool IsAdmin();
        bool IsElectrolinera();
        bool IsSec();
        string GetName();
        string GetPerfil();
    }
}
