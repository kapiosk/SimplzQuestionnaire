using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimplzQuestionnaire.Interfaces;
using SimplzQuestionnaire.Model;

namespace SimplzQuestionnaire.Pages.Questionnaire
{
    public class EditModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int QuestionnaireId { get; set; }

        private ICurrentUserService _currentUser;
        private SQContext _context;
        
        public string UserName
        {
            get
            {
                return _currentUser.UserName;
            }
        }
        public Model.Questionnaire Questionnaire { get; set; }

        public EditModel(ICurrentUserService currentUser, SQContext context)
        {
            _currentUser = currentUser;
            _context = context;
        }

        public IActionResult OnGet()
        {
            Questionnaire = _context.Questionnaires.FirstOrDefault(x => x.QuestionnaireId == QuestionnaireId && x.UserId == _currentUser.UserId);
            if(Questionnaire is null) return LocalRedirect("/");
            return Page();
        }
    }
}
