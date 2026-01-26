

using HostelWebApi.Data;
using HostelWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
//For JWT
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace HostelWebApi.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/[controller]")]

  //  public class AuthController(AppDbContext context, IConfiguration config) : ControllerBase
    public class AuthController : ControllerBase
    {
        //this is for database access
        private readonly AppDbContext _context;
        private readonly IConfiguration _config; // For accessing JWT settings
        private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

        public AuthController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterRequest req)
        {
            // 1. Check password match
            if (req.Password != req.ConfirmPassword)
                return BadRequest("Passwords do not match.");

            // 2. Check if email already exists
            var existingUser = _context.Users.FirstOrDefault(u => u.Email == req.Email);
            if (existingUser != null)
                return BadRequest("Email already in use.");

            // 3. Create user
            var user = new User
            {
                FullName = req.FullName,
                Email = req.Email
            };

            // 4. Hash password properly
            user.PasswordHash = _passwordHasher.HashPassword(user, req.Password);

            // 5. Save user to database
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

            // Verify hashed password
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, req.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                return BadRequest("Invalid password.");
            }

           // return Ok("Login successful!");
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                     new Claim(ClaimTypes.Name, user.Email),
                     new Claim("FullName", user.FullName)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };
            //THIS IS PAY LOAD(2)
            //Subject = new ClaimsIdentity(new[]
            //   {
            //         new Claim(ClaimTypes.Name, user.Email),
            //         new Claim("FullName", user.FullName)
            //    }),
            //    Expires = DateTime.UtcNow.AddHours(1),
            //    Issuer = _config["Jwt:Issuer"],
            //    Audience = _config["Jwt:Audience"],

            //THIS IS HEADER (1)
            //SigningCredentials = new SigningCredentials(
            //       new SymmetricSecurityKey(key),             //SIGNATURE KEY(3)
            //       SecurityAlgorithms.HmacSha256Signature
            //   )


            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            //  Console.WriteLine("KEY: " + _config["Jwt:Key"]);
            return Ok(new
            {
                Success = true,
                Message = "Login successful!",
                Token = tokenString,
                User = new
                {
                    user.FullName,
                    user.Email
                }
            });


        }

    }
}
