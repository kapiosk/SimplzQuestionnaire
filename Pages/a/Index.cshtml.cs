using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SimplzQuestionnaire.Model;

namespace SimplzQuestionnaire.Pages.a
{
    public class IndexModel : PageModel
    {
        private readonly SimplzQuestionnaire.Model.SQContext _context;

        public IndexModel(SimplzQuestionnaire.Model.SQContext context)
        {
            _context = context;
        }

        public IList<Answer> Answer { get;set; }

        public async Task OnGetAsync()
        {
            Answer = await _context.Answers
                .Include(a => a.Question).ToListAsync();
        }
    }
}
