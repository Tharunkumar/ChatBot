using ChatBot.WebAPI.DTO;
using ChatBot.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ChatBot.WebAPI.Controllers
{
  [RoutePrefix("api")]
  public class QuestionsController : ApiController
  {
    private AppDBDataModelContext db = new AppDBDataModelContext();
    // GET api/<controller>
    public IEnumerable<string> Get()
    {
      return new string[] { "value1", "value2" };
    }


    [HttpGet]
    [Route("Survey/{surveyId:long}/Question/{QuestionId:long}")]
    public async Task<IHttpActionResult> GetQuestion(long surveyId, long questionId)
    {
      Survey_Question questionDb  = await db.Survey_Question.FindAsync(surveyId, questionId).ConfigureAwait(false);

      if (questionDb == null)
      {
        return NotFound();
      }
      var question = new Question
      {
        SurveyId = questionDb.SurveyId,
        QuestionID = questionDb.QuestionId,
        question = questionDb.Question,
        AnswerType = (AnswerType)Enum.Parse(typeof(AnswerType), questionDb.AnswerFormat.Trim(), ignoreCase: true),
        QTriggerId = questionDb.Visibility_F
      };
      return Json(question);
    }

    // POST api/<controller>
    public void Post([FromBody]string value)
    {
    }

    // PUT api/<controller>/5
    public void Put(int id, [FromBody]string value)
    {
    }

    // DELETE api/<controller>/5
    public void Delete(int id)
    {
    }
  }
}