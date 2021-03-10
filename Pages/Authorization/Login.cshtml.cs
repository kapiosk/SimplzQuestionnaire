using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using SimplzQuestionnaire.Model;

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

        private readonly SQContext _context;

        public LoginModel(SQContext context)
        {
            _context = context;
        }

        public void OnGet() { }

        private async Task<bool> LogIn(string username, string password)
        {
            var hashedPassword = QuestionnaireUser.ComputeSha256Hash(password);
            var user = _context.QuestionnaireUsers.FirstOrDefault(x => x.Username.Equals(username) && x.Password.Equals(hashedPassword));
            if (user is not null)
            {
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.NameIdentifier, $"{user.QuestionnaireUserId}" )
                    };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                return true;
            }
            return false;
        }

        public async Task<IActionResult> OnPostLoginAsync(string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                if (await LogIn(Input.Username, Input.Password))
                {
                    if (!string.IsNullOrEmpty(returnUrl))
                        return LocalRedirect(returnUrl);

                    return LocalRedirect("/");
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostCreateAsync(string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var hashedPassword = QuestionnaireUser.ComputeSha256Hash(Input.Password);
                    await _context.QuestionnaireUsers.AddAsync(new QuestionnaireUser { Username = Input.Username, Password = hashedPassword });
                    _context.SaveChanges();

                    await LogIn(Input.Username, Input.Password);

                    if (!string.IsNullOrEmpty(returnUrl))
                        return LocalRedirect(returnUrl);

                    return LocalRedirect("/");
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
