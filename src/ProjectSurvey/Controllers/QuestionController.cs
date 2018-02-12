using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using ProjectSurvey.Data;
using ProjectSurvey.Models;
using ProjectSurvey.Models.SurveyViewModel;

namespace ProjectSurvey.Controllers
{
    public class QuestionController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<SurveyUser> _userManager;

        public QuestionController(ApplicationDbContext context, UserManager<SurveyUser> userManager )
        {
            _context = context;
            _userManager = userManager;
        }

//        // GET: Question
//        public async Task<IActionResult> Index()
//        {
//            var applicationDbContext = _context.Questions.Include(q => q.Sphere);
//            return View(await applicationDbContext.ToListAsync());
//        }

        
        public void addQuestionToCookie(int answerId, int questionId)
        {       
                Response.Cookies.Append(answerId.ToString(), questionId.ToString());   

        }


        IQueryable<Question> getUncompletedQuestions(IQueryable<Question> questions)
        {
            string[] cookieKeys = Request.Cookies.Keys.ToArray();
            List<Question> uncompletedQuestionIdList = new List<Question>();
//            var questions = _context.Questions.Include(q => q.Answers);

            for (int i = 3; i < Request.Cookies.Count; i++)                //index of cookies: 0 - antiforgery, 1 - identity, 2... - user answers
            {
               int uncompletedQuestionId = Int32.Parse(cookieKeys[i]);
                if (Request.Cookies[cookieKeys[i]] == "0")
                {
                    Question uncompletedQuestion = questions.Single(q => q.Id == uncompletedQuestionId);
                    uncompletedQuestionIdList.Add(uncompletedQuestion);
                }              
            }
            return uncompletedQuestionIdList.AsQueryable();
        }


        int getLastAnswer(int currentQuestionId, IQueryable<Question> questions)
        {
            string[] cookieKeys = Request.Cookies.Keys.ToArray();

            int previousQuestionId = -1;
            foreach (Question question in questions)
                         {
                            if (question.Id == currentQuestionId)
                            {
            
                                for (int i = 3; i < Request.Cookies.Count; i++)    //index of cookies: 0 - antiforgery, 1 - identity, 2... - user answers
                                {
                                    if (previousQuestionId.ToString() == cookieKeys[i])
                                    {
                                         return Int32.Parse(Request.Cookies[cookieKeys[i]]); //last answer
                                    }
                                }
                            }
                             previousQuestionId = question.Id;
                        }

             return -1;     //if dont find return -1;
        }

        int getNextAnswer(int currentQuestionId, IQueryable<Question> questions)
        {

            string[] cookieKeys = Request.Cookies.Keys.ToArray();

            int nextQuestionId = -1;
            bool isNext= false;
            foreach (Question question in questions)
            {
                if (question.Id == currentQuestionId)
                {
                    isNext = true;
                    continue;
                }

                if (isNext)
                {
                    nextQuestionId = question.Id; 
                
                for (int i = 3; i < Request.Cookies.Count; i++)    //index of cookies: 0 - antiforgery, 1 - identity, 2... - user answers
                    {
                        if (nextQuestionId.ToString() == cookieKeys[i])
                        {
                            return Int32.Parse(Request.Cookies[cookieKeys[i]]); //next answer
                        }
                    }
                }
            }

            return -1;     //if dont find return -1;
        }


        [HttpGet]
       public async Task<IActionResult> Index(
         int? pageSize,
         int? pageIndex,
         int sphereId,
         bool newSurvey
         )  
        {
          
            pageIndex = null;              //default value
            bool isUncompletedQuestions = false;
            ViewData["pageIndex"] = HttpContext.Session.GetInt32("pageIndex");
//            TempData["checkedAnswerId"] = -1;

            if (newSurvey)
            {
                HttpContext.Session.Clear();
            }

            if (sphereId != 0)
            {
                HttpContext.Session.SetInt32("sphereId", sphereId);
            }
            IQueryable<Question> questions = _context.Questions.Where(q => q.SphereId == (int)HttpContext.Session.GetInt32("sphereId")).Include(q => q.Answers);

           
            if (HttpContext.Session.Keys.Count() <= 1)        //initzialize
            {
                HttpContext.Session.SetInt32("pageIndex", 1);
                ViewData["pageIndex"] = 1;
                TempData["sendData"] = false;
                TempData["checkedAnswerId"] = -1;
                HttpContext.Session.SetInt32("answerId", -1);
            }
            else pageIndex = HttpContext.Session.GetInt32("pageIndex");

            if (TempData["isLastQuestion"] == null)
            {
               TempData["isLastQuestion"] = false;
                HttpContext.Session.SetString("isLastQuestion", "");
            }

            if (ViewData["isPaginationButtonsVisible"] == null)
                ViewData["isPaginationButtonsVisible"] = true;

            if ((bool)TempData["isLastQuestion"])
            {
                ViewData["isPaginationButtonsVisible"] = false;
                questions = getUncompletedQuestions(questions);
                isUncompletedQuestions = questions.Any();
                pageSize = questions.Count() + 1;             
            }

            if (HttpContext.Session.GetInt32("questionId") != null)
            {
                if (HttpContext.Session.GetString("direction") == "next")
                {
                TempData["checkedAnswerId"] = getNextAnswer((int)HttpContext.Session.GetInt32("questionId"), questions);
                }
                else 
                TempData["checkedAnswerId"] = getLastAnswer((int)HttpContext.Session.GetInt32("questionId"), questions);
//                HttpContext.Session.SetInt32("questionId", -1);                
            }
//            IQueryable<UserAnswer> userAnswers = _context.UserAnswers;


            //            if ((bool)ViewData["endListOfQuestion"]  && !hasAllCompletedAnswer())
            //             {  
            //                 List<int> pageOfUncompletedQuestions = getPageOfUncompletedQuestions();
            //                 foreach (int uncompletedQuestionId in pageOfUncompletedQuestions)
            //                 {
            //                    questions = _context.Questions.Where(q => q.Id == uncompletedQuestionId).Include(q => q.Answers);
            //                 }
            //                 pageSize = pageOfUncompletedQuestions.Count + 1;               
            //             }


            return  View(new SurveyViewModel(PaginatedList<Question>.CreateAsync(questions, pageIndex ?? 1, pageSize ?? 1, isUncompletedQuestions)));
        }

   


        public IActionResult HandleQuestion(int answerId, int questionId, string Command,
            List<UncompletedQuestionAnswer> restQuestionsAbswers)
        {  
            int pageIndex;
//            HttpContext.Session.SetInt32("answerId", answerId);
            pageIndex = (int) HttpContext.Session.GetInt32("pageIndex");
            HttpContext.Session.SetInt32("questionId", questionId);

            if (Command == "Next")
            {
                HttpContext.Session.SetInt32("pageIndex", pageIndex + 1);
                HttpContext.Session.SetString("direction", "next");
            }
            else if (Command == "Preview")
            {
                HttpContext.Session.SetInt32("pageIndex", pageIndex - 1);
                HttpContext.Session.SetString("direction", "preview");
            }
            else if (Command == "SendAnswers")
            {
                saveInformations();
            }

            HttpContext.Session.SetString("isLastQuestion", "");
                addQuestionToCookie(questionId, answerId);
                 
            return RedirectToAction("Index");
        }

        void saveInformations()
        {
            UserSphere userSphere = new UserSphere();
            userSphere.UserId = _userManager.GetUserId(HttpContext.User);
//            userSphere.UserId = 1;                       //test value
            userSphere.CompleteSphereDate = DateTime.Now;
            userSphere.ResultId = 5;
            _context.UserSpheres.Add(userSphere);
            _context.SaveChanges();

            string[] cookieKeys = Request.Cookies.Keys.ToArray();
            for (int i = 3; i < cookieKeys.Length; i++)   //index of cookies: 0 - antiforgery, 1 - identity, 2 - session, 3... - user questions
            {
                UserAnswer userAnswer = new UserAnswer()
                {
                    UserSphereId = userSphere.Id,
                    AnswerId = Int32.Parse(cookieKeys[i])
                };
                _context.UserAnswers.Add(userAnswer);                                         
            };
            _context.SaveChanges();                  // save to database; generates id for userSphere and userAnswer             
        }




        // GET: Question/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Questions.SingleOrDefaultAsync(m => m.Id == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        // GET: Question/Create
        public IActionResult Create()
        {
            ViewData["SphereId"] = new SelectList(_context.Spheres, "Id", "Id");
            return View();
        }

        // POST: Question/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SphereId,Title")] Question question)
        {
            if (ModelState.IsValid)
            {
                _context.Add(question);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["SphereId"] = new SelectList(_context.Spheres, "Id", "Id", question.SphereId);
            return View(question);
        }

        // GET: Question/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Questions.SingleOrDefaultAsync(m => m.Id == id);
            if (question == null)
            {
                return NotFound();
            }
            ViewData["SphereId"] = new SelectList(_context.Spheres, "Id", "Id", question.SphereId);
            return View(question);
        }

        // POST: Question/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SphereId,Title")] Question question)
        {
            if (id != question.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(question);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionExists(question.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["SphereId"] = new SelectList(_context.Spheres, "Id", "Id", question.SphereId);
            return View(question);
        }

        // GET: Question/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Questions.SingleOrDefaultAsync(m => m.Id == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        // POST: Question/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var question = await _context.Questions.SingleOrDefaultAsync(m => m.Id == id);
            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool QuestionExists(int id)
        {
            return _context.Questions.Any(e => e.Id == id);
        }
    }
}
