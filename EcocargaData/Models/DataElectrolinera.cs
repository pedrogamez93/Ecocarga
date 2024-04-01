using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Cl.Gob.Energia.Ecocarga.Data.Models
{
    [Table("data_electrolinera")]
    public partial class DataElectrolinera
    {
        public DataElectrolinera()
        {
            DataCargador = new HashSet<DataCargador>();
            DataObservacion = new HashSet<DataObservacion>();
        }

        [Column("id")]
        [JsonIgnore]
        public int Id { get; set; }
        [Remote(action: "VerificarNombreAsync", controller: "Electrolineras", AdditionalFields = nameof(Id), ErrorMessage = "Nombre ya existe en sistema.")]
        [Required(ErrorMessage = "Campo requerido")]
        [Column("nombre")]
        [JsonProperty("nombre")]
        [StringLength(60)]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Campo requerido")]
        [Range(-85, 85, ErrorMessage = "El campo Latitud debe ser numérico decimal entre -85 y 85")]
        [Column("latitud")]
        [JsonProperty("latitud")]
        public double Latitud { get; set; }
        [Required(ErrorMessage = "Campo requerido")]
        [Range(-180, 180, ErrorMessage = "El campo Longitud debe ser numérico decimal entre -180 y 180")]
        [Column("longitud")]
        [JsonProperty("longitud")]
        public double Longitud { get; set; }
        [Required(ErrorMessage = "Campo requerido")]
        [Display(Name = "Dirección")]
        [Column("direccion")]
        [JsonProperty("direccion")]
        [StringLength(100)]
        public string Direccion { get; set; }
        [Display(Name = "N° punto de carga")]
        [Required(ErrorMessage = "Campo requerido")]
        [Column("cantidad_puntos_carga")]
        [JsonProperty("cantidad_puntos_carga")]
        [Range(1, 100, ErrorMessage = "Valor debe ser mayor a 0 y menor que 100")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "El campo N° Punto de Carga debe ser numérico positivo")]
        public int CantidadPuntosCarga { get; set; }
        [Required(ErrorMessage = "Campo requerido")]
        [Column("marca")]
        [JsonProperty("marca")]
        [StringLength(30)]
        public string Marca { get; set; }
        [Display(Name = "Potencia (en kW)")]
        [Required(ErrorMessage = "Campo requerido")]
        [Column("potencia")]
        [JsonProperty("potencia")]
        [Range(0, 99999, ErrorMessage = "Valor debe ser mayor o igual a 0 y menor que 100000")]
        public double Potencia { get; set; }
        [Column("precio")]
        [JsonProperty("precio")]
        [Range(0, 99999, ErrorMessage = "Valor debe ser mayor o igual a 0 y menor que 100000")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "El campo Precio debe ser numérico positivo")]
        public double Precio { get; set; }
        [Required(ErrorMessage = "Campo requerido")]
        [Column("horario")]
        [JsonProperty("horario")]
        [StringLength(100)]
        public string Horario { get; set; }
        [Required(ErrorMessage = "Campo requerido")]
        [Column("comuna")]
        [JsonProperty("comuna")]
        [StringLength(50)]
        public string Comuna { get; set; }
        [Required(ErrorMessage = "Campo requerido")]
        [Display(Name = "Región")]
        [Column("region")]
        [JsonProperty("region")]
        [StringLength(50)]
        public string Region { get; set; }
        [Column("id_publico")]
        [JsonIgnore]
        public Guid IdPublico { get; set; }
        [Required(ErrorMessage = "Campo requerido")]
        [Display(Name = "Propietario")]
        [Column("compania_id")]
        [JsonIgnore]
        public int CompaniaId { get; set; }
        [Display(Name = "Acepta conexión AC")]
        [Column("acepta_conexion_ac")]
        [JsonProperty("acepta_conexion_ac")]
        public bool AceptaConexionAc { get; set; }
        [Display(Name = "Acepta conexión DC")]
        [Column("acepta_conexion_dc")]
        [JsonProperty("acepta_conexion_dc")]
        public bool AceptaConexionDc { get; set; }
        [Range(0, 3, ErrorMessage = "Campo requerido")]
        [Display(Name = "Id estado Electrolinera")]
        [Column("id_estado_electrolinera")]
        [JsonProperty("id_estado_electrolinera")]
        public int? IdEstadoElectrolinera { get; set; }
        [Display(Name = "Estado Electrolinera")]
        [Column("estado_electrolinera")]
        [JsonProperty("estado_electrolinera")]
        [StringLength(50)]
        public string EstadoElectrolinera { get; set; }
        [Required(ErrorMessage = "Campo requerido")]
        [Display(Name = "Modelo")]
        [Column("modelo")]
        [JsonProperty("modelo")]
        [StringLength(100)]
        public string Modelo { get; set; }
        [Required(ErrorMessage = "Campo requerido")]
        [Display(Name = "Id Electrolinera cliente")]
        [Column("id_electrolinera_cliente")]
        [JsonIgnore]
        [StringLength(100)]
        public string IdElectrolineraCliente { get; set; }
        [Required(ErrorMessage = "Campo requerido")]
        [Display(Name = "Id Electrolinera SEC")]
        [Column("id_electrolinera_sec")]
        [JsonIgnore]
        [StringLength(100)]
        public string IdElectrolineraSec { get; set; }
        [Column("coordenadas_actualizar")]
        [JsonIgnore]
        public bool CoordenadasActualizar { get; set; }

        [Display(Name = "Propietario")]
        [ForeignKey("CompaniaId")]
        [InverseProperty("DataElectrolinera")]
        [JsonProperty("compania")]
        public virtual DataCompania Compania { get; set; }
        [InverseProperty("Electrolinera")]
        [JsonProperty("cargadores")]
        public virtual ICollection<DataCargador> DataCargador { get; set; }
        [InverseProperty("Electrolinera")]
        [JsonIgnore]
        public virtual ICollection<DataObservacion> DataObservacion { get; set; }
        [JsonProperty("observaciones")]
        [NotMapped]
        public virtual string[] Observaciones
        {
            get
            {
                List<string> observacionesLista = new List<string>();
                DataObservacion.ToList().ForEach(o =>
                {
                    observacionesLista.Add(o.Mensaje);
                });

                return observacionesLista.ToArray();
            }
        }
    }
}
