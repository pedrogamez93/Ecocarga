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
    [Table("data_auto")]
    public partial class DataAuto
    {
        [Key]
        [Column("id")]
        [JsonIgnore]
        public int Id { get; set; }
        [Remote(action: "VerificarNombreAsync", controller: "Autos", AdditionalFields = nameof(Id), ErrorMessage = "Nombre ya existe en sistema.")]
        [Required(ErrorMessage = "Campo requerido")]
        [Column("modelo")]
        [JsonProperty("modelo")]
        [StringLength(100)]
        public string Modelo { get; set; }
        [Required(ErrorMessage = "Campo requerido")]
        [Column("traccion")]
        [JsonProperty("traccion")]
        [StringLength(10)]
        [Display(Name = "Tracción")]
        public string Traccion { get; set; }
        [Required(ErrorMessage = "Campo requerido")]
        [Column("id_publico")]
        [JsonProperty("id_publico")]
        public Guid IdPublico { get; set; }
        [Required(ErrorMessage = "Campo requerido")]
        [Column("marca_id")]
        [JsonIgnore]
        [Display(Name = "Marca")]
        public int MarcaId { get; set; }
        [Column("tipo_conector_ac_id")]
        [JsonIgnore]
        [Display(Name = "Tipo de conector AC")]
        public int? TipoConectorAcId { get; set; }
        [Column("tipo_conector_dc_id")]
        [JsonIgnore]
        [Display(Name = "Tipo de conector DC")]
        public int? TipoConectorDcId { get; set; }
        [Required(ErrorMessage = "Campo requerido")]
        [Column("capacidad_bateria")]
        [JsonProperty("capacidad_bateria")]
        [Display(Name = "Capacidad de bateria (en kWh)")]
        public double CapacidadBateria { get; set; }
        [Required(ErrorMessage = "Campo requerido")]
        [Column("capacidad_inversor_interno_ac")]
        [JsonProperty("capacidad_inversor_interno_ac")]
        [Display(Name = "Capacidad de inversor interno AC (en kW)")]
        public double CapacidadInversorInternoAc { get; set; }
        [Required(ErrorMessage = "Campo requerido")]
        [Column("rendimiento_electrico")]
        [JsonProperty("rendimiento_electrico")]
        [Display(Name = "Rendimiento eléctrico")]
        public double RendimientoElectrico { get; set; }
        [Column("imagen")]
        [JsonProperty("imagen")]
        [StringLength(100)]
        public string Imagen { get; set; }
        [Required(ErrorMessage = "Campo requerido")]
        [Column("codigo_informe_tecnico")]
        [Display(Name = "Código informe técnico")]
        [JsonProperty("codigo_informe_tecnico")]
        [StringLength(50)]
        public string CodigoInformeTecnico { get; set; }

        [Display(Name = "Imagen")]
        [MaxFileSize(Constantes.MAX_FILE_SIZE)]
        [AllowedExtensions(new string[] { ".jpg", ".png", ".jpeg", ".ico" })]
        [Column("Archivo")]
        [NotMapped]
        [JsonIgnore]
        public IFormFile Archivo { get; set; }

        [ForeignKey("MarcaId")]
        [InverseProperty("DataAuto")]
        [JsonIgnore]
        public virtual DataMarcaauto Marca { get; set; }
        [ForeignKey("TipoConectorAcId")]
        [JsonProperty("tipo_conector_ac")]
        [InverseProperty("DataAutoTipoConectorAc")]
        public virtual DataTipoconector TipoConectorAc { get; set; }
        [ForeignKey("TipoConectorDcId")]
        [JsonProperty("tipo_conector_dc")]
        [InverseProperty("DataAutoTipoConectorDc")]
        public virtual DataTipoconector TipoConectorDc { get; set; }
    }
}
