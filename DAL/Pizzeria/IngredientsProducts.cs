using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.DAL.Pizzeria;

[Table("ingredients_products")]
public class IngredientsProducts
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }
    
    [Column("amount")]
    public double? Amount { get; set; }
    
    [Column("amounttype")]
    public string AmountType { get; set; }
    
    [Column("ingredientid")]
    public Guid IngredientID { get; set; }
    
    public virtual Ingredient? Ingredient { get; set; }
    
    [Column("productid")]
    public Guid ProductID { get; set; }
    
    [Column("productname")]
    public string ProductName { get; set; }
    
    public virtual Product? Product { get; set; }
}