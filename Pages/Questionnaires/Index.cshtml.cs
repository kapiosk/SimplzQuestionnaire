using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SimplzQuestionnaire.Interfaces;
using SimplzQuestionnaire.Model;

namespace SimplzQuestionnaire.Pages.Questionnaires
{
    public class IndexModel : PageModel
    {
        private readonly ICurrentUserService _currentUser;
        private readonly SQContext _context;
        public IndexModel(ICurrentUserService currentUser, SQContext context)
        {
            _currentUser = currentUser;
            _context = context;
        }
        public IList<Model.Questionnaire> Questionnaires { get; set; }
        public async Task OnGetAsync()
        {
            Questionnaires = await _context.Questionnaires.Where(x => x.UserId == _currentUser.UserId).ToListAsync();
        }
    }
}
