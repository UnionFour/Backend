using Backend.DAL.Pizzeria;
using Backend.Services.Context;

namespace Backend.Services.Repositories;

public class ProductRepository : IProductRepository
{
    public IQueryable<Product>? GetProducts(PizzeriaContext pizzeriaContext)
    {
        return pizzeriaContext.Products;
    }
}