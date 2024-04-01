# EcoCarga Web Versión .Net Core

Proyecto Web para la mantención de entidades de App móvil Ecocarga y Api de sincronización de cargadores

## Getting Started
La solución contempla dos aplicaciones web:

* **EcocargaWeb**: Corresponde a una aplicación web mediante la cual, los usuarios podrán dar mantenimiento a las entidades que soportan el sistema, tales como modelos de vehículos, electrolineras, cargadores, tipos de conectores, entre otros.
* **EcocargaApi**: Corresponde a una API REST que publicará los datos de las electrolineras. Mediante la cual, las aplicaciones móviles para Android y iOS obtendrán la información a desplegar. También estará disponible para que terceros puedan consumir sus datos.

Ambas aplicaciones accederán a la misma base de datos y compartirán los mismos modelos mediante una biblioteca de clases compartida, denominada **EcocargaData**

### Prerequisites
* [SQL Server Express Edition](https://www.microsoft.com/es-es/sql-server/sql-server-editions-express)
* [Visual Studio 2019 Community Edition](https://visualstudio.microsoft.com/es/vs/community/)
* [.Net Core 2.2 SDK](https://dotnet.microsoft.com/download/dotnet-core/2.2)
* [Entity Framework Core tools 2.2.x](https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/dotnet)

### Installing

* Descargar el repo y cargar la solución EcocargaWeb.sln con Visual Studio 2019
* Crear una base de datos en SQL Server Express
* En los proyectos EcocargaWeb y EcocargaApi, actualizar propiedad `ConnectionString` con el valor que corresponda a su base de datos.
* Utilizando la consola, situarse en la carpeta del proyecto EcocargaWeb y ejecutar el comando:
```
dotnet ef database update
```

### Coding Style

Estándar de C#, en español.


## Deployment

TBA

## Built With

* [.Net Core 2.2](https://dotnet.microsoft.com/download/dotnet-core/2.2) - Development Platform

## Contributing

Se utiliza Gitlab Flow para agregar funcionalidades. En resumen:
* Crear issue para cada cambio a realizar
* Crear rama a partir del issue.
* Trabajar en la rama creada
* Una vez que se haya terminado y se haya subido el código, crear un Merge Request para que el código sea revisado.

Para la construcción de servicios, leer [esta guía](https://restfulapi.net/resource-naming/)

Para documentar el servicio, leer [esta documentación](https://github.com/domaindrivendev/Swashbuckle.AspNetCore#getting-started). Revisar que:
* Las acciones de la api y parametros que no van en la ruta estén explícitamente decorados con "Http" y "From".
* Agregar documentación XML. Ver [documentación](https://github.com/domaindrivendev/Swashbuckle.AspNetCore#include-descriptions-from-xml-comments) al respecto.


Para agregar migraciones, seguir [esta guía](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/)


## Versioning

Usaremos [SemVer](http://semver.org/) para el versionamiento. Para ver las versiones disponibles, ver los [tags en este repositorio](https://gitlab.zeke.cl/MINENERGIA/ecocargaweb2/tags). 

## Authors

Ver la lista de [contributors](https://gitlab.zeke.cl/MINENERGIA/ecocargaweb2/contributors) que participan en ete proyecto.

