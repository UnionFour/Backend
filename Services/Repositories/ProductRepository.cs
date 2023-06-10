using Backend.DAL.Pizzeria;
using Backend.Services.Context;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services.Repositories;

public class ProductRepository : IProductRepository
{
    public IQueryable<Product> GetProducts(PizzeriaContext pizzeriaContext)
    {
        return pizzeriaContext.Products
            .Include(p => p.IngredientsProducts)
            .ThenInclude(ip=> ip.Ingredient);
    }
}