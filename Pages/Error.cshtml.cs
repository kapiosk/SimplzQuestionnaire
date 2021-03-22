using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SimplzQuestionnaire.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
    public class ErrorModel : PageModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        private readonly ILogger<ErrorModel> _logger;
        private readonly Interfaces.ICurrentUserService _currentUser;
        private readonly Model.SQContext _context;

        public ErrorModel(ILogger<ErrorModel> logger, Interfaces.ICurrentUserService currentUser, Model.SQContext context)
        {
            _logger = logger;
            _currentUser = currentUser;
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

            if (!string.IsNullOrEmpty(_currentUser.UserId))
            {
                try
                {
                    var user = _context.QuestionnaireUsers.FirstOrDefault(u => u.Id == _currentUser.UserId);
                    if (user is null)
                    {
                        await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
                        return LocalRedirect("/");
                    }
                }
                catch { }
            }

            return Page();
        }
    }
}
