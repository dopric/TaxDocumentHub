using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaxDocumentHub.Api.Data;
using TaxDocumentHub.Api.Domain;
using TaxDocumentHub.Api.DTOs;
using TaxDocumentHub.Api.Extensions;
using TaxDocumentHub.Api.Services;

namespace TaxDocumentHub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly AppDbContext _dbContext;
        private readonly ITokenGenerator _tokenGenerator;

        public AuthController(IPasswordHasher passwordHasher, AppDbContext dbContext, ITokenGenerator tokenGenerator)
        {
            _passwordHasher = passwordHasher;
            _dbContext = dbContext;
            _tokenGenerator = tokenGenerator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto request)
        {
            if(await _dbContext.Users.AnyAsync(u => u.Email == request.Email))
            {
                return BadRequest("User with this email already exists.");
            }

            var user = new User
            {
                Email = request.Email,
                PasswordHash = _passwordHasher.HashPassword(request.Password)
            };
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            var token = _tokenGenerator.GenerateToken(user);
            return Ok(new AuthResponseDto(token, user.Email, user.Id));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto request)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if(user == null || !_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            {
                return Unauthorized("Invalid email or password.");
            }
            var token = _tokenGenerator.GenerateToken(user);
            return Ok(new AuthResponseDto(token, user.Email, user.Id));
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult GetCurrentUser()
        {
            var userId = User.GetUserId();

            return Ok(new
            {
                UserId = userId,
                Message = "Du bist authentifiziert und isoliert!"
            });
        }
    }
}
