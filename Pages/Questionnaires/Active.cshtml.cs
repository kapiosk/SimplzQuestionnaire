using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SimplzQuestionnaire.Pages.Questionnaires
{
    public class ActiveModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string QuestionnaireCode { get; set; }
        public void OnGet()
        {
        }
    }
}
