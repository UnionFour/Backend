using Backend.DAL.Pizzeria.Enums;

namespace Backend.DTO.Entities;

public class OrderDTO
{
    public Guid? OrderId { get; set; }
    
    public decimal? Cost { get; set; }
    
    public string? Address { get; set; }
    
    public DateTime? Createdate { get; set; }
    
    public DateTime? Preparationdate { get; set; }
    
    public DateTime? Completingdate { get; set; }
    
    public string? Promocode { get; set; }
    
    public OrderExtradition Extradition { get; set; }
    
    public Guid Userid { get; set; }
    
    public List<ProductDTO> Products { get; set; }
}