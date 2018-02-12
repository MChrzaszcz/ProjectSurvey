using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectSurvey.Data;
using ProjectSurvey.Models;

namespace ProjectSurvey.Data
{
    public class DbInitzializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
//            context.Database.EnsureDeleted();
            context.Database.Migrate();

            if (context.SurveyUsers.Any())
            {
                return;
            }

            var users = new SurveyUser[]
            {
                new SurveyUser() { FirstName = "John", LastName = "Travolta", Email = "Travolta@gmail.com"},
                new SurveyUser() { FirstName = "Doris", LastName = "Day", Email = "DDay@gmail.com"}
            };
            foreach (SurveyUser user in users)
            {
                context.SurveyUsers.Add(user);
            }
            context.SaveChanges();

            var spheres = new Sphere[]
            {
                new Sphere { Id = 1, Title = "Sfera fizyczna"},
                new Sphere { Id = 2, Title = "Sfera mentalna"}
            };

            foreach (Sphere sphere in spheres)
            {
                context.Spheres.Add(sphere);
            }
            context.SaveChanges();

            var results = new Result[]
            {
                new Result { Level = 0, Description = "Stan fatalny"},
                new Result { Level = 1, Description = "Stan bardzo zły"},
                new Result { Level = 2, Description = "Stan zły"},
                new Result { Level = 3, Description = "Stan stabilny"},
                new Result { Level = 4, Description = "Stan dobry"},
                new Result { Level = 5, Description = "Stan bardzo dobry"},
            };

            foreach (Result result in results)
            {
                context.Results.Add(result);
            }
            context.SaveChanges();

            var questions = new Question[]
            {
                new Question {Title = "Rozumie podstawowe polecenia słowne i gesty, czy polecenia słowne podparte gestem.", SphereId = spheres.Single(s => s.Title == "Sfera fizyczna").Id},
                new Question {Title = "Odpowiada na pytania o aktualne samopoczucie: „lubię to”/”nie lubię tego”, „czuję się dobrze/źle”", SphereId = spheres.Single(s => s.Title == "Sfera fizyczna").Id},
                new Question {Title = "Potrafi samodzielnie wskazywać (np. ręką, palcem wzrokiem), że chce daną rzecz.", SphereId = spheres.Single(s => s.Title == "Sfera fizyczna").Id},
                new Question {Title = "Wykonuje polecenia - z odroczeniem, odracza gratyfikację w czasie (np. chce mu się pić, ale wypije herbatę dopiero jak ostygnie, podczas przygotowywania wspólnego posiłku  nie podjada, „Jak skończysz to zadanie, dostaniesz nagrodę, [np. pobiegasz], w nagrodę pogra na komputerze jak będzie się cały dzień dobrze zachowywał na lekcjach, spokojnie czeka na autobus na przystanku aż on przyjedzie)",
                    SphereId = spheres.Single(s => s.Title == "Sfera mentalna").Id},
                new Question {Title = "Wykonuje instrukcje podane w formie wizualnej (np. w postaci zdjęć, rysunku, filmu instruktażowego)", SphereId = spheres.Single(s => s.Title == "Sfera mentalna").Id},
//                new Question {Title = "Pytanie kontrolne 3 - sfera mentalna", SphereId = spheres.Single(s => s.Title == "Sfera mentalna").Id},
            };

            foreach (Question question in questions)
            {
                context.Questions.Add(question);
            }
            context.SaveChanges();

            var answers = new Answer[]
            {
                new Answer {Title = "Nie potrafi/nie jest w ogóle zdolny tego wykonać", QuestionId = questions.Single(q => q.Title.Contains("Rozumie podstawowe")).Id},
                new Answer {Title = "Bardzo słabe albo słabe opanowanie wykonania danej czynności,  wymaga (i to najczęściej ciągłej lub rozległej) pomocy i podpowiedzi", QuestionId = questions.Single(q => q.Title.Contains("Rozumie podstawowe")).Id},
                new Answer {Title = "Połowiczne wykonuje, często wymaga wsparcia manualnego  słownego  lub demonstracji czynności", QuestionId = questions.Single(q => q.Title.Contains("Rozumie podstawowe")).Id},
                new Answer {Title = "Wykonuje samodzielnie, i czasem tylko wymaga kontroli  poprawności wykonania", QuestionId = questions.Single(q => q.Title.Contains("Rozumie podstawowe")).Id},
                new Answer {Title = "Wykonuje bardzo dobrze i samodzielnie", QuestionId = questions.Single(q => q.Title.Contains("Rozumie podstawowe")).Id},
                new Answer {Title = "Wykonuje ponadprzeciętnie (i całkowicie samodzielnie).", QuestionId = questions.Single(q => q.Title.Contains("Rozumie podstawowe")).Id},

                new Answer {Title = "Nie potrafi/nie jest w ogóle zdolny tego wykonać", QuestionId = questions.Single(q => q.Title.Contains("Odpowiada na pytania")).Id},
                new Answer {Title = "Bardzo słabe albo słabe opanowanie wykonania danej czynności,  wymaga (i to najczęściej ciągłej lub rozległej) pomocy i podpowiedzi", QuestionId = questions.Single(q => q.Title.Contains("Odpowiada na pytania")).Id},
                new Answer {Title = "Połowiczne wykonuje, często wymaga wsparcia manualnego  słownego  lub demonstracji czynności", QuestionId = questions.Single(q => q.Title.Contains("Odpowiada na pytania")).Id},
                new Answer {Title = "Wykonuje samodzielnie, i czasem tylko wymaga kontroli  poprawności wykonania", QuestionId = questions.Single(q => q.Title.Contains("Odpowiada na pytania")).Id},
                new Answer {Title = "Wykonuje bardzo dobrze i samodzielnie", QuestionId = questions.Single(q => q.Title.Contains("Odpowiada na pytania")).Id},
                new Answer {Title = "Wykonuje ponadprzeciętnie (i całkowicie samodzielnie).", QuestionId = questions.Single(q => q.Title.Contains("Odpowiada na pytania")).Id},

                new Answer {Title = "Nie potrafi/nie jest w ogóle zdolny tego wykonać", QuestionId = questions.Single(q => q.Title.Contains("Potrafi samodzielnie wskazywać")).Id},
                new Answer {Title = "Bardzo słabe albo słabe opanowanie wykonania danej czynności,  wymaga (i to najczęściej ciągłej lub rozległej) pomocy i podpowiedzi", QuestionId = questions.Single(q => q.Title.Contains("Potrafi samodzielnie wskazywać")).Id},
                new Answer {Title = "Połowiczne wykonuje, często wymaga wsparcia manualnego  słownego  lub demonstracji czynności", QuestionId = questions.Single(q => q.Title.Contains("Potrafi samodzielnie wskazywać")).Id},
                new Answer {Title = "Wykonuje samodzielnie, i czasem tylko wymaga kontroli  poprawności wykonania", QuestionId = questions.Single(q => q.Title.Contains("Potrafi samodzielnie wskazywać")).Id},
                new Answer {Title = "Wykonuje bardzo dobrze i samodzielnie", QuestionId = questions.Single(q => q.Title.Contains("Potrafi samodzielnie wskazywać")).Id},
                new Answer {Title = "Wykonuje ponadprzeciętnie (i całkowicie samodzielnie).", QuestionId = questions.Single(q => q.Title.Contains("Potrafi samodzielnie wskazywać")).Id},

                new Answer {Title = "Nie potrafi/nie jest w ogóle zdolny tego wykonać", QuestionId = questions.Single(q => q.Title.Contains("Wykonuje polecenia -")).Id},
                new Answer {Title = "Bardzo słabe albo słabe opanowanie wykonania danej czynności,  wymaga (i to najczęściej ciągłej lub rozległej) pomocy i podpowiedzi", QuestionId = questions.Single(q => q.Title.Contains("Wykonuje polecenia -")).Id},
                new Answer {Title = "Połowiczne wykonuje, często wymaga wsparcia manualnego  słownego  lub demonstracji czynności", QuestionId = questions.Single(q => q.Title.Contains("Wykonuje polecenia -")).Id},
                new Answer {Title = "Wykonuje samodzielnie, i czasem tylko wymaga kontroli  poprawności wykonania", QuestionId = questions.Single(q => q.Title.Contains("Wykonuje polecenia -")).Id},
                new Answer {Title = "Wykonuje bardzo dobrze i samodzielnie", QuestionId = questions.Single(q => q.Title.Contains("Wykonuje polecenia -")).Id},
                new Answer {Title = "Wykonuje ponadprzeciętnie (i całkowicie samodzielnie).", QuestionId = questions.Single(q => q.Title.Contains("Wykonuje polecenia -")).Id},

                new Answer {Title = "Nie potrafi/nie jest w ogóle zdolny tego wykonać", QuestionId = questions.Single(q => q.Title.Contains("Wykonuje instrukcje podane")).Id},
                new Answer {Title = "Bardzo słabe albo słabe opanowanie wykonania danej czynności,  wymaga (i to najczęściej ciągłej lub rozległej) pomocy i podpowiedzi", QuestionId = questions.Single(q => q.Title.Contains("Wykonuje instrukcje podane")).Id},
                new Answer {Title = "Połowiczne wykonuje, często wymaga wsparcia manualnego  słownego  lub demonstracji czynności", QuestionId = questions.Single(q => q.Title.Contains("Wykonuje instrukcje podane")).Id},
                new Answer {Title = "Wykonuje samodzielnie, i czasem tylko wymaga kontroli  poprawności wykonania", QuestionId = questions.Single(q => q.Title.Contains("Wykonuje instrukcje podane")).Id},
                new Answer {Title = "Wykonuje bardzo dobrze i samodzielnie", QuestionId = questions.Single(q => q.Title.Contains("Wykonuje instrukcje podane")).Id},
                new Answer {Title = "Wykonuje ponadprzeciętnie (i całkowicie samodzielnie).", QuestionId = questions.Single(q => q.Title.Contains("Wykonuje instrukcje podane")).Id},




//                new Answer {Title = "Odp1 pyt1 - sfera mentalna", QuestionId = questions.Single(q => q.Title.Contains("3 - sfera fizyczna")).Id},
//                new Answer {Title = "Odp2 pyt1 - sfera mentalna", QuestionId = questions.Single(q => q.Title.Contains("1 - sfera mentalna")).Id},
//                new Answer {Title = "Odp3 pyt1 - sfera mentalna", QuestionId = questions.Single(q => q.Title.Contains("2 - sfera mentalna")).Id},
//                new Answer {Title = "Odp4 pyt1 - sfera mentalna", QuestionId = questions.Single(q => q.Title.Contains("3 - sfera mentalna")).Id},
            };

            foreach (Answer answer in answers)
            {
                context.Answers.Add(answer);
            }
            context.SaveChanges();


            var userSpheres = new UserSphere[]
            {
                new UserSphere
                {
                    SphereId = spheres.Single(s => s.Title == "Sfera fizyczna").Id, UserId = users.Single(u => u.LastName == "Travolta").Id,
                    ResultId = results.Single(r => r.Level == 2).ResultId, CompleteSphereDate = DateTime.Parse("2007-01-01"), Title = "Fizyczna"
                },
                 new UserSphere
                {
                    SphereId = spheres.Single(s => s.Title == "Sfera fizyczna").Id, UserId = users.Single(u => u.LastName == "Travolta").Id,
                    ResultId = results.Single(r => r.Level == 0).ResultId, CompleteSphereDate = DateTime.Parse("2007-03-28"), Title = "Fizyczna"
                },
                  new UserSphere
                {
                    SphereId = spheres.Single(s => s.Title == "Sfera fizyczna").Id, UserId = users.Single(u => u.LastName == "Travolta").Id,
                    ResultId = results.Single(r => r.Level == 3).ResultId, CompleteSphereDate = DateTime.Parse("2007-05-14"), Title = "Fizyczna"
                },
                   new UserSphere
                {
                    SphereId = spheres.Single(s => s.Title == "Sfera fizyczna").Id, UserId = users.Single(u => u.LastName == "Travolta").Id,
                    ResultId = results.Single(r => r.Level == 2).ResultId, CompleteSphereDate = DateTime.Parse("2007-10-24"), Title = "Fizyczna"
                },
                    new UserSphere
                {
                    SphereId = spheres.Single(s => s.Title == "Sfera fizyczna").Id, UserId = users.Single(u => u.LastName == "Travolta").Id,
                    ResultId = results.Single(r => r.Level == 5).ResultId, CompleteSphereDate = DateTime.Parse("2007-12-30"), Title = "Fizyczna"
                },
                     new UserSphere
                {
                    SphereId = spheres.Single(s => s.Title == "Sfera fizyczna").Id, UserId = users.Single(u => u.LastName == "Travolta").Id,
                    ResultId = results.Single(r => r.Level == 3).ResultId, CompleteSphereDate = DateTime.Parse("2008-02-11"), Title = "Fizyczna"
                },
                new UserSphere
                {
                    SphereId = spheres.Single(s => s.Title == "Sfera mentalna").Id, UserId = users.Single(u => u.LastName == "Travolta").Id,
                    ResultId = results.Single(r => r.Level == 5).ResultId, CompleteSphereDate = DateTime.Parse("2008-09-12"), Title = "Mentalna"
                },

            };

            foreach (UserSphere userSphere in userSpheres)
            {
                context.UserSpheres.Add(userSphere);
            }
            context.SaveChanges();
//
//            var userAnswers = new UserAnswer[]
//            {
//                new UserAnswer {AnswerId = answers.Single(a => a.Title.Contains("Odp1 pyt1 - sfera fizyczna")).Id,
//                    UserSphereId = userSpheres.Single(u => u.Title == "Fizyczna").SphereId},
//                new UserAnswer {AnswerId = answers.Single(a => a.Title.Contains("Odp1 pyt1 - sfera mentalna")).Id,
//                    UserSphereId = userSpheres.Single(u => u.Title == "Mentalna").SphereId},
//            };
        }
    }
}
