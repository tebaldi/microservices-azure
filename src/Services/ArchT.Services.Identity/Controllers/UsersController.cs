using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using ArchT.Services.Identity.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArchT.Services.Identity.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public UsersController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        [HttpGet, Route(""), ResponseCache(NoStore =true, Location = ResponseCacheLocation.None)]
        public async Task<IEnumerable<IdentityUser>> GetUsers() => await _userManager.Users.ToArrayAsync();

        [HttpPost, Route("register"), AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var user = new IdentityUser(request.UserName)
            {
                Email = request.Email,
                EmailConfirmed = false
            };
            var createResult = await _userManager.CreateAsync(user, request.Password);
            if (!createResult.Succeeded)
                return new BadRequestObjectResult(createResult.Errors);

            var userRole = await AddRole(user, "user");
            if (!userRole.Succeeded) return new BadRequestObjectResult(userRole.Errors);

            if (request.IsAdmin)
            {
                var adminRole = await AddRole(user, "admin");
                if (!adminRole.Succeeded) return new BadRequestObjectResult(adminRole.Errors);
            }

            var confirmToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            //var callbackUrl = Url.Page(
            //    $"/Users/{nameof(ConfirmEmail)}",
            //    pageHandler: null,
            //    values: new ConfirmEmailRequest { Email = request.Email, Token = confirmToken },
            //    protocol: Request.Scheme);

            //var emailMessage =
            //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.";

            //await SendMail("Registration Confirmation", emailMessage);

            return new OkObjectResult(confirmToken);
        }

        [HttpPost, Route("confirmemail"), AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            var result = await _userManager.ConfirmEmailAsync(user, request.Token);
            if (!result.Succeeded) return new BadRequestObjectResult(result.Errors);
            return new OkResult();
        }

        [HttpPost, Route("signin"), AllowAnonymous]
        public async Task<IActionResult> SignIn([FromBody] SignInRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            var canSignIn = await _signInManager.CanSignInAsync(user);
            if (!canSignIn)
                return new BadRequestObjectResult("Cant Sign In!");

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!result.Succeeded)
                return new BadRequestObjectResult("Wrong Password!");

            var identity = await _signInManager.CreateUserPrincipalAsync(user);
            var secret = Convert.ToBase64String(Encoding.UTF8.GetBytes("client-top-secret"));
            var token = (identity.Identity as ClaimsIdentity).GenerateSecurityToken(secret);

            return new OkObjectResult(token);
        }

        private async Task<IdentityResult> AddRole(IdentityUser user, string role)
        {
            var result = default(IdentityResult);
            var userRole = await _roleManager.FindByNameAsync(role);

            if (userRole == null)
                result = await _roleManager.CreateAsync(new IdentityRole(role));

            result = await _userManager.AddToRoleAsync(user, role);

            return result;
        }

        private async Task SendMail(string subject, string body)
        {
            using (var client = new SmtpClient("stmpserver", 587))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("username", "password");
                await client.SendMailAsync(from: "from", recipients: "to", subject: subject, body: body);
            }
        }

        public class RegisterRequest
        {
            [Required]
            public string Email { get; set; }
            [Required]
            public string Password { get; set; }
            [Required]
            public string UserName { get; set; }
            public bool IsAdmin { get; set; }
        }

        public class ConfirmEmailRequest
        {
            [Required]
            public string Email { get; set; }
            [Required]
            public string Token { get; set; }
        }

        public class SignInRequest
        {
            [Required]
            public string Email { get; set; }
            [Required]
            public string Password { get; set; }
        }
    }
}
