using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cl.Gob.Energia.Ecocarga.Data.Models
{
    [Table("data_cargador")]
    public partial class DataCargador
    {
        [Column("id")]
        [JsonIgnore]
        public int Id { get; set; }
        [Column("cable")]
        [JsonProperty("cable")]
        public bool Cable { get; set; }
        [Display(Name = "Electrolinera")]
        [Column("electrolinera_id")]
        [JsonIgnore]
        public int ElectrolineraId { get; set; }
        [Required(ErrorMessage = "Campo requerido")]
        [Display(Name = "Tipo conector")]
        [Column("tipo_conector_id")]
        [JsonIgnore]
        public int TipoConectorId { get; set; }
        [Display(Name = "Tipo corriente")]
        [Required(ErrorMessage = "Campo requerido")]
        [Column("tipo_corriente")]
        [JsonProperty("tipo_corriente")]
        [StringLength(2)]
        public string TipoCorriente { get; set; }
        [Display(Name = "Potencia (en kW)")]
        [Required(ErrorMessage = "Campo requerido")]
        [Column("potencia")]
        [JsonProperty("potencia")]
        [Range(0, 99999, ErrorMessage = "Valor debe ser mayor o igual a 0 y menor que 100000")]
        public double Potencia { get; set; }
        [Range(0, 3, ErrorMessage = "Campo requerido")]
        [Display(Name = "Id estado cargador")]
        [Column("id_estado_cargador")]
        [JsonProperty("id_estado_cargador")]
        public int? IdEstadoCargador { get; set; }
        [Display(Name = "Estado cargador")]
        [Column("estado_cargador")]
        [JsonProperty("estado_cargador")]
        [StringLength(50)]
        public string EstadoCargador { get; set; }
        [Required(ErrorMessage = "Campo requerido")]
        [Display(Name = "Id cargador cliente")]
        [Column("id_cargador_cliente")]
        [JsonIgnore]
        [StringLength(100)]
        public string IdCargadorCliente { get; set; }
        [Required(ErrorMessage = "Campo requerido")]
        [Display(Name = "Id cargador SEC")]
        [Column("id_cargador_sec")]
        [JsonIgnore]
        [StringLength(100)]
        public string IdCargadorSec { get; set; }

        [Display(Name = "Electrolinera")]
        [ForeignKey("ElectrolineraId")]
        [InverseProperty("DataCargador")]
        [JsonIgnore]
        public virtual DataElectrolinera Electrolinera { get; set; }

        [Display(Name = "Tipo conector")]
        [ForeignKey("TipoConectorId")]
        [InverseProperty("DataCargador")]
        [JsonProperty("tipo_conector")]
        public virtual DataTipoconector TipoConector { get; set; }

        [Display(Name = "Tipo conector")]
        [InverseProperty("Cargador")]
        [JsonProperty("tipo_precio")]
        public virtual ICollection<DataTipocobro> DataTipocobroCargador { get; set; }
    }
}
