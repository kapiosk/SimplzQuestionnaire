using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SimplzQuestionnaire.Interfaces;
using SimplzQuestionnaire.Model;

namespace SimplzQuestionnaire.Pages.Questionnaires
{
    public class DeleteModel : PageModel
    {
        private readonly ICurrentUserService _currentUser;
        private readonly SQContext _context;
        public DeleteModel(ICurrentUserService currentUser, SQContext context)
        {
            _currentUser = currentUser;
            _context = context;
        }

        [BindProperty]
        public Questionnaire Questionnaire { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Questionnaire = await _context.Questionnaires
                .FirstOrDefaultAsync(q => q.UserId == _currentUser.UserId && q.QuestionnaireId == id);

            if (Questionnaire == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Questionnaire = await _context.Questionnaires.FindAsync(id);

            if (Questionnaire != null)
            {
                _context.Questionnaires.Remove(Questionnaire);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
