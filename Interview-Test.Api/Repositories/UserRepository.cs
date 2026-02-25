using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Interview_Test.Models;
using Interview_Test.Repositories.Interfaces;

namespace Interview_Test.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DbContext _context;

    // ทำการ Inject DbContext ผ่าน Constructor เพื่อใช้ในการทำ Entity Framework
    public UserRepository(DbContext context)
    {
        _context = context;
    }

    public dynamic GetUserById(string id)
    {
        // ใช้ LINQ ดึงข้อมูลจาก Data.Users และเช็คเงื่อนไขจาก Id (Guid) หรือ UserId (string)
        var user = Data.Users.FirstOrDefault(u => 
            u.Id.ToString().Equals(id, StringComparison.OrdinalIgnoreCase) || 
            u.UserId.Equals(id, StringComparison.OrdinalIgnoreCase));

        if (user == null)
        {
            return null;
        }

        // แปลงข้อมูลและจัดโครงสร้างให้ตรงตาม ExpectResult1.json และ ExpectResult2.json
        var result = new
        {
            id = user.Id.ToString().ToUpper(),
            userId = user.UserId,
            username = user.Username,
            firstName = user.UserProfile?.FirstName,
            lastName = user.UserProfile?.LastName,
            age = user.UserProfile?.Age,
            
            roles = user.UserRoleMappings?
                .Where(m => m.Role != null)
                .Select(m => m.Role)
                .GroupBy(r => r.RoleId) // กันข้อมูล Role ซ้ำ
                .Select(g => g.First())
                .Select(r => new 
                { 
                    roleId = r.RoleId, 
                    roleName = r.RoleName 
                })
                .OrderBy(r => r.roleId) // เรียงลำดับ roleId
                .ToList(),
                
            permissions = user.UserRoleMappings?
                .Where(m => m.Role != null && m.Role.Permissions != null)
                .SelectMany(m => m.Role.Permissions)
                .Select(p => p.Permission)
                .Distinct() // กัน Permission ซ้ำ (เช่น picking-report ที่มาจาก 2 roles)
                .OrderBy(p => p) // เรียงลำดับตัวอักษรของ permission
                .ToList()
        };

        return result;
    }

    public int CreateUser(UserModel user)
    {
        // จากโจทย์ให้สร้าง User โดยใช้ Entity Framework และใช้ Data.cs 
        // หากมีการส่ง Parameter user มา จะบันทึก user นั้น แต่ถ้าส่งเป็น null จะบันทึกข้อมูลทั้งหมดจาก Data.Users แทน
        if (user != null)
        {
            _context.Set<UserModel>().Add(user);
        }
        else
        {
            _context.Set<UserModel>().AddRange(Data.Users);
        }
        
        // คืนค่าจำนวนแถว (Affected Rows) ที่ได้รับผลกระทบจากการบันทึกลง Database
        return _context.SaveChanges();
    }
}