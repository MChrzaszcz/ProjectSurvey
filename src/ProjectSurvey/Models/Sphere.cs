using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSurvey.Models
{
    public class Sphere
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Title { get; set; }

        public ICollection<UserSphere> UserSpheres { get; set; }
        public ICollection<Question> Questions { get; set; }
    }
}
