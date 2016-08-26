namespace ChatBot.WebAPI
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Question_Trigger
    {
        [Key]
        [Column(Order = 0)]
        public long TriggerId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long QuestionId { get; set; }

        [StringLength(10)]
        public string MatchType { get; set; }

        [StringLength(3000)]
        public string MatchValue { get; set; }

        [Column(Order = 2)]
        public DateTime RowChangeTimestamp { get; set; }
    }
}
