using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cl.Gob.Energia.Ecocarga.Api.Models
{
    public class ConsumoVehicular
    {
        public string Id { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Traccion { get; set; }
        public string Transmision { get; set; }
        public string Combustible { get; set; }
        public string Propulsion { get; set; }
        public string Cilindrada { get; set; }
        public string Carroceria { get; set; }
        [JsonProperty("Codigo_informe_tecnico")]
        public string CodigoInformeTecnico { get; set; }
        [JsonProperty("Fecha_homologacion")]
        public string FechaHomologacion { get; set; }
        [JsonProperty("Categoria_vehiculo")]
        public string CategoriaVehiculo { get; set; }
        [JsonProperty("Empresa_homologacion")]
        public string EmpresaHomologacion { get; set; }
        [JsonProperty("Norma_emisiones")]
        public string NormaEmisiones { get; set; }
        public string Co2 { get; set; }
        [JsonProperty("Rendimiento_ciudad")]
        public string RendimientoCiudad { get; set; }
        [JsonProperty("Rendimiento_carretera")]
        public string RendimientoCarretera { get; set; }
        [JsonProperty("Rendimiento_mixto")]
        public string RendimientoMixto { get; set; }
        [JsonProperty("Rendimiento_puro_electrico")]
        public string RendimientoPuroElectrico { get; set; }
        [JsonProperty("Rendimiento_enchufable_combustible")]
        public string RendimientoEnchufableCombustible { get; set; }
        [JsonProperty("Rendimiento_enchufable_electrico")]
        public string RendimientoEnchufableElectrico { get; set; }
        [JsonProperty("Tipo_de_conector_ac")]
        public string TipoDeConectorAc { get; set; }
        [JsonProperty("Tipo_de_conector_dc")]
        public string TipoDeConectorDc { get; set; }
        [JsonProperty("Acumulacion_energia_bateria")]
        public string AcumulacionEnergiaBateria { get; set; }
        [JsonProperty("Capacidad_convertidor_vehiculo_electrico")]
        public string CapacidadConvertidorVehiculoElectrico { get; set; }
        public string Autonomia { get; set; }
        public string Rendimiento { get; set; }
        [JsonProperty("Created_at")]
        public string CreatedAt { get; set; }
        [JsonProperty("Updated_at")]
        public string UpdatedAt { get; set; }
    }
}
