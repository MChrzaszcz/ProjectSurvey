using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ProjectSurvey.Models;

namespace ProjectSurvey.Models
{
    public class UserSphere
    {

//        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UserId { get; set; }
        public int SphereId { get; set; }
        public int ResultId { get; set; }
        public DateTime CompleteSphereDate { get; set; }
        public string Title { get; set; }

        public SurveyUser User { get; set; }
        public Sphere Sphere { get; set; }
        public Result Result { get; set; }
        public ICollection<UserAnswer> UserAnswers { get; set; }
    }
}
