using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Http.Internal;

namespace Cl.Gob.Energia.Ecocarga.Web.Utils
{
    public class Funciones
    {
        public static async Task<string> GuardarArchivoAsync(IFormFile imagen, string ruta, string destino)
        {
            string dataAutoImagen = "";

            if (imagen != null && imagen.Length > 0)
            {
                string extension = Path.GetExtension(imagen.FileName);
                string fileNameUuid = Guid.NewGuid().ToString();
                var fileName = string.Concat(fileNameUuid, extension);

                string fullPathAntecedentes = Path.Combine(Directory.GetCurrentDirectory(), ruta);
                string pathModelo = destino;
                var fullFilePath = Path.Combine(fullPathAntecedentes, pathModelo, fileName);
                using (var fileSteam = new FileStream(fullFilePath, FileMode.Create))
                {
                    await imagen.CopyToAsync(fileSteam);
                }
                dataAutoImagen = Path.Combine(pathModelo, fileName);
            }

            return dataAutoImagen;
        }

        public static async Task<IFormFile> LeerArchivoAsync(string ruta)
        {
            FormFile formFile = null;

            if (ruta.Length > 0)
            {
                var fullFilePath = ruta;
                using (var fileStream = new FileStream(fullFilePath, FileMode.Open))
                {
                    var ms = new MemoryStream();
                    try
                    {
                        await fileStream.CopyToAsync(ms);
                        formFile = new FormFile(ms, 0, ms.Length, "Archivo", Path.GetFileName(ruta));
                    }
                    finally { ms.Dispose(); }
                }
            }

            return formFile;
        }
    }
}
