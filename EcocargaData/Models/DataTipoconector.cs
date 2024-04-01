using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cl.Gob.Energia.Ecocarga.Data.Models
{
    [Table("data_tipoconector")]
    public partial class DataTipoconector
    {
        public DataTipoconector()
        {
            DataAutoTipoConectorAc = new HashSet<DataAuto>();
            DataAutoTipoConectorDc = new HashSet<DataAuto>();
            DataCargador = new HashSet<DataCargador>();
        }

        [Column("id")]
        [JsonIgnore]
        public int Id { get; set; }
        [Required(ErrorMessage = "Campo requerido")]
        [Column("nombre")]
        [JsonProperty("nombre")]
        [StringLength(50)]
        public string Nombre { get; set; }
        [Column("id_publico")]
        [JsonProperty("id_publico")]
        public Guid IdPublico { get; set; }

        [InverseProperty("TipoConectorAc")]
        [JsonIgnore]
        public virtual ICollection<DataAuto> DataAutoTipoConectorAc { get; set; }
        [InverseProperty("TipoConectorDc")]
        [JsonIgnore]
        public virtual ICollection<DataAuto> DataAutoTipoConectorDc { get; set; }
        [InverseProperty("TipoConector")]
        [JsonIgnore]
        public virtual ICollection<DataCargador> DataCargador { get; set; }
        [InverseProperty("TipoConector")]
        [JsonIgnore]
        public virtual ICollection<DataDiccionarioTipoConector> DataDiccionarioTipoConector { get; set; }
    }
}
