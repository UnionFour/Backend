using System.ComponentModel.DataAnnotations;

namespace Backend.DTO;

public class UpdateUserDTO
{
    public string? Name { get; set; }
    
    public DateOnly? Birth { get; set; }
    
    [EmailAddress]
    public string? Email { get; set; }
}