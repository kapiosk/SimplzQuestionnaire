using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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

        [BindProperty]
        public Question Question { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            var prevQuestion = _context.Questions
                                       .OrderByDescending(q => q.QuestionId)
                                       .FirstOrDefault(q => q.QuestionnaireId == QuestionnaireId);
            Question = new();
            if (prevQuestion is not null)
            {
                Question.AcceptsCustomAnswer = prevQuestion.AcceptsCustomAnswer;
                Question.MaxAnswers = prevQuestion.MaxAnswers;
                Question.MaxPoints = prevQuestion.MaxPoints;
                Question.Timeout = prevQuestion.Timeout;
                Question.Rank = prevQuestion.Rank + 1;
            }

            Question.QuestionnaireId = QuestionnaireId;
            return await Task.FromResult(Page());
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            //Question.Rank = await _context.Questionnaires
            //                              .Include("Questions")
            //                              .Select(q => q.Questions.Count)
            //                              .FirstOrDefaultAsync(q => QuestionnaireId == QuestionnaireId) + 1;
            //Question.QuestionnaireId = QuestionnaireId;
            _context.Questions.Add(Question);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index", new { QuestionnaireId });
        }
    }
}
