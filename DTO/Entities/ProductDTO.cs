
namespace Backend.DTO.Entities;

public class ProductDTO
{
    public Guid ProductId { get; set; }

    public string Name { get; set; }

    public decimal? Price { get; set; }

    public string? Category { get; set; }
    
    public int Amount { get; set; }

}