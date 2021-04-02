using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SimplzQuestionnaire.Interfaces;
using SimplzQuestionnaire.Model;

namespace SimplzQuestionnaire.Pages.Groups
{
    public class IndexModel : PageModel
    {
        [BindProperty()]
        public IEnumerable<BindableQuestionnaire> Items { get; set; }

        private readonly ICurrentUserService _currentUser;
        private readonly SQContext _context;
        public IndexModel(ICurrentUserService currentUser, SQContext context)
        {
            _currentUser = currentUser;
            _context = context;
        }

        public void OnGet()
        {
            var assigned = from sessionGroup in _context.SessionGroups
                           join questionnaire in _context.Questionnaires on sessionGroup.QuestionnaireId equals questionnaire.QuestionnaireId
                           where questionnaire.UserId == _currentUser.UserId
                           join sessionGroupUser in _context.SessionGroupUsers on sessionGroup.SessionGroupId equals sessionGroupUser.SessionGroupId
                           join user in _context.QuestionnaireUsers on sessionGroupUser.UserId equals user.Id
                           let Key = questionnaire.QuestionnaireId + "_" + user.Id
                           select new
                           {
                               questionnaire.QuestionnaireId,
                               QuestionnaireName = questionnaire.Name,
                               UserId = user.Id,
                               user.UserName,
                               sessionGroup.SessionGroupId,
                               GroupName = sessionGroup.Name,
                               Key
                           };

            var unassigned = from questionnaire in _context.Questionnaires
                             where questionnaire.UserId == _currentUser.UserId
                             join question in _context.Questions on questionnaire.QuestionnaireId equals question.QuestionnaireId
                             join A in _context.Answers on question.QuestionId equals A.QuestionId
                             join UA in _context.UserAnswers on A.AnswerId equals UA.AnswerId
                             join user in _context.QuestionnaireUsers on UA.UserId equals user.Id
                             let Key = questionnaire.QuestionnaireId + "_" + user.Id
                             where !assigned.Select(x => x.Key).Contains(Key)
                             select new
                             {
                                 questionnaire.QuestionnaireId,
                                 QuestionnaireName = questionnaire.Name,
                                 UserId = user.Id,
                                 user.UserName,
                                 SessionGroupId = 0,
                                 GroupName = "Unassigned",
                                 Key
                             };

            Items = assigned.Concat(unassigned)
                            .ToList()
                            .GroupBy(x => new
                            {
                                x.QuestionnaireId,
                                x.QuestionnaireName,
                                x.SessionGroupId,
                                x.GroupName
                            })
                            .Select(x => new
                            {
                                x.Key,
                                Users = x.Select(y => new BindableUser
                                {
                                    UserId = y.UserId,
                                    UserName = y.UserName,
                                    Key = y.Key
                                })
                            })
                            .GroupBy(x => new
                            {
                                x.Key.QuestionnaireId,
                                x.Key.QuestionnaireName
                            })
                            .Select(x => new BindableQuestionnaire
                            {
                                QuestionnaireId = x.Key.QuestionnaireId,
                                QuestionnaireName = x.Key.QuestionnaireName,
                                Groups = x.Select(y => new BindableGroup
                                {
                                    SessionGroupId = y.Key.SessionGroupId,
                                    GroupName = y.Key.GroupName,
                                    Users = y.Users
                                })
                            });
        }

        public class BindableQuestionnaire
        {
            public int QuestionnaireId { get; set; }
            public string QuestionnaireName { get; set; }
            public IEnumerable<BindableGroup> Groups { get; set; }
        }

        public class BindableGroup
        {
            public int SessionGroupId { get; set; }
            public string GroupName { get; set; }
            public IEnumerable<BindableUser> Users { get; set; }
        }

        public class BindableUser
        {
            public string UserId { get; set; }
            public string UserName { get; set; }
            public string Key { get; set; }
        }
    }
}
