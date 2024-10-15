using System.ComponentModel.DataAnnotations.Schema;

namespace Ecocarga.Models
{
    public class Bateria
    {
        public int Id { get; set; }
        
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Capacidad { get; set; } // Cambio a decimal
    }
}
