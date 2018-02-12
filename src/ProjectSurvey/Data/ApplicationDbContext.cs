using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ProjectSurvey.Models;

namespace ProjectSurvey.Data
{
    public class ApplicationDbContext : IdentityDbContext<SurveyUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<SurveyUser> SurveyUsers { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<Sphere> Spheres { get; set; }
        public DbSet<UserSphere> UserSpheres { get; set; }
        public DbSet<UserAnswer> UserAnswers { get; set; }
        public IdentityRole<int> IdentityRole { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<SurveyUser>().ToTable("User");
            builder.Entity<Question>().ToTable("Question");
            builder.Entity<Answer>().ToTable("Answer");
            builder.Entity<Result>().ToTable("Result");
            builder.Entity<Sphere>().ToTable("Sphere");
            builder.Entity<UserSphere>().ToTable("UserSphere");
            builder.Entity<UserAnswer>().ToTable("UserAnswer");

//            builder.Entity<UserSphere>()
//                .HasKey(c => new { c.SphereId, c.UserId });
//            builder.Entity<UserAnswer>()
//                .HasKey(c => new { c.AnswerId, c.UserSphereId });



            base.OnModelCreating(builder);
                       
            
            



            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }

}
