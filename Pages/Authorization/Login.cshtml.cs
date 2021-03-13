using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using SimplzQuestionnaire.Model;
using Microsoft.AspNetCore.Identity;

namespace SimplzQuestionnaire.Pages.Authorization
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Text)]
            public string Username { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        private readonly SignInManager<QuestionnaireUser> _signInManager;

        public LoginModel(SignInManager<QuestionnaireUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnGet() 
        {
            return await Task.FromResult(Page());
        }

        public async Task<IActionResult> OnPostLoginAsync(string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _signInManager.UserManager.FindByNameAsync(Input.Username);
                    var result = await _signInManager.UserManager.CheckPasswordAsync(user, Input.Password);
                    if (result)
                    {
                        var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, user.UserName),
                                new Claim(ClaimTypes.NameIdentifier, user.Id)
                            };

                        await _signInManager.SignInWithClaimsAsync(user, true, claims);

                        if (!string.IsNullOrEmpty(returnUrl))
                            return LocalRedirect(returnUrl);

                        return LocalRedirect("/");
                    }
                }
                catch { }
            }
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return Page();
        }

        public async Task<IActionResult> OnPostCreateAsync(string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = new QuestionnaireUser { UserName = Input.Username };
                    var result = await _signInManager.UserManager.CreateAsync(user, Input.Password);

                    if (result.Succeeded)
                    {

                        var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, user.UserName),
                                new Claim(ClaimTypes.NameIdentifier, user.Id)
                            };

                        await _signInManager.SignInWithClaimsAsync(user, true, claims);

                        if (!string.IsNullOrEmpty(returnUrl))
                            return LocalRedirect(returnUrl);

                        return LocalRedirect("/");
                    }
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return Page();
        }
    }
}