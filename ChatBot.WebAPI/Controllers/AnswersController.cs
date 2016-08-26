using AIMLbot;
using ChatBot.WebAPI.DTO;
using ChatBot.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace ChatBot.WebAPI.Controllers
{
  [RoutePrefix("api")]
  public class AnswersController : ApiController
  {
    private AppDBDataModelContext db = new AppDBDataModelContext();

    [HttpPost]
    public async Task<IHttpActionResult> PostAnswer([FromBody]SurveyAnswer answer)
    {
      try
      {
        var respondentIP = Request.GetClientIpAddress();
        Survey_Question nextQuestion = null;
        Survey_Answer surveyAnswer = null;
        Question question = null;
        const long DEFAULT_RESPONDENTID = 1;
        if (ValidateAnswer(answer))
        {
          var sAns = db.Survey_Answer.Where(e => e.QuestionId == answer.QuestionId).ToList();
          if (sAns.Count() == 0)
          {
            surveyAnswer = new Survey_Answer()
            {
              QuestionId = answer.QuestionId,
              RespondentId = DEFAULT_RESPONDENTID,
              Answer = answer.Answer,
              ipAdrs = respondentIP,
              RowChangeTimestamp = DateTime.UtcNow
            };
          }
          else
          {
            surveyAnswer = new Survey_Answer()
            {
              QuestionId = answer.QuestionId,
              RespondentId = sAns.Max(e => e.RespondentId) + 1,
              Answer = answer.Answer,
              ipAdrs = respondentIP,
              RowChangeTimestamp = DateTime.UtcNow
            };
          }
          db.Survey_Answer.Add(surveyAnswer);
          db.SaveChanges();
          var qt = await db.Survey_Question.FindAsync(answer.SurveyId, answer.QuestionId + 1).ConfigureAwait(false);
          if (qt != null && qt.Visibility_F != 0)
          {
            var qTrigger = await db.Question_Trigger.FindAsync(qt.Visibility_F, answer.QuestionId).ConfigureAwait(false);
            if (MatchCondition(answer, qTrigger))
            {
              // next available non zero question
              nextQuestion = GetNextQuestion(answer, isTriggeredQuestion: true);
              question = new Question
              {
                QuestionID = nextQuestion.QuestionId,
                question = nextQuestion.Question,
                AnswerType = (AnswerType)Enum.Parse(typeof(AnswerType), nextQuestion.AnswerFormat.Trim(), ignoreCase: true),
              };
              return Json(question);
            }
          }
          nextQuestion = GetNextQuestion(answer, isTriggeredQuestion: false);
          if (nextQuestion != null)
          {
            question = new Question
            {
              QuestionID = nextQuestion.QuestionId,
              question = nextQuestion.Question,
              AnswerType = (AnswerType)Enum.Parse(typeof(AnswerType), nextQuestion.AnswerFormat.Trim(), ignoreCase: true),
            };
            return Json(question);
          }
          else
          {
            return Ok("");
          }
        }
        return Content(HttpStatusCode.BadRequest, "Spam message. Please reenter the answer again");
      }
      catch (Exception ex)
      {
        return InternalServerError(ex);
      }
    }

    private Survey_Question GetNextQuestion(SurveyAnswer answer,bool isTriggeredQuestion=false)
    {
      return (isTriggeredQuestion) ? db.Survey_Question.Where(e => e.SurveyId == answer.SurveyId && e.Visibility_F != 0 && e.QuestionId > answer.QuestionId).FirstOrDefault()
                                   : db.Survey_Question.Where(e => e.SurveyId == answer.SurveyId && e.Visibility_F == 0 && e.QuestionId > answer.QuestionId).FirstOrDefault();
    }

    private bool MatchCondition(SurveyAnswer ans, Question_Trigger qTrigger)
    {
      var logicalOperators = new List<string>()
      {
        "==","!=","<",">","<=",">="
      };
      var matchType = qTrigger.MatchType.Trim().ToLower();
      if (logicalOperators.Contains(matchType))
      {
        var value = long.Parse(ans.Answer);
        var matchValue = long.Parse(qTrigger.MatchValue);
        switch (matchType)
        {
          case "==":
            return value == matchValue;
          case "!=":
            return value != matchValue;
          case "<":
            return value < matchValue;
          case ">":
            return value > matchValue;
          case "<=":
            return value <= matchValue;
          case ">=":
            return value >= matchValue;
          default:
            return false;
        }
      }
      if(matchType == "keyword")
      {
        var matchValue = qTrigger.MatchValue.Split(',').ToList();
        var answerList = ans.Answer.Split(' ').ToList();
        return answerList.Any(e => matchValue.Contains(e));
      }
      return true;
    }

    private bool ValidateAnswer(SurveyAnswer answer)
    {
      if (answer.AnswerType == AnswerType.Email)
      {
        var regex = @"^\w+@\w+\.(com|in|org|net|edu)$";
        var match = Regex.Match(answer.Answer.Trim(), regex, RegexOptions.IgnoreCase);
        return match.Success;
      }
      if (answer.AnswerType == AnswerType.Stars)
      {
        int val;
        if (int.TryParse(answer.Answer.Trim(), out val))
        {
          return true;
        }
        return false;
      }
      if(answer.AnswerType == AnswerType.Text)
      {
        Bot myBot = new Bot();
        //string pathToSettings = @"C:\Users\tpraka\Documents\Visual Studio 2015\Projects\ChatBot\ChatBot.WebAPI\AIML\config";
        myBot.loadSettings();
        User myUser = new User("consoleUser", myBot);
        myBot.isAcceptingUserInput = false;
        myBot.loadAIMLFromFiles();
        myBot.isAcceptingUserInput = true;
        Request r = new Request(answer.Answer.Trim(), myUser, myBot);
        Result res = myBot.Chat(r);
        if (res.Output.Contains("#next"))
          return true;
        else if (res.Output.Contains("#repeat"))
          return false;
      }
      return true;
    }

    private bool isQuesTriggerRequired(SurveyAnswer answer)
    {
      return true;
    }
  }
  public static class HttpRequestMessageExtensions
  {
    private const string HttpContext = "MS_HttpContext";
    private const string RemoteEndpointMessage = "System.ServiceModel.Channels.RemoteEndpointMessageProperty";

    public static string GetClientIpAddress(this HttpRequestMessage request)
    {
      if (request.Properties.ContainsKey(HttpContext))
      {
        dynamic ctx = request.Properties[HttpContext];
        if (ctx != null)
        {
          return ctx.Request.UserHostAddress;
        }
      }

      if (request.Properties.ContainsKey(RemoteEndpointMessage))
      {
        dynamic remoteEndpoint = request.Properties[RemoteEndpointMessage];
        if (remoteEndpoint != null)
        {
          return remoteEndpoint.Address;
        }
      }

      return null;
    }
  }
}
