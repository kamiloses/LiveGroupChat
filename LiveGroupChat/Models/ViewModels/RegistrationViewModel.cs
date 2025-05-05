using System.ComponentModel.DataAnnotations;

namespace LiveGroupChat.ViewModels;

public class RegistrationViewModel {
    
    
    [Required(ErrorMessage = "First name is required.")]
    [StringLength(20, ErrorMessage = "First name can't be longer than 20 characters.")]
    [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First name can only contain letters.")]
    public string? FirstName { get; set; }

    [Required(ErrorMessage = "Last name is required.")]
    [StringLength(20, ErrorMessage = "Last name can't be longer than 20 characters.")]
    [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Last name can only contain letters.")]
    public string? LastName { get; set; }
    
    // atrybut taki email już istieje
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string? Email { get; set; }
    
    [Required(ErrorMessage = "Password is required.")]
    [StringLength(20, MinimumLength = 6, ErrorMessage = "Password is invalid.")]
    public string? Password { get; set; }
    
    
    
}