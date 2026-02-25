using Interview_Test.Models;
using Interview_Test.Repositories;
using Interview_Test.Repositories.Interfaces; // 1. เพิ่ม using สำหรับเรียกใช้ Interface
using Microsoft.AspNetCore.Mvc;

namespace Interview_Test.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    // 2. ประกาศตัวแปร _userRepository
    private readonly IUserRepository _userRepository;

    // 3. สร้าง Constructor เพื่อทำ Dependency Injection
    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet("GetUserById/{id}")]
    public ActionResult GetUserById(string id)
    {
        // 4. นำโค้ดใหม่มาแทนที่ Todo เดิม
        var result = _userRepository.GetUserById(id);

        if (result == null)
        {
            return NotFound(new { message = "User not found" }); 
        }

        return Ok(result);
    }

    [HttpPost("CreateUser")]
    public ActionResult CreateUser(UserModel user)
    {
        // เรียกใช้งาน _userRepository ที่ได้ทำการ Inject ไว้ เพื่อบันทึกข้อมูล
        int result = _userRepository.CreateUser(user);

        // ตรวจสอบผลลัพธ์ว่าบันทึกสำเร็จหรือไม่ (ผลลัพธ์มากกว่า 0 คือสำเร็จ)
        if (result > 0)
        {
            return Ok(new { message = "User created successfully" });
        }

        // ถ้าบันทึกไม่สำเร็จ
        return BadRequest(new { message = "Failed to create user" });
    }

}
