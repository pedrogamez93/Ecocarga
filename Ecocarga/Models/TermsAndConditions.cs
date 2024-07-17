namespace Ecocarga.Models
{
    public class TermsAndConditions
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty; // Asignar valor predeterminado
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }


}
