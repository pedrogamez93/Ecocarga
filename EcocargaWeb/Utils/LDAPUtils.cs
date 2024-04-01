using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.DirectoryServices;

namespace Cl.Gob.Energia.Ecocarga.Web.Utils
{
    public class LDAPUtils
    {
        public static User ValidarLogin(string usuario, string clave, IOptions<AppSettings> _settings, ILogger _logger)
        {
            string host = _settings.Value.Host;
            string port = _settings.Value.Port;
            string userDir = _settings.Value.UserDir;
            string userParam = _settings.Value.UserParam;
            string protocol = _settings.Value.Protocol;

            string hostPath = String.Format("LDAP://{0}/{1}", host, userDir);
            if (protocol.Equals(Constantes.PROTOCOLO_LDAP))
            {
                hostPath = String.Format("LDAP://{0}:{1}/{2}={3},{4}", host, port, userParam, usuario, userDir);
            }

            try
            {
                User user = null;

                if (protocol.Equals(Constantes.PROTOCOLO_ACTIVE_DIRECTORY))
                {
                    DirectoryEntry objDE = new DirectoryEntry(hostPath, usuario, clave);

                    var directorySearcher = new DirectorySearcher(objDE)
                    {
                        Filter = "(&(objectClass=user)(" + userParam + "=" + usuario + "))"
                    };
                    var searchResult = directorySearcher.FindOne();

                    user = new User()
                    {
                        Nombres = searchResult.Properties["givenname"][0].ToString(),
                        Apellidos = searchResult.Properties["sn"][0].ToString(),
                        Usuario = usuario,
                        CorreoElectronico = searchResult.Properties["userprincipalname"][0].ToString()
                    };

                    try
                    {
                        user.CorreoElectronico = searchResult.Properties["mail"][0].ToString();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                    }

                }
                else if (protocol.Equals(Constantes.PROTOCOLO_LDAP))
                {
                    string strPath = String.Format("{0}={1},{2}", userParam, usuario, userDir);
                    DirectoryEntry objDE = new DirectoryEntry(hostPath, strPath, clave, AuthenticationTypes.FastBind);

                    user = new User()
                    {
                        Nombres = objDE.Properties["givenName"].Value.ToString(),
                        Apellidos = objDE.Properties["sn"].Value.ToString(),
                        Usuario = usuario,
                        CorreoElectronico = objDE.Properties["mail"].Value.ToString()
                    };
                }

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }
    }
}
