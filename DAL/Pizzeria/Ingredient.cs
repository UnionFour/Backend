using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.DAL.Pizzeria;

[Table("ingredients")]
public class Ingredient
{
    [Key]
    [Column("ingredientid")]
    public Guid IngredientID { get; set; }
    
    [Column("name")]
    public string Name { get; set; }
    
    [Column("price")]
    public Decimal? Price { get; set; }

    public ICollection<Product> Products { get; set; } = new List<Product>();

    public ICollection<IngredientsProducts> IngredientsProducts { get; set; } = new List<IngredientsProducts>();
}