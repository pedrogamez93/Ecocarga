using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cl.Gob.Energia.Ecocarga.Api.Models
{
    public class ConsumoSec
    {
        [JsonProperty("Nombre")]
        public string Nombre { get; set; }
        [JsonProperty("Tipo_Instalacion")]
        public string TipoInstalacion { get; set; }
        [JsonProperty("Inscripcion")]
        public string Inscripcion { get; set; }
        [JsonProperty("Latitud")]
        public float Latitud { get; set; }
        [JsonProperty("Longitud")]
        public float Longitud { get; set; }
        [JsonProperty("Rut_Propietario")]
        public string Rut_Propietario { get; set; }
        [JsonProperty("Propietario")]
        public string Propietario { get; set; }
        [JsonProperty("Empresa_Dx")]
        public string EmpresaDx { get; set; }
        [JsonProperty("Region")]
        public string Region { get; set; }
        [JsonProperty("Comuna")]
        public string Comuna { get; set; }
        [JsonProperty("Horario")]
        public string Horario { get; set; }
        [JsonProperty("Direccion")]
        public string Direccion { get; set; }
        [JsonProperty("Cantidad_Cargadores")]
        public int CantidadCargadores { get; set; }
        public CargadoresSec[] Cargadores { get; set; }
    }

    public class CargadoresSec
    {
        [JsonProperty("Carga")]
        public string Carga { get; set; }
        [JsonProperty("Nombre")]
        public string Nombre { get; set; }
        [JsonProperty("Marca")]
        public string Marca { get; set; }
        [JsonProperty("Modelo")]
        public string Modelo { get; set; }
        [JsonProperty("Id_Cargador")]
        public int IdCargador { get; set; }
        [JsonProperty("Potencia")]
        public float Potencia { get; set; }
        [JsonProperty("Cantidad_Conectores")]
        public int CantidadConectores { get; set; }
        public ConectoresSec[] Conectores { get; set; }
    }

    public class ConectoresSec
    {
        [JsonProperty("Tipo")]
        public string Tipo { get; set; }
        [JsonProperty("Id_Conector")]
        public int IdConector { get; set; }
        [JsonProperty("Potencia")]
        public float Potencia { get; set; }
        [JsonProperty("Cable")]
        public string Cable { get; set; }
        [JsonProperty("Carga")]
        public string Carga { get; set; }
    }
}

