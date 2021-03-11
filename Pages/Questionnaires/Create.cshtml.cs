using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SimplzQuestionnaire.Interfaces;
using SimplzQuestionnaire.Model;

namespace SimplzQuestionnaire.Pages.Questionnaires
{
    public class CreateModel : PageModel
    {
        private readonly ICurrentUserService _currentUser;
        private readonly SQContext _context;
        public CreateModel(ICurrentUserService currentUser, SQContext context)
        {
            _currentUser = currentUser;
            _context = context;
        }

        public IActionResult OnGet()
        {
            //ViewData["UserId"] = new SelectList(_context.QuestionnaireUsers, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public Questionnaire Questionnaire { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Questionnaire.Code = Guid.NewGuid();
            Questionnaire.UserId = _currentUser.UserId;
            _context.Questionnaires.Add(Questionnaire);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
