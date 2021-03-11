using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SimplzQuestionnaire.Model;

namespace SimplzQuestionnaire.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionnaireController : ControllerBase
    {
        private readonly SQContext _context;
        public QuestionnaireController(SQContext context)
        {
            _context = context;
        }

        [HttpGet("ActiveQuestion/{QuestionnaireId}")]
        public async Task<int> GetActiveQuestion([FromRoute] int QuestionnaireId)
        {
            int activeQuestionId = 0;
            var questionnaire = await _context.Questionnaires.FindAsync(QuestionnaireId);
            if (questionnaire is not null && questionnaire.ActiveQuestionId.HasValue) activeQuestionId = questionnaire.ActiveQuestionId.Value;
            return activeQuestionId;
        }
    }
}
