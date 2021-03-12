using System;
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
    public class DeleteModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int QuestionnaireId { get; set; }
        [BindProperty(SupportsGet = true)]
        public int QuestionId { get; set; }
        private readonly ICurrentUserService _currentUser;
        private readonly SQContext _context;
        public DeleteModel(ICurrentUserService currentUser, SQContext context)
        {
            _currentUser = currentUser;
            _context = context;
        }

        [BindProperty]
        public Question Question { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Question = await _context.Questions.FirstOrDefaultAsync(m => m.QuestionId == QuestionId);

            if (Question == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Question = await _context.Questions.FindAsync(QuestionId);

            if (Question != null)
            {
                _context.Questions.Remove(Question);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index", new { QuestionnaireId });
        }
    }
}
