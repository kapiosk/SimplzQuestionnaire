using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimplzQuestionnaire.Interfaces;
using SimplzQuestionnaire.Model;

namespace SimplzQuestionnaire.Pages.Questionnaires
{
    public class ActiveModel : PageModel
    {
        [BindProperty]
        public Answer Answer { get; set; }

        [BindProperty]
        public Question Question { get; set; }

        [BindProperty(SupportsGet = true)]
        public string QuestionnaireCode { get; set; }

        [BindProperty(SupportsGet = true)]
        public int QuestionId { get; set; }

        public int PreviousQuestionId { get; set; }
        public int NextQuestionId { get; set; }

        private readonly ICurrentUserService _currentUser;
        private readonly SQContext _context;
        public ActiveModel(ICurrentUserService currentUser, SQContext context)
        {
            _currentUser = currentUser;
            _context = context;
        }

        public bool IsAdmin;
        public async Task<IActionResult> OnGetAsync()
        {
            Bind();
            return await Task.FromResult(Page());
        }

        public async Task<IActionResult> OnPostAsync()
        {
            return await Task.FromResult(Page());
        }

        private void Bind()
        {
            var questionnaire = _context.Questionnaires.FirstOrDefault(q => q.Code.Equals(System.Guid.Parse(QuestionnaireCode)));

            IsAdmin = questionnaire.UserId == _currentUser.UserId;

            if (questionnaire.ActiveQuestionId > 0)
                QuestionId = questionnaire.ActiveQuestionId;

            Question = _context.Questions.FirstOrDefault(q => q.QuestionId == QuestionId);

            if (Question is null)
                Question = _context.Questions.FirstOrDefault(q => q.QuestionnaireId == questionnaire.QuestionnaireId);

            Answer = (from ua in _context.UserAnswers
                      where ua.UserId == _currentUser.UserId
                      join a in _context.Answers on ua.AnswerId equals a.AnswerId
                      where a.QuestionId == Question.QuestionId
                      select a).FirstOrDefault();


        }
    }
}
