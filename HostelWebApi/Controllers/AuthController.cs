

using Microsoft.AspNetCore.Mvc;
using HostelWebApi.Models;
using HostelWebApi.Data;

namespace HostelWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        //this is for database access
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterRequest req)//called registered model class
        {
            // 1. Check password match
            if (req.Password != req.ConfirmPassword)
            {
                return BadRequest("Passwords do not match.");
            }
            // 2. Check if email already exists
            var existingUser = _context.Users.FirstOrDefault(u => u.Email == req.Email);
            if(existingUser != null)
            {
                return BadRequest("Email already in use.");
            }
            // 3. Hash password (simple for now)
            string hashedPassword = req.Password; // we will hash later

            var user = new User
            {
                FullName = req.FullName,
                Email = req.Email,
                PasswordHash = hashedPassword
            };
            // 4. Save user to database
            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok("User registered successfully!");
        }


        [HttpPost("login")]
        public IActionResult Login(LoginRequest req) //ikkada login request model class ni call cheathunnam
        { 
            var user = _context.Users.FirstOrDefault(u => u.Email == req.Email);
            if(user == null)
            {
                return NotFound("User not found.Please register.");
            }

            // Check password (simple for now)
            if(user.PasswordHash != req.Password)
            {
                return BadRequest("Invalid password.");
            }
            return Ok("Login successful!");
        }

    }
}
