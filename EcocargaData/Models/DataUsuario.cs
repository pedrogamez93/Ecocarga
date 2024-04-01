using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cl.Gob.Energia.Ecocarga.Data.Models
{
    [Table("data_usuario")]
    public partial class DataUsuario
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("usuario")]
        [StringLength(100)]
        public string Usuario { get; set; }
        [Column("perfil_id")]
        public int PerfilId { get; set; }
        [Column("compania_id")]
        public int? CompaniaId { get; set; }

        [ForeignKey("CompaniaId")]
        [InverseProperty("DataUsuario")]
        public virtual DataCompania Compania { get; set; }
    }
}
