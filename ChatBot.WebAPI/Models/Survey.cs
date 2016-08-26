namespace ChatBot.WebAPI
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Survey")]
    public partial class Survey
    {
        public long SurveyId { get; set; }

        public long CustId { get; set; }

        [Required]
        [StringLength(300)]
        public string SurveyName { get; set; }

        [StringLength(300)]
        public string EmailLink { get; set; }

        [StringLength(20)]
        public string EmbedLink { get; set; }

        [StringLength(1)]
        public string Captcha_F { get; set; }

        [Required]
        [StringLength(1)]
        public string Active_F { get; set; }

        public DateTime RowCreateTimestamp { get; set; }

        public DateTime RowUpdateTimestamp { get; set; }
    }
}
