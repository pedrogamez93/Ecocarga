using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cl.Gob.Energia.Ecocarga.Web.Utils
{
    public class User
    {
        public int IdUsuario { get; set; }
        public string Usuario { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string CorreoElectronico { get; set; }
        public string Role { get; set; }
        public List<UserRole> Roles { get; set; }
        public string Empresa { get; set; }
        public int MinutosTimeout { get; set; }
        public string Token { get; set; }
    }
}
