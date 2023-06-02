using System.Security.Claims;
using Backend.DAL.Pizzeria;
using Backend.DTO.Entities;
using Backend.Services.Context;

namespace Backend.Services.Repositories;

public class UserRepository : IUserRepository
{
    public User GetMe(PizzeriaContext pizzeriaContext, ClaimsPrincipal claimsPrincipal)
    {
        throw new NotImplementedException();
    }

    public User UpdateUser(PizzeriaContext pizzeriaContext, UserDTO userDto)
    {
        throw new NotImplementedException();
    }
}