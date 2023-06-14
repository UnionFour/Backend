using System.Security.Claims;
using Backend.DAL.Pizzeria;
using Backend.DTO;
using Backend.Services.Context;

namespace Backend.Services.Repositories;

public class UserRepository : IUserRepository
{
    public User GetMe(PizzeriaContext pizzeriaContext, ClaimsPrincipal claimsPrincipal)
    {
        throw new NotImplementedException();
    }

    public User UpdateUser(PizzeriaContext pizzeriaContext, Guid userId, UpdateUserDTO userDto)
    {
        var user = pizzeriaContext.Users?.FirstOrDefault(u => u.Userid == userId);

        if (user == null)
            throw new Exception("User not found");

        user.Name = userDto.Name ?? user.Name;
        user.Birth = userDto.Birth ?? user.Birth;
        user.Email = userDto.Email ?? user.Email;

        pizzeriaContext.Users?.Update(user);
        pizzeriaContext.SaveChanges();
        
        return user;
    }
}