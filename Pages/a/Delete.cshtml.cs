using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SimplzQuestionnaire.Model;

namespace SimplzQuestionnaire.Pages.a
{
    public class DeleteModel : PageModel
    {
        private readonly SimplzQuestionnaire.Model.SQContext _context;

        public DeleteModel(SimplzQuestionnaire.Model.SQContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Answer Answer { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Answer = await _context.Answers
                .Include(a => a.Question).FirstOrDefaultAsync(m => m.AnswerId == id);

            if (Answer == null)
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

            Answer = await _context.Answers.FindAsync(id);

            if (Answer != null)
            {
                _context.Answers.Remove(Answer);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
