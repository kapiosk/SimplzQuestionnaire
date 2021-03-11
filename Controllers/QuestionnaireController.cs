using System.Linq;
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
        public int GetActiveQuestion([FromRoute] int QuestionnaireId)
        {
            return (from it in _context.Questionnaires
                    where it.QuestionnaireId == QuestionnaireId
                    select it.ActiveQuestionId).FirstOrDefault();
        }
    }
}
