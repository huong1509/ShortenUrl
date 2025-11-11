using Authentication_Service.Data;
using Authentication_Service.DTOs;
using Authentication_Service.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Authentication_Service.Services;
using BC = BCrypt.Net.BCrypt;


namespace Authentication_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthenticationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(AuthenticationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.Users
                .Select(u => new
                {
                    u.Id,
                    u.UserName,
                    u.Email
                })
                .ToListAsync();

            return Ok(users);
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto registerDto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
            {
                return BadRequest("Email already exists");
            }
            string passwordHash = BC.HashPassword(registerDto.Password);
            var user = new User
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                PasswordHash = passwordHash,
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return StatusCode(201, "Account registered successfully!");
        }
        [HttpPost("login")]
        [AllowAnonymous]

        public async Task<ActionResult<TokenDto>> Login(UserLoginDto loginDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
            if (user == null || !BC.Verify(loginDto.Password, user.PasswordHash))
            {
                return Unauthorized("Invalid email or password!");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new TokenDto { Token = tokenString, Expiration = token.ValidTo });
        }

        [HttpPut("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateUser(int id, UserUpdateDto updateDto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound("User not found.");

            if (!string.IsNullOrWhiteSpace(updateDto.Email))
            {
                if (await _context.Users.AnyAsync(u => u.Email == updateDto.Email && u.Id != id))
                    return BadRequest("Email already in use by another account.");

                user.Email = updateDto.Email;
            }


            if (!string.IsNullOrWhiteSpace(updateDto.UserName))
            {
                user.UserName = updateDto.UserName;
            }
            else
            {
                user.UserName = user.UserName;
            }

            if (!string.IsNullOrWhiteSpace(updateDto.Email))
            {
                user.Email = updateDto.Email;
            }
            else
            {
                user.Email = user.Email;
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Account updated successfully.",
                user.Id,
                user.UserName,
                user.Email
            });
        }


        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound("User not found.");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok("Account deleted successfully.");
        }



        [HttpPost("send-email")]
        [AllowAnonymous]
        public async Task<IActionResult> RequestForgotPassword( SendEmailDto sendEmailDto,EmailService emailService)
        {
            var email = await _context.Users.FirstOrDefaultAsync(u => u.Email == sendEmailDto.Email);
            if (email == null)
                return BadRequest("Email not found.");

            var code = new Random().Next(100000, 999999).ToString();
            email.ResetCode = code;
            email.ResetCodeExpiry = DateTime.UtcNow.AddMinutes(10);
            email.ResetCodeVerified = false;

            await _context.SaveChangesAsync();

            await emailService.SendEmailAsync(
                sendEmailDto.Email,
                "Password Reset Verification Code",
                $"Your verification code is: {code}");

            return Ok("A verification code has been sent to your email.");
        }


        [HttpPost("verify-code")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyResetCode(VerifyCodeDto codeVerifyDto)
        {
            var email = await _context.Users.FirstOrDefaultAsync(u => u.Email == codeVerifyDto.Email);
            if (email == null)
                return BadRequest("Email not found.");

            if (email.ResetCode != codeVerifyDto.Code || email.ResetCodeExpiry < DateTime.UtcNow)
                return BadRequest("Invalid or expired code.");

            email.ResetCodeVerified = true;
            await _context.SaveChangesAsync();

            return Ok("Verification code is valid. You can now reset your password.");
        }


        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var email = await _context.Users.FirstOrDefaultAsync(u => u.Email == resetPasswordDto.Email);
            if (email == null)
                return BadRequest("Email not found.");

            if (!email.ResetCodeVerified)
                return BadRequest("Verification code not confirmed.");

            if(resetPasswordDto.NewPassword != resetPasswordDto.ConfirmPassword)
            {
                return BadRequest("Password do not match!");

            }
            else
            {
                email.PasswordHash = BC.HashPassword(resetPasswordDto.NewPassword);
                email.ResetCode = null;
                email.ResetCodeVerified = false;
                email.ResetCodeExpiry = null;

                await _context.SaveChangesAsync();

                return Ok("Password has been reset successfully.");
            }
        }
    }
}

