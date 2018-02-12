using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ProjectSurvey.Models
{
    public class SurveyUser : IdentityUser
    {
//        public new int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
//        public new string Email { get; set; }
//        public string Password { get; set; }

        public ICollection<UserSphere> User { get; set; }

    }
}
