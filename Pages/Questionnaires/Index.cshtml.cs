using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SimplzQuestionnaire.Interfaces;
using SimplzQuestionnaire.Model;

namespace SimplzQuestionnaire.Pages.Questionnaires
{
    public class IndexModel : PageModel
    {
        private readonly ICurrentUserService _currentUser;
        private readonly SQContext _context;

        public readonly string QRURL;
        public readonly string QURL;
        public IndexModel(ICurrentUserService currentUser, IConfiguration config, SQContext context)
        {
            QRURL = config["QRURL"];
            QURL = config["QURL"];
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
