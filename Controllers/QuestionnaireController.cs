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

        [HttpGet("ActiveQuestionAnswers/{ActiveQuestionId}")]
        public object ActiveQuestionAnswers([FromRoute] int ActiveQuestionId)
        {
            return from a in _context.Answers
                   where a.QuestionId == ActiveQuestionId
                   join ua in _context.UserAnswers on a.AnswerId equals ua.AnswerId
                   join u in _context.QuestionnaireUsers on ua.UserId equals u.Id
                   select new { u.UserName, a.Description };

        }

        [HttpGet("QuestionAnswerCount/{QuestionId}")]
        public int GetQuestionAnswerCount([FromRoute] int QuestionId)
        {
            return (from it in _context.UserAnswers.Include(ua => ua.Answer)
                    where it.Answer.QuestionId == QuestionId
                    select it.UserId).Count();
        }
    }
}
