namespace ChatBot.WebAPI
{
  using System;
  using System.Data.Entity;
  using System.ComponentModel.DataAnnotations.Schema;
  using System.Linq;

  public partial class AppDBDataModelContext : DbContext
  {
    public AppDBDataModelContext()
        : base("name=AppDBConn")
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }
    public virtual DbSet<Customer_Feedback> Customer_Feedback { get; set; }
    public virtual DbSet<Survey> Surveys { get; set; }
    public virtual DbSet<Survey_Answer> Survey_Answer { get; set; }
    public virtual DbSet<Survey_Question> Survey_Question { get; set; }
    public virtual DbSet<Question_Trigger> Question_Trigger { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Customer>()
          .Property(e => e.Name)
          .IsUnicode(false);

      modelBuilder.Entity<Customer>()
          .Property(e => e.Email)
          .IsUnicode(false);

      modelBuilder.Entity<Customer>()
          .Property(e => e.CustPassword)
          .IsUnicode(false);

      modelBuilder.Entity<Customer>()
          .Property(e => e.phone)
          .IsUnicode(false);

      modelBuilder.Entity<Customer>()
          .Property(e => e.Website)
          .IsUnicode(false);

      modelBuilder.Entity<Customer>()
          .Property(e => e.WebsiteScript)
          .IsUnicode(false);

      modelBuilder.Entity<Customer>()
          .Property(e => e.Active_F)
          .IsFixedLength()
          .IsUnicode(false);

      modelBuilder.Entity<Customer_Feedback>()
          .Property(e => e.Feedback)
          .IsUnicode(false);

      modelBuilder.Entity<Survey>()
          .Property(e => e.SurveyName)
          .IsUnicode(false);

      modelBuilder.Entity<Survey>()
          .Property(e => e.EmailLink)
          .IsUnicode(false);

      modelBuilder.Entity<Survey>()
          .Property(e => e.EmbedLink)
          .IsUnicode(false);

      modelBuilder.Entity<Survey>()
          .Property(e => e.Captcha_F)
          .IsFixedLength()
          .IsUnicode(false);

      modelBuilder.Entity<Survey>()
          .Property(e => e.Active_F)
          .IsFixedLength()
          .IsUnicode(false);

      modelBuilder.Entity<Survey_Answer>()
          .Property(e => e.Answer)
          .IsUnicode(false);

      modelBuilder.Entity<Survey_Answer>()
          .Property(e => e.ipAdrs)
          .IsUnicode(false);

      modelBuilder.Entity<Survey_Question>()
          .Property(e => e.Question)
          .IsUnicode(false);

      modelBuilder.Entity<Survey_Question>()
          .Property(e => e.AnswerFormat)
          .IsFixedLength()
          .IsUnicode(false);

      modelBuilder.Entity<Question_Trigger>()
          .Property(e => e.MatchType)
          .IsFixedLength()
          .IsUnicode(false);

      modelBuilder.Entity<Question_Trigger>()
          .Property(e => e.MatchValue)
          .IsUnicode(false);
    }
  }
}
