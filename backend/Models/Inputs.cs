using GraduProjj.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduProjj.Models
{
    public class Input
    {
        [Key]
        public int InputID { get; set; }
        public int UserID { get; set; }
        public int age { get; set; }
        public int dependents { get; set; }
        public string occupation { get; set; }
        public string city_tier { get; set; }
        public decimal income { get; set; }
        public decimal rent { get; set; }
        public decimal loanPayment { get; set; }
        public decimal insurance { get; set; }
        public decimal groceries { get; set; }
        public decimal transport { get; set; }
        public decimal eatingOut { get; set; }
        public decimal entertainment { get; set; }
        public decimal utilities { get; set; }
        public decimal healthcare { get; set; }
        public decimal education { get; set; }
        public decimal otherMoney { get; set; }

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

		// Navigation property
		public virtual User User { get; set; }

    }
}