using AcademicAppointmentApi.BusinessLayer.Abstract;
using AcademicAppointmentApi.EntityLayer.Entities;
using AcademicAppointmentApi.Presentation.Dtos;
using AcademicAppointmentApi.Presentation.Dtos.PasswordDtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AcademicAppointmentApi.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _config;
        private readonly ITokenService _tokenService;

        public AuthController(
            UserManager<AppUser> userMgr,
            SignInManager<AppUser> signInMgr,
            IEmailService emailSrv,
            IConfiguration config,
            ITokenService tokenService)
        {
            _userManager = userMgr;
            _signInManager = signInMgr;
            _emailService = emailSrv;
            _config = config;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var emailUsernamePart = dto.Email.Split('@')[0];

            var user = new AppUser
            {
                UserName = emailUsernamePart,
                UserFullName = dto.UserFullName,
                Email = dto.Email,
                SchoolId = dto.SchoolId,
                DepartmentId = dto.DepartmentId
            };

            var res = await _userManager.CreateAsync(user, dto.Password);
            if (!res.Succeeded) return BadRequest(res.Errors);

            await _userManager.AddToRoleAsync(user, "Student");

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var link = Url.Action("ConfirmEmail", "Auth", new { token, email = user.Email }, Request.Scheme);
            await _emailService.SendConfirmationEmailAsync(user.Email, link);

            return Ok("Registration successful, email confirmation link sent.");
        }

        [HttpGet("confirmemail")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
                return BadRequest("Invalid token or email.");

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return NotFound("User not found.");

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
                return BadRequest("Email confirmation failed.");

            return Ok("Email confirmed.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return Unauthorized("Kullanıcı bulunamadı.");

            var pwRes = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
            if (!pwRes.Succeeded)
                return Unauthorized("Şifre hatalı.");

            if (!user.EmailConfirmed)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var link = Url.Action("ConfirmEmail", "Auth", new { token, email = user.Email }, Request.Scheme);
                await _emailService.SendConfirmationEmailAsync(user.Email, link);

                return Unauthorized("Email doğrulanmamış. Yeni bir doğrulama bağlantısı e-posta adresinize gönderildi.");
            }

            var tokenJwt = await _tokenService.CreateAccessTokenAsync(user);
            return Ok(new { token = tokenJwt });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok("Logged out.");
        }

        // --- Şifre Sıfırlama için eklenen kısım ---

        [HttpPost("forgotpassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return BadRequest("User not found.");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var link = Url.Action("ResetPassword", "Auth", new { token, email = user.Email }, Request.Scheme);

            await _emailService.SendPasswordResetEmailAsync(user.Email, link);

            return Ok("Password reset link sent to your email.");
        }

        [HttpPost("resetpassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return BadRequest("User not found.");

            var result = await _userManager.ResetPasswordAsync(user, dto.Token, dto.NewPassword);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("Password has been reset successfully.");
        }
        [HttpPost("resend-confirmation")]
        public async Task<IActionResult> ResendConfirmation([FromBody] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return NotFound("Kullanıcı bulunamadı.");

            if (user.EmailConfirmed)
                return BadRequest("Email zaten doğrulanmış.");

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var link = Url.Action("ConfirmEmail", "Auth", new { token, email = user.Email }, Request.Scheme);
            await _emailService.SendConfirmationEmailAsync(user.Email, link);

            return Ok("Yeni doğrulama bağlantısı gönderildi.");
        }
    }
}
