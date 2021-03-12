using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public int GetActiveQuestion([FromRoute] int QuestionnaireId)
        {
            return (from it in _context.Questionnaires
                    where it.QuestionnaireId == QuestionnaireId
                    select it.ActiveQuestionId).FirstOrDefault();
        }

        [HttpGet("QuestionAnswerCount/{QuestionId}")]
        public int GetQuestionAnswerCount([FromRoute] int QuestionId)
        {
            return (from it in _context.UserAnswers.Include(ua=>ua.Answer)
                    where it.Answer.QuestionId == QuestionId
                    select it.UserId).Count();
        }
    }
}
