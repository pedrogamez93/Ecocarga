using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cl.Gob.Energia.Ecocarga.Data.Models
{
    [Table("data_diccionariotipoconector")]
    public partial class DataDiccionarioTipoConector
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("tipo_conector_id")]
        public int TipoConectorId { get; set; }
        [Column("tipo_conector_externo")]
        [StringLength(50)]
        public string TipoConectorExterno { get; set; }
        [Column("entidad_externo")]
        [StringLength(50)]
        public string EntidadExterna { get; set; }

        [ForeignKey("TipoConectorId")]
        [InverseProperty("DataDiccionarioTipoConector")]
        public virtual DataTipoconector TipoConector { get; set; }
    }
}
