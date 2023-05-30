namespace Backend.DTO.Entities;

public class OrderDTO
{
    public Guid OrderId { get; set; }
    
    public decimal Cost { get; set; }
    
    public string? Address { get; set; }
    
    public DateOnly Createdate { get; set; }
    
    public DateOnly Preparationdate { get; set; }
    
    public DateOnly Completingdate { get; set; }
    
    public string? Promocode { get; set; }
    
    public Guid Userid { get; set; }
    
    public List<ProductDTO> Products { get; set; }
}