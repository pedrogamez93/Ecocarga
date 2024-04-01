using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cl.Gob.Energia.Ecocarga.Api
{
    public class AppSettings
    {
        public string ApiConsumoVehicular { get; set; }
        public string KeyApiConsumoVehicular { get; set; }
        public string PathAntecedentes { get; set; }
        public string ApiCopec { get; set; }
        public string PathImagenNoDisponibleModelos { get; set; }
        public string PathImagenNoDisponibleMarcas { get; set; }
        public string ApiSEC { get; set; }
        public int MinutosEsperaSEC { get; set; }
        public int MinutosEsperaElectrolinera { get; set; }
        public int MinutosEsperaConsumoVehicular { get; set; }

    }
    public class EmailSettings
    {
        public string MailServer { get; set; }
        public int MailPort { get; set; }
        public string SenderName { get; set; }
        public string Sender { get; set; }
        public string Password { get; set; }
        public string Receiver { get; set; }
        public string Subject { get; set; }
    }
}
