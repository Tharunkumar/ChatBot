namespace ChatBot.WebAPI
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Survey_Question
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long SurveyId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long QuestionId { get; set; }

        [StringLength(1000)]
        public string Question { get; set; }

        [StringLength(10)]
        public string AnswerFormat { get; set; }

        public long Visibility_F { get; set; }

        public DateTime RowChangeTimestamp { get; set; }
    }
}
