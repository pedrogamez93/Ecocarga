using System;

namespace Ecocarga.Models
{
    public class UserAction
    {
        public int Id { get; set; }  // ID del registro
        public string UserId { get; set; }  // ID del usuario
        public string ActionType { get; set; }  // Tipo de acción (Inicio de sesión, Cierre de sesión, etc.)
        public string ActionDescription { get; set; }  // Descripción detallada de la acción
        public DateTime ActionTime { get; set; }  // Fecha y hora de la acción
    }
}
