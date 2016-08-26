using ChatBot.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChatBot.WebAPI.DTO
{
  public class SurveyAnswer
  {
    [Required]
    public long SurveyId { get; set; }
    [Required]
    public long QuestionId { get; set; }
    public string Answer { get; set; }
    public AnswerType AnswerType { get; set; }
    public long QTriggerId { get; set; }
  }
}