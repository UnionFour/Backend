using Backend.DAL.Pizzeria;
using Backend.Services.Context;

namespace Backend.Services.Repositories;

public interface IProductRepository
{
    public IQueryable<Product>? GetProducts(PizzeriaContext pizzeriaContext);
}