using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Cl.Gob.Energia.Ecocarga.Web.Models
{
    public class ExportarAuto
    {
        [DisplayName("Id")]
        public int Id { get; set; }
        [DisplayName("Marca")]
        public string Marca { get; set; }
        [DisplayName("Modelo")]
        public string Modelo { get; set; }
        [DisplayName("Tracción")]
        public string Traccion { get; set; }
        [DisplayName("Capacidad de bateria (en kWh)")]
        public double CapacidadBateria { get; set; }
        [DisplayName("Capacidad de inversor interno AC (en kW)")]
        public double CapacidadInversorInternoAC { get; set; }
        [DisplayName("Rendimiento eléctrico")]
        public double RendimientoElectrico { get; set; }
        [DisplayName("Tipo de conector AC")]
        public string ConectorAC { get; set; }
        [DisplayName("Tipo de conector DC")]
        public string ConectorDC { get; set; }
        [DisplayName("Código informe técnico")]
        public string CodigoInformeTecnico { get; set; }
    }
}
