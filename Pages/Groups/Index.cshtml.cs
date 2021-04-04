using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SimplzQuestionnaire.Interfaces;
using SimplzQuestionnaire.Model;

namespace SimplzQuestionnaire.Pages.Groups
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public List<BindableQuestionnaire> Items { get; set; }
        public Dictionary<int, List<SelectListItem>> Groups { get; set; }

        private readonly ICurrentUserService _currentUser;
        private readonly SQContext _context;
        public IndexModel(ICurrentUserService currentUser, SQContext context)
        {
            _currentUser = currentUser;
            _context = context;
        }

        public void OnPost()
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var questionnaire in Items)
                    {
                        if (!string.IsNullOrEmpty(questionnaire.NewGroup))
                        {
                            var sg = new SessionGroup { QuestionnaireId = questionnaire.QuestionnaireId, Name = questionnaire.NewGroup };
                            _context.SessionGroups.Add(sg);
                            _context.SaveChanges();
                        }
                        foreach (var group in questionnaire.Groups)
                        {
                            foreach (var user in group.Users)
                            {
                                if (user.SessionGroupId != user.PreviousSessionGroupId)
                                {
                                    var uOld = new SessionGroupUser { UserId = user.UserId, SessionGroupId = user.PreviousSessionGroupId };
                                    var nOld = new SessionGroupUser { UserId = user.UserId, SessionGroupId = user.SessionGroupId };
                                    if (user.PreviousSessionGroupId > 0) _context.SessionGroupUsers.Remove(uOld);
                                    if (user.SessionGroupId > 0) _context.SessionGroupUsers.Add(nOld);
                                    _context.SaveChanges();
                                }
                            }
                        }
                    }
                    transaction.Commit();
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    transaction.Rollback();
                }
            }
            OnGet();
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

            Items = assigned.Union(unassigned)
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
                                    Key = y.Key,
                                    SessionGroupId = y.SessionGroupId,
                                    PreviousSessionGroupId = y.SessionGroupId
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
                                    Users = y.Users.ToList()
                                }).ToList()
                            })
                            .ToList();

            Groups = (from questionnaire in _context.Questionnaires
                      join sessionGroup in _context.SessionGroups on questionnaire.QuestionnaireId equals sessionGroup.QuestionnaireId
                      select new { questionnaire.QuestionnaireId, sg = new SelectListItem { Value = sessionGroup.SessionGroupId.ToString(), Text = sessionGroup.Name } })
                      .ToList()
                      .GroupBy(x => x.QuestionnaireId)
                      .ToDictionary(x => x.Key, x => x.Select(x => x.sg).Concat(new[] { new SelectListItem { Value = "0", Text = "Unassigned" } }).ToList());
        }

        public class BindableQuestionnaire
        {
            public int QuestionnaireId { get; set; }
            public string QuestionnaireName { get; set; }
            public List<BindableGroup> Groups { get; set; }
            public string NewGroup { get; set; }
        }

        public List<SelectListItem> GetQuestionnaires()
        {
            return Items.Select(x => new SelectListItem { Text = x.QuestionnaireName, Value = x.QuestionnaireId.ToString() }).ToList();
        }

        public class BindableGroup
        {
            public int SessionGroupId { get; set; }
            public string GroupName { get; set; }
            public List<BindableUser> Users { get; set; }
            public int QuestionnaireId { get; set; }
        }

        public class BindableUser
        {
            public string UserId { get; set; }
            public string UserName { get; set; }
            public string Key { get; set; }
            public int SessionGroupId { get; set; }
            public int PreviousSessionGroupId { get; set; }
        }
    }
}
