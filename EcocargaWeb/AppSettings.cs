using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cl.Gob.Energia.Ecocarga.Web
{
    public class AppSettings
    {
        public string PathAntecedentes { get; set; }
        public string PathModelos { get; set; }
        public string PathMarcas { get; set; }
        public string PathCompanias { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public string UserDir { get; set; }
        public string UserParam { get; set; }
        public string Protocol { get; set; }
        public Authentication Authentication { get; set; }
    }
    public class Authentication
    {
        public string CookieDomain { get; set; }
    }
}
