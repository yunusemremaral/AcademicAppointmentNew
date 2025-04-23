using AcademicAppointmentApi.BusinessLayer.Abstract;
using AcademicAppointmentApi.EntityLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
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

        public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService; // Email servisinin yapılandırılması
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
        public async Task<IActionResult> Login(string username, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(username, password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
                return Ok("Giriş başarılı");

            return Unauthorized("Geçersiz kullanıcı adı veya şifre");
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok("Çıkış yapıldı");
        }
    }
}
