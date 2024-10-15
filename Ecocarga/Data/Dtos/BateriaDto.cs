using CsvHelper.Configuration.Attributes;

namespace Ecocarga.Data.Dtos
{
    public class BateriaDto
    {
        [Name("Capacidad")]
        public int Capacidad { get; set; }
    }
}
