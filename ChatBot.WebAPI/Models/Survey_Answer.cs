namespace ChatBot.WebAPI
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Survey_Answer
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long QuestionId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long RespondentId { get; set; }

        [Required]
        [StringLength(2000)]
        public string Answer { get; set; }

        [StringLength(100)]
        public string ipAdrs { get; set; }

        public DateTime RowChangeTimestamp { get; set; }
    }
}
