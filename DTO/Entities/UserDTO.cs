using System.ComponentModel.DataAnnotations;
using Backend.DAL.Pizzeria;

namespace Backend.DTO.Entities;

public class UserDTO
{
    public Guid Userid { get; set; }

    public string? Name { get; set; }

    public string? Address { get; set; }

    public string Phone { get; set; } = null!;

    public DateOnly? Birth { get; set; }

    public string? Email { get; set; }

    public long? Gamepoints { get; set; }

    public ICollection<Order> Orders { get; set; }
}