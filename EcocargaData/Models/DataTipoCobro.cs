using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cl.Gob.Energia.Ecocarga.Data.Models
{
    [Table("data_tipocobro")]
    public partial class DataTipocobro
    {
        [Key]
        [Column("id")]
        [JsonIgnore]
        public int Id { get; set; }
        [Required(ErrorMessage = "Campo requerido")]
        [Column("nombre")]
        [JsonProperty("nombre")]
        [StringLength(50)]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Campo requerido")]
        [Column("unidad_cobro")]
        [Display(Name = "Unidad cobro")]
        [StringLength(50)]
        public string UnidadCobro { get; set; }
        [Column("cargador_id")]
        [JsonIgnore]
        [Display(Name = "Cargador")]
        public int CargadorId { get; set; }
        [Required(ErrorMessage = "Campo requerido")]
        [Column("precio")]
        [JsonProperty("precio")]
        [Display(Name = "Precio")]
        [Range(0, 99999, ErrorMessage = "Valor debe ser mayor o igual a 0 y menor que 100000")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "El campo Precio debe ser numérico positivo")]
        public int Precio { get; set; }

        [Display(Name = "Cargador")]
        [ForeignKey("CargadorId")]
        [InverseProperty("DataTipocobroCargador")]
        [JsonIgnore]
        public virtual DataCargador Cargador { get; set; }
    }
}
