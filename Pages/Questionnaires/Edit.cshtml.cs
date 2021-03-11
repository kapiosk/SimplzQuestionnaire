using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SimplzQuestionnaire.Interfaces;
using SimplzQuestionnaire.Model;

namespace SimplzQuestionnaire.Pages.Questionnaires
{
    public class EditModel : PageModel
    {
        private readonly ICurrentUserService _currentUser;
        private readonly SQContext _context;
        public EditModel(ICurrentUserService currentUser, SQContext context)
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

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Questionnaire).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionnaireExists(Questionnaire.QuestionnaireId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool QuestionnaireExists(int id)
        {
            return _context.Questionnaires.Any(e => e.QuestionnaireId == id);
        }
    }
}
