using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cl.Gob.Energia.Ecocarga.Web.Utils
{
    public class Constantes
    {
        public const int REGISTROS_PAGINADO = 10;
        public const string PROTOCOLO_LDAP = "LDAP";
        public const string PROTOCOLO_ACTIVE_DIRECTORY = "ActiveDirectory";
        public const string IDENTIDAD = "ModeloUsuario";
        public const string TIMEOUT_SESION = "TimeoutSesion";
    }
    public class Mensajes
    {
        public const string MENSAJE_ACTUALIZACION = " ha sido actualizado correctamente";
        public const string MENSAJE_BORRADO = " ha sido borrado correctamente";
        public const string MENSAJE_INSERCION = " ha sido creado correctamente";
        public const string MENSAJE_ALERTA_AUTO_CARGADORES = " no puede ser eliminado, aún existen cargadores y/o autos asociados al registro.";
        public const string MENSAJE_ALERTA_AUTO = " no puede ser eliminado, aún existen autos asociados al registro.";
        public const string MENSAJE_ALERTA_ELECTROLINERA = " no puede ser eliminado, aún existen electrolineras y/o usuarios asociados al registro.";
        public const string MENSAJE_ALERTA_TIPO_CONECTOR = "Debe seleccionar a lo menos un tipo de conector, ya sea AC y/o DC";
    }
}
