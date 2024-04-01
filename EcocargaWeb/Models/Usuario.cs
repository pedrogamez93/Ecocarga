using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cl.Gob.Energia.Ecocarga.Web.Models
{
    public class Usuario
    {
        [Display(Name = "Usuario")]
        [Required(ErrorMessage = "Campo requerido")]
        public string User { get; set; }
        [Display(Name = "Password")]
        [Required(ErrorMessage = "Campo requerido")]
        public string Password { get; set; }
    }
}
