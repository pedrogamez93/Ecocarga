using System.ComponentModel.DataAnnotations;

public class EditUserViewModel
{
    public string Id { get; set; } = string.Empty; // Asignar valor predeterminado

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty; // Asignar valor predeterminado

    [DataType(DataType.Password)]
    [Display(Name = "New Password")]
    public string Password { get; set; } = string.Empty; // Asignar valor predeterminado

    [DataType(DataType.Password)]
    [Display(Name = "Confirm New Password")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; } = string.Empty; // Asignar valor predeterminado
}
