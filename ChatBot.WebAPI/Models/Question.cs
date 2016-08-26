using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatBot.WebAPI.Models
{
  public class Question
  {
    public long SurveyId { get; set; }
    public long QuestionID { get; set; }
    public string question { get; set; }
    public AnswerType AnswerType { get; set; }

    public long QTriggerId { get; set; }
  }
}