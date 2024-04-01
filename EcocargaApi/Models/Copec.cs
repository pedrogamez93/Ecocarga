using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cl.Gob.Energia.Ecocarga.Api.Models
{
    public class Copec
    {
        public string Status { get; set; }
        public string IdCargador { get; set; }
        public Conectores[] Conectores { get; set; }
    }
    public class Conectores
    {
        public string IdConector { get; set; }
        public string Status { get; set; }
        public string Unidad { get; set; }
        public int Precio { get; set; }
    }
}
