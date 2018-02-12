using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSurvey.Models
{
    public class Result
    {
        public int ResultId { get; set; }
        public int Level { get; set; }
        public string Description { get; set; }

        public ICollection<UserSphere> UserSpheres { get; set; }
    }
}
