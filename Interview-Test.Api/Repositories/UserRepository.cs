using Interview_Test.Models;
using Interview_Test.Repositories.Interfaces;
using Interview_Test.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Interview_Test.Repositories;

public class UserRepository : IUserRepository
{
    private readonly InterviewTestDbContext _context;

    public UserRepository(InterviewTestDbContext context)
    {
        _context = context;
    }

    public dynamic GetUserById(string id)
    {
        var user = _context.UserTb
            .Include(u => u.UserProfile)
            .Include(u => u.UserRoleMappings)
                .ThenInclude(urm => urm.Role)
                    .ThenInclude(r => r.Permissions)
            .FirstOrDefault(u => u.Id == Guid.Parse(id));

        if (user == null)
        {
            return null;
        }

        return new
        {
            id = user.Id.ToString(),
            userId = user.UserId,
            username = user.Username,
            firstName = user.UserProfile.FirstName,
            lastName = user.UserProfile.LastName,
            age = user.UserProfile.Age,
            roles = user.UserRoleMappings.Select(urm => new
            {
                roleId = urm.Role.RoleId,
                roleName = urm.Role.RoleName
            }).ToList(),
            permissions = user.UserRoleMappings
                .SelectMany(urm => urm.Role.Permissions)
                .Select(p => p.Permission)
                .Distinct()
                .ToList()
        };
    }

    public int CreateUser(UserModel user)
    {
        _context.UserTb.Add(user);
        int affected = _context.SaveChanges();
        return affected;
    }
}