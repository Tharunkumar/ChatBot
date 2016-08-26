using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChatBot.WebAPI.Controllers
{
  public class HomeController : Controller
  {
    private AppDBDataModelContext db = new AppDBDataModelContext();
    // GET: Home
    [HttpGet]
    [Route("{surveyId}")]
    public ActionResult Index(string surveyId)
    {
      var result = (from s in db.Surveys
                    join sq in db.Survey_Question on s.SurveyId equals sq.SurveyId
                    where s.EmailLink == surveyId
                    orderby sq.QuestionId
                    select new
                    {
                      SurveyId = s.SurveyId,
                      QuestionId = sq.QuestionId
                    }).FirstOrDefault();
      if (result != null)
      {
        ViewBag.ReqSurveyId = result.SurveyId;
        ViewBag.ReqQuestionId = result.QuestionId;
        return View();
      }
      else
      {
        return new HttpNotFoundResult("The Requested Survey is not found");
      }
    }
  }
}