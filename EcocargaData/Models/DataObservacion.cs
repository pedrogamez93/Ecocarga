using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cl.Gob.Energia.Ecocarga.Data.Models
{
    [Table("data_observacion")]
    public partial class DataObservacion
    {
        [Column("id")]
        [JsonIgnore]
        public int Id { get; set; }
        [Required(ErrorMessage = "Campo requerido")]
        [Column("mensaje")]
        [JsonProperty("mensaje")]
        [StringLength(150)]
        public string Mensaje { get; set; }
        [Column("electrolinera_id")]
        [Display(Name = "Electrolinera")]
        [JsonIgnore]
        public int ElectrolineraId { get; set; }

        [Display(Name = "Electrolinera")]
        [ForeignKey("ElectrolineraId")]
        [InverseProperty("DataObservacion")]
        [JsonIgnore]
        public virtual DataElectrolinera Electrolinera { get; set; }
    }
}
