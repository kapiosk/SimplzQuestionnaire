﻿using Microsoft.AspNetCore.Mvc.RazorPages;
using SimplzQuestionnaire.Interfaces;

namespace SimplzQuestionnaire.Pages
{
    public class IndexModel : PageModel
    {
        private ICurrentUserService _currentUser;
        public IndexModel(ICurrentUserService currentUser)
        {
            _currentUser = currentUser;
        }

        public void OnGet()
        { }
    }
}
