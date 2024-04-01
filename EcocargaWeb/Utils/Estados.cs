using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cl.Gob.Energia.Ecocarga.Web.Utils
{
    public static class Estados
    {
        public enum EstadosCargadores
        {
            [Display(Name = "Ocupado")]
            [Description("Ocupado")]
            Ocupado = 0,
            [Display(Name = "Disponible")]
            [Description("Disponible")]
            Disponible = 1,
            [Display(Name = "Fuera de Servicio")]
            [Description("Fuera de Servicio")]
            Fuera_Servicio = 2,
            [Display(Name = "Disponibilidad no informada")]
            [Description("Disponibilidad no informada")]
            No_Informado = 3
        }
        public static HtmlString EnumDisplayNameFor(this Enum item)
        {
            var type = item.GetType();
            var member = type.GetMember(item.ToString());
            DisplayAttribute displayName = (DisplayAttribute)member[0].GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault();

            if (displayName != null)
            {
                return new HtmlString(displayName.Name);
            }

            return new HtmlString(item.ToString());
        }
    }
}
