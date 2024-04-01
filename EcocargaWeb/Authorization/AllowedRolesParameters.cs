using Cl.Gob.Energia.Ecocarga.Web.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cl.Gob.Energia.Ecocarga.Web.Authorization
{
    public class AllowedRolesParameters
    {
        public UserRole Rol { get; set; }
        public UserRole[] Roles { get; set; }
    }
}
