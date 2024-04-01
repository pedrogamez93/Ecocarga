using Cl.Gob.Energia.Ecocarga.Data.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cl.Gob.Energia.Ecocarga.Data.Models
{
    [Table("data_marcaauto")]
    public partial class DataMarcaauto
    {
        public DataMarcaauto()
        {
            DataAuto = new HashSet<DataAuto>();
        }

        [Column("id")]
        [JsonIgnore]
        public int Id { get; set; }
        [Remote(action: "VerificarNombreAsync", controller: "MarcasAutos", AdditionalFields = nameof(Id), ErrorMessage = "Nombre ya existe en sistema.")]
        [Required(ErrorMessage = "Campo requerido")]
        [Column("nombre")]
        [JsonProperty("nombre")]
        [StringLength(30)]
        public string Nombre { get; set; }
        [Column("imagen")]
        [JsonProperty("imagen")]
        [StringLength(100)]
        public string Imagen { get; set; }

        [Display(Name = "Imagen")]
        [MaxFileSize(Constantes.MAX_FILE_SIZE)]
        [AllowedExtensions(new string[] { ".jpg", ".png", ".jpeg", ".ico" })]
        [Column("Archivo")]
        [NotMapped]
        [JsonIgnore]
        public IFormFile Archivo { get; set; }

        [InverseProperty("Marca")]
        [JsonProperty("modelos")]
        public virtual ICollection<DataAuto> DataAuto { get; set; }
    }
}
