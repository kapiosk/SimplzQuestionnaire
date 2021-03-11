using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SimplzQuestionnaire.Interfaces;
using SimplzQuestionnaire.Model;

namespace SimplzQuestionnaire.Pages.Questions
{
    public class IndexModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int QuestionnaireId { get; set; }

        private readonly ICurrentUserService _currentUser;
        private readonly SQContext _context;
        public IndexModel(ICurrentUserService currentUser, SQContext context)
        {
            _currentUser = currentUser;
            _context = context;
        }

        public IList<Question> Question { get; set; }
        public async Task OnGetAsync()
        {
            Question = await _context.Questions
                .Where(q => q.QuestionnaireId == QuestionnaireId && q.Questionnaire.UserId == _currentUser.UserId)
                .ToListAsync();
        }
    }
}
