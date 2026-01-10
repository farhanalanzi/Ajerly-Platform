using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Ajerly_Platform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Ajerly_Platform.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<ApplicationUser> signInManager, ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
            Input = new InputModel();
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string? ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string? Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string? Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public void OnGet(string? returnUrl = null)
        {
            ReturnUrl = returnUrl ?? Url.Content("~/");
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            ReturnUrl = returnUrl ?? Url.Content("~/");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in.");
                return LocalRedirect(ReturnUrl);
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning("User account locked out.");
                return RedirectToPage("/Account/Lockout");
            }

            ModelState.AddModelError(string.Empty, "محاولة تسجيل الدخول فشلت. تأكد من البريد الإلكتروني وكلمة المرور.");
            return Page();
        }
    }
}
