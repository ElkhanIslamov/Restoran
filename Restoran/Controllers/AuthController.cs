using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Restoran.Areas.Admin.Controllers;
using Restoran.Areas.Admin.Data;
using Restoran.DataContext.Entities;
using Restoran.Helpers;

namespace Restoran.Controllers
{
    public class AuthController: AdminController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Login()
        {
            //if (User.Identity.IsAuthenticated)
            //    RedirectToAction("Index", "Home");

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (User.Identity.IsAuthenticated)
                RedirectToAction("Index", "Home");

            if (!ModelState.IsValid)
                return View();

            var user = await _userManager.FindByNameAsync(loginViewModel.UserNameOrEmail);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(loginViewModel.UserNameOrEmail);
                if (user == null)
                {
                    ModelState.AddModelError("", "username/email or password is incorrect");
                    return View();

                }
            }
            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError("", "Please confirm email");
                return View();
            }
            var signInManager = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, loginViewModel.RememberMe, true);
            if (signInManager.IsLockedOut)
            {
                ModelState.AddModelError("", "Blok edildi...");
                return View();
            }
            if (!signInManager.Succeeded)
            {
                ModelState.AddModelError("", "username/email or password is incorrect");
                return View();

            }

            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Logout()
        {
            if (!User.Identity.IsAuthenticated)
                return BadRequest();

            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgotPasswordViewModel)
        {
            if (!ModelState.IsValid)
                return View();

            var user = await _userManager.FindByEmailAsync(forgotPasswordViewModel.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Email not found");
                return View();
            }

            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            string link = Url.Action("ResetPassword", "Auth", new { email = user.Email, token = token },
             HttpContext.Request.Scheme, HttpContext.Request.Host.Value);
            //return Content(link);
            // string body = $"<a href='{link}'> Change Password</a>";
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "templates", "index.html");
            using StreamReader streamReader = new StreamReader(path);
            string content = await streamReader.ReadToEndAsync();
            string body = content.Replace("[link]", link);


            EmailHelper emailHelper = new EmailHelper(_configuration);
            await emailHelper.SendEmailAsync(new MailRequest
            {
                ToEmail = user.Email,
                Subject = "ResetPassword",
                Body = body
            });

            return RedirectToAction(nameof(Login));
        }


        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordViewModel.Email);
            if (user == null)
                return NotFound();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(SubmitResetPasswordViewModel submitResetPasswordViewModel,
            string email, string token)
        {
            if (!ModelState.IsValid)
                return View();
            var user = await _userManager.FindByEmailAsync(email);

            IdentityResult identityResult = await _userManager.ResetPasswordAsync(user, token, submitResetPasswordViewModel.Password);
            if (!identityResult.Succeeded)
            {
                foreach (var error in identityResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View();
            }
            return RedirectToAction(nameof(Login));
        }

        public async Task<IActionResult> ConfirmEmail(ConfirmEmailViewModel confirmEmailViewModel)
        {
            var user = await _userManager.FindByEmailAsync(confirmEmailViewModel.Email);
            if (user == null)
                return NotFound();

            if (await _userManager.IsEmailConfirmedAsync(user))
                return BadRequest();

            IdentityResult identityResult = await _userManager.ConfirmEmailAsync(user, confirmEmailViewModel.Token);
            if (identityResult.Succeeded)
            {
                TempData["ConfirmationMessage"] = "Your email successfully confirmed";
                return RedirectToAction(nameof(Login));
            }

            return BadRequest();
        }
        //public async Task<IActionResult> CreateRole()
        //{
        //    foreach (var userRole in Enum.GetNames(typeof(Roles)))
        //    {
        //        await _roleManager.CreateAsync(new IdentityRole { Name = userRole });
        //    }
        //    return Content("Role yarandi");
        //}
    }
}
