using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Interview_Test.Middlewares;

public class AuthenMiddleware : IMiddleware
{
    // นำค่า x-api-key ที่ถูก Hash ด้วย SHA512 มาใส่ตรงนี้ (แนะนำให้เป็นรูปแบบ Hex String)
    private const string hashedKey = "<your hash sha512 x-api-key>";

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var apiKeyHeader = context.Request.Headers["x-api-key"].ToString();
        
        if (string.IsNullOrEmpty(apiKeyHeader))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized; // 401
            await context.Response.WriteAsync("API Key is missing");
            return;
        }

        // นำค่า x-api-key จาก Header มาเข้ารหัสด้วย SHA512
        using var sha512 = SHA512.Create();
        byte[] hashedBytes = sha512.ComputeHash(Encoding.UTF8.GetBytes(apiKeyHeader));
        
        // แปลง byte array เป็น Hexadecimal string เพื่อใช้เปรียบเทียบ
        string computedHash = Convert.ToHexString(hashedBytes);

        // เปรียบเทียบค่า Hash ที่คำนวณได้กับค่าที่ตั้งไว้ (ไม่สนตัวพิมพ์เล็ก-ใหญ่)
        if (!string.Equals(computedHash, hashedKey, StringComparison.OrdinalIgnoreCase))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized; // 401
            await context.Response.WriteAsync("Invalid API Key");
            return;
        }

        // หาก x-api-key ถูกต้อง ให้ส่ง request ทำงานใน Middleware ถัดไป
        await next(context);
    }
}