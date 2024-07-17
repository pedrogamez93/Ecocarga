using System.ComponentModel.DataAnnotations;

public class LoginViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty; // Asignar valor predeterminado

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty; // Asignar valor predeterminado

    [Display(Name = "Remember me?")]
    public bool RememberMe { get; set; }
}

