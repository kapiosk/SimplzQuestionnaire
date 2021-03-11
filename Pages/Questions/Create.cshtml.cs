using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SimplzQuestionnaire.Interfaces;
using SimplzQuestionnaire.Model;

namespace SimplzQuestionnaire.Pages.Questions
{
    public class CreateModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int QuestionnaireId { get; set; }
        private readonly ICurrentUserService _currentUser;
        private readonly SQContext _context;
        public CreateModel(ICurrentUserService currentUser, SQContext context)
        {
            _currentUser = currentUser;
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Question Question { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Question.QuestionnaireId = QuestionnaireId;
            _context.Questions.Add(Question);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index", new { QuestionnaireId = QuestionnaireId });
        }
    }
}
