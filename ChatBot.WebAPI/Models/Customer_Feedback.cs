namespace ChatBot.WebAPI
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Customer_Feedback
    {
        public long CustId { get; set; }

        [Key]
        public long FeedbackId { get; set; }

        [Required]
        [StringLength(3000)]
        public string Feedback { get; set; }

        public DateTime RowCreateTimestamp { get; set; }
    }
}
