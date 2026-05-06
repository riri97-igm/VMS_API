//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Configuration;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;
//using VMS.DataAccess.Interface;
//using VMS.Model.DTOs;

//[ApiController]
//[Route("api/[controller]")]
//public class LoginController : ControllerBase
//{
//    private readonly ILoginRepository _loginRepository;
//    private readonly IConfiguration _configuration;

//    public LoginController(ILoginRepository loginRepository, IConfiguration configuration)
//    {
//        _loginRepository = loginRepository;
//        _configuration = configuration;
//    }

//    [HttpPost]
//    public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
//    {
//        var user = await _loginRepository.GetByUserNameAndPasswordAsync(loginDto.UserName, loginDto.Password);
//        if (user == null)
//            return Unauthorized("Invalid credentials");

//        // Commented out JWT token generation
//        /*
//        var tokenHandler = new JwtSecurityTokenHandler();
//        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
//        var tokenDescriptor = new SecurityTokenDescriptor
//        {
//            Subject = new ClaimsIdentity(new[]
//            {
//                new Claim(ClaimTypes.Name, user.UserName),
//                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
//            }),
//            Expires = DateTime.UtcNow.AddHours(1),
//            Issuer = _configuration["Jwt:Issuer"],
//            Audience = _configuration["Jwt:Audience"],
//            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
//        };
//        var token = tokenHandler.CreateToken(tokenDescriptor);
//        return Ok(new { token = tokenHandler.WriteToken(token) });
//        */
//        // Instead, just return success or user info
//        return Ok(new { message = "Login successful (JWT disabled)", user = user.UserName });
//    }
//}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VMS.API.Services;
using VMS.DataAccess.Interface;
using VMS.Model.DTOs.Auth;

namespace VMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly IJwtService _jwtService;

        public AuthController(IAuthRepository authRepository, IJwtService jwtService)
        {
            _authRepository = authRepository;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponseDTO>> Login(LoginRequestDTO dto)
        {
            var (success, roleName, staff) = await _authRepository.LoginAsync(dto);
            if (!success || staff == null)
                return Unauthorized(new { message = "Invalid email or password" });

            var token = _jwtService.GenerateToken(staff, roleName);
            return Ok(new LoginResponseDTO
            {
                Token = token,
                StaffId = staff.Id,
                Name = staff.Name,
                Email = staff.Email,
                RoleName = roleName,
            });
        }

        [HttpPost("register")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Register(RegisterRequestDTO dto)
        {
            var (success, message) = await _authRepository.RegisterAsync(dto);
            if (!success) return BadRequest(new { message });
            return Ok(new { message });
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO dto)
        {
            var staffIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(staffIdClaim, out var staffId)) return Unauthorized();

            var success = await _authRepository.ChangePasswordAsync(staffId, dto.CurrentPassword, dto.NewPassword);
            if (!success) return BadRequest(new { message = "Current password is incorrect" });
            return Ok(new { message = "Password changed successfully" });
        }

        [HttpGet("me")]
        [Authorize]
        public IActionResult Me()
        {
            return Ok(new
            {
                StaffId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value,
                Name = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value,
                Email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value,
                Role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value,
            });
        }
    }

    public record ChangePasswordDTO(string CurrentPassword, string NewPassword);
}
