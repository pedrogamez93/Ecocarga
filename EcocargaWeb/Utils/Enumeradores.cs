using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Cl.Gob.Energia.Ecocarga.Web.Utils
{

    public enum UserRole
    {
        [Description("Administrador")]
        Administrador,
        [Description("Compañía")]
        Electrolinera,
        [Description("SEC")]
        Sec
    }

    public class Roles
    {
        public static string GetDescription(UserRole value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            if (attributes != null && attributes.Any())
            {
                return attributes.First().Description;
            }

            return value.ToString();
        }
    }
    public enum TiposDeOrdenamiento
    {
        OrdenPorDefecto,
        ModeloDesc,
        MarcaAsc,
        MarcaDesc,
        NombreDesc,
        CompaniaAsc,
        CompaniaDesc,
        ComunaAsc,
        ComunaDesc,
        RegionAsc,
        RegionDesc
    }
}
