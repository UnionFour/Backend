using System.Security.Claims;
using Backend.DAL.Pizzeria;
using Backend.DTO;
using Backend.DTO.Entities;
using Backend.Services.Context;
using Backend.Types.Mutation;

namespace Backend.Services.Repositories;

public interface IUserRepository
{
    public User GetMe(PizzeriaContext pizzeriaContext, ClaimsPrincipal claimsPrincipal);
    public User UpdateUser(PizzeriaContext pizzeriaContext, Guid userId, UpdateUserDTO userDto);
}