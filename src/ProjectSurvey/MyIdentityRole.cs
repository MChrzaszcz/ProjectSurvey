﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ProjectSurvey
{
    public class MyIdentityRole : IdentityRole<int>
    {
        public string Descriptin { get; set; }
    }
}
