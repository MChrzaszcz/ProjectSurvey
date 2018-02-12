using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ProjectSurvey.Config
{
    public class SurveyUserValidator<TUser> : IUserValidator<TUser>
        where TUser : IdentityUser
    {
        private readonly List<string> _allowedDomains = new List<string>
    {
        "elanderson.net",
        "test.com"
    };

        public Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user)
        {

            return null;
        }
        
    }
}
