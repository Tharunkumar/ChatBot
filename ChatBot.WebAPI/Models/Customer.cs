namespace ChatBot.WebAPI
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Customer")]
    public partial class Customer
    {
        [Key]
        public long CustId { get; set; }

        [StringLength(200)]
        public string Name { get; set; }

        [Required]
        [StringLength(200)]
        public string Email { get; set; }

        [Required]
        [StringLength(30)]
        public string CustPassword { get; set; }

        public int CustLoginAttempts { get; set; }

        [StringLength(20)]
        public string phone { get; set; }

        [StringLength(100)]
        public string Website { get; set; }

        [StringLength(300)]
        public string WebsiteScript { get; set; }

        [StringLength(1)]
        public string Active_F { get; set; }

        public DateTime? LastLogin { get; set; }

        public DateTime? RowCreateTimestamp { get; set; }

        public DateTime? RowUpdateTimestamp { get; set; }
    }
}
