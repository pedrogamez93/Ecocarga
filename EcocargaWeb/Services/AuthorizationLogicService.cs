using Cl.Gob.Energia.Ecocarga.Web.Authorization;
using Cl.Gob.Energia.Ecocarga.Web.Services.Interfaces;
using Cl.Gob.Energia.Ecocarga.Web.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cl.Gob.Energia.Ecocarga.Web.Services
{
    public class AuthorizationLogicService : IAuthorizationLogicService
    {
        public bool AllowedRoles(User user, AllowedRolesParameters parameters)
        {
            if (parameters.Roles != null)
            {
                foreach (UserRole rol in parameters.Roles)
                {
                    if (user.Roles.Exists(r => r == rol))
                        return true;
                }
                return false;
            }
            else
                return (user.Roles).Exists(r => r == parameters.Rol);
        }
    }
}
