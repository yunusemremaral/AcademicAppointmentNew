using AcademicAppointmentApi.BusinessLayer.Abstract;
using AcademicAppointmentApi.EntityLayer.Entities;
using AcademicAppointmentApi.Presentation.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailService _emailService; // Email servisi ekleniyor
        private readonly IConfiguration _config;


        public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailService emailService, IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService; // Email servisinin yapılandırılması
            _config = config;

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(string username, string email, string password)
        {
            var user = new AppUser { UserName = username, Email = email };

            // Kullanıcı oluşturuluyor
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // Kullanıcı oluşturulmuşsa, e-posta onay token'ı oluşturuluyor
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            // Kullanıcıya onay maili gönderiliyor
            var confirmationLink = Url.Action("ConfirmEmail", "Auth", new { token, email = user.Email }, Request.Scheme);
            await _emailService.SendConfirmationEmailAsync(user.Email, confirmationLink);

            return Ok("Kayıt başarılı! E-posta adresinizi doğrulamak için e-posta gönderildi.");
        }

        [HttpGet("confirmemail")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
                return BadRequest("Geçersiz token veya e-posta.");

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return NotFound("Kullanıcı bulunamadı.");

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
                return BadRequest("E-posta doğrulaması başarısız.");

            return Ok("E-posta doğrulandı.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            // 1) Kullanıcı var mı, parola doğru mu?
            var user = await _userManager.FindByNameAsync(dto.Username);
            if (user == null)
                return Unauthorized("Kullanıcı bulunamadı");

            var pwCheck = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
            if (!pwCheck.Succeeded)
                return Unauthorized("Geçersiz kimlik bilgileri");

            // 2) Claim’leri hazırla (id, email, username + roller)
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id),
        new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
        new Claim(JwtRegisteredClaimNames.Email, user.Email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };
            // rollerini de ekle
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            // 3) Secret key + signing credentials
            var jwtSettings = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // 4) Token’ı oluştur
            var expires = DateTime.UtcNow.AddMinutes(
                              double.Parse(jwtSettings["ExpiresInMinutes"]));
            var tokenDescriptor = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler()
                                  .WriteToken(tokenDescriptor);

            // 5) İstemciye dön
            return Ok(new
            {
                token = tokenString,
                expiresAt = expires
            });
        }


        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok("Çıkış yapıldı");
        }
    }
}
