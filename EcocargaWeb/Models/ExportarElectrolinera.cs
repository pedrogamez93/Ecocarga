using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Cl.Gob.Energia.Ecocarga.Web.Models
{
    public class ExportarElectrolinera
    {
        [DisplayName("Id")]
        public int Id { get; set; }
        [DisplayName("Propietario")]
        public string Compania { get; set; }
        [DisplayName("Nombre Electrolinera")]
        public string NombreElectrolinera { get; set; }
        [DisplayName("Región")]
        public string Region { get; set; }
        [DisplayName("Comuna")]
        public string Comuna { get; set; }
        [DisplayName("Dirección")]
        public string Direccion { get; set; }
        [DisplayName("Latitud")]
        public double Latitud { get; set; }
        [DisplayName("Longitud")]
        public double Longitud { get; set; }
        [DisplayName("N° punto de carga")]
        public int PuntosCarga { get; set; }
        [DisplayName("Marca")]
        public string Marca { get; set; }
        [DisplayName("Modelo")]
        public string Modelo { get; set; }
        [DisplayName("Potencia (en kW) Electrolinera")]
        public double Potencia { get; set; }
        [DisplayName("Acepta conexión AC")]
        public Boolean AceptaConexionAC { get; set; }
        [DisplayName("Acepta conexión DC")]
        public Boolean AceptaConexionDC { get; set; }
        [DisplayName("Horario")]
        public string Horario { get; set; }
        [DisplayName("Estado Electrolinera")]
        public string EstadoElectrolinera { get; set; }
        [DisplayName("Id Electrolinera cliente")]
        public string IdElectrolineraCliente { get; set; }
        [DisplayName("Id Electrolinera SEC")]
        public string IdElectrolineraSEC { get; set; }
        [DisplayName("Id cargador")]
        public int IdCargador { get; set; }
        [DisplayName("Tipo conector")]
        public string TipoConector { get; set; }
        [DisplayName("Tipo corriente")]
        public string TipoCorriente { get; set; }
        [DisplayName("Cable")]
        public Boolean Cable { get; set; }
        [DisplayName("Potencia (en kW) cargador")]
        public double PotenciaCargador { get; set; }
        [DisplayName("Estado cargador")]
        public string EstadoCargador { get; set; }
        [DisplayName("Id cargador cliente")]
        public string IdCargadorCliente { get; set; }
        [DisplayName("Id cargador SEC")]
        public string IdCargadorSEC { get; set; }
    }
}
