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
    [Table("data_compania")]
    public partial class DataCompania
    {
        public DataCompania()
        {
            DataElectrolinera = new HashSet<DataElectrolinera>();
            DataUsuario = new HashSet<DataUsuario>();
        }

        [Column("id")]
        [JsonIgnore]
        public int Id { get; set; }
        [Required(ErrorMessage = "Campo requerido")]
        [Column("nombre")]
        [JsonProperty("nombre")]
        [StringLength(30)]
        public string Nombre { get; set; }
        [Remote(action: "VerificarRutAsync", controller: "Companias", AdditionalFields = nameof(Id), ErrorMessage = "Rut propietario no valido o ya existente en sistema.")]
        [Required(ErrorMessage = "Campo requerido")]
        [Display(Name = "Rut Propietario")]
        [Column("rut_propietario")]
        [JsonProperty("rut_propietario")]
        [StringLength(15)]
        public string RutPropietario { get; set; }
        [Display(Name = "Url web")]
        [Column("url_in_image")]
        [JsonProperty("url_in_image")]
        [StringLength(200)]
        public string UrlInImage { get; set; }
        [Display(Name = "Logo")]
        [Column("url_image")]
        [JsonProperty("url_image")]
        [StringLength(100)]
        public string UrlImage { get; set; }

        [Display(Name = "Logo")]
        [MaxFileSize(Constantes.MAX_FILE_SIZE)]
        [AllowedExtensions(new string[] { ".jpg", ".png", ".jpeg", ".ico" })]
        [Column("Archivo")]
        [NotMapped]
        [JsonIgnore]
        public IFormFile Archivo { get; set; }

        [InverseProperty("Compania")]
        [JsonIgnore]
        public virtual ICollection<DataElectrolinera> DataElectrolinera { get; set; }

        [InverseProperty("Compania")]
        [JsonIgnore]
        public virtual ICollection<DataUsuario> DataUsuario { get; set; }
    }
}
