using Cl.Gob.Energia.Ecocarga.Web.Authorization;
using Cl.Gob.Energia.Ecocarga.Web.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cl.Gob.Energia.Ecocarga.Web.Services.Interfaces
{
    public interface IAuthorizationLogicService
    {
        bool AllowedRoles(User user, AllowedRolesParameters parameters);
    }
}
