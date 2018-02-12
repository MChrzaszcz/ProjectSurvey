using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectSurvey.Models;


namespace ProjectSurvey.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int SphereId { get; set; }

        public ICollection<Answer> Answers { get; set; }
        public Sphere Sphere { get; set; }

    }
}
