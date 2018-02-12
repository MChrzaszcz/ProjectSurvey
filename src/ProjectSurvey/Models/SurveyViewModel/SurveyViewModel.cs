using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSurvey.Models.SurveyViewModel
{
    public class SurveyViewModel
    {
        public  PaginatedList<Question> PaginatedList { get; set; }
        public int QuestionId { get; set; }
        public int AnswerId { get; set; }
        public string QuestionTitle { get; set; }
//        public List<int> RestAnswers { get; set; }
        public List<UncompletedQuestionAnswer> RestQuestionsAbswers { get; set; }
        public int Page { get; set; }

        public List<UserAnswer> UserAnswers { get; set; }

        public SurveyViewModel(PaginatedList<Question> paginatedList)
        {
            PaginatedList = paginatedList;
        }

    }
}
