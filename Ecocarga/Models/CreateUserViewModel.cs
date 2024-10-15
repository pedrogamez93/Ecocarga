using System.ComponentModel.DataAnnotations;

using System.Collections.Generic; // Añadir si es necesario

public class CreateUserViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty; // Asignar valor predeterminado

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty; // Asignar valor predeterminado

    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; } = string.Empty; // Asignar valor predeterminado

    [Required]
    [Display(Name = "Rol")]
    public string? Role { get; set; }
}
