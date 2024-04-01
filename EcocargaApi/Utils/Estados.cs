using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cl.Gob.Energia.Ecocarga.Api.Utils
{
    public static class Estados
    {
        static Dictionary<string, string> Homologacion = new Dictionary<string, string>
        {
            { "OCCUPIED", "Ocupado" },
            { "AVAILABLE", "Disponible" },
            { "FAULTED", "Fuera de Servicio" },
        };

        public static string ObtenerEstados(string estadoIngles)
        {
            var estadoEspanol = Homologacion.ContainsKey(estadoIngles)
                ? Homologacion[estadoIngles]
                : "Disponibilidad no informada";

            return estadoEspanol;
        }
        public static bool ObtenerCargas(string carga, string tipoConector)
        {
            bool estadoCarga;
            if (tipoConector == "AC")
            {
                estadoCarga = carga == "DC" ? false : true;
            }
            else
            {
                estadoCarga = carga == "AC" ? false : true;
            }

            return estadoCarga;
        }
        public enum EstadosCargadores
        {
            OCCUPIED = 0,
            AVAILABLE = 1,
            FAULTED = 2,
            UNKNOWN = 3
        }
    }
}
