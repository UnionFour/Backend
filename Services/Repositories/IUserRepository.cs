using System.Security.Claims;
using Backend.DAL.Pizzeria;
using Backend.DTO.Entities;
using Backend.Services.Context;

namespace Backend.Services.Repositories;

public interface IUserRepository
{
    public User GetMe(PizzeriaContext pizzeriaContext, ClaimsPrincipal claimsPrincipal);
    public User UpdateUser(PizzeriaContext pizzeriaContext, UserDTO userDto);
}