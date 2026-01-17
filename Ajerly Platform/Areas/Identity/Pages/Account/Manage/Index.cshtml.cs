using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Ajerly_Platform.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Ajerly_Platform.Areas.Identity.Pages.Account
{
    public class ManageIndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ManageIndexModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            Input = new InputModel();
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string? Email { get; set; }
        public string? StatusMessage { get; set; }

        public class InputModel
        {
            [Display(Name = "Full name")]
            public string? FullName { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("Unable to load user.");
            }

            Email = user.Email;
            Input.FullName = user.FullName;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("Unable to load user.");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            user.FullName = Input.FullName;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var err in result.Errors)
                    ModelState.AddModelError(string.Empty, err.Description);
                return Page();
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "تم حفظ التغييرات.";
            Email = user.Email;
            return Page();
        }
    }
}
