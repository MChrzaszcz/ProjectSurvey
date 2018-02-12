using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSurvey.Models
{
    public class UserAnswer
    {
//        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int Id { get; set; }
        public int? UserSphereId { get; set; }
        public int AnswerId { get; set; }


        public Answer Answer { get; set; }
        public UserSphere UserSphere { get; set; }
    }
}
