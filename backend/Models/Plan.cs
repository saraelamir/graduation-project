using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduProjj.Models
{
    public class Plan
    {
        [Key]
        public int PlanID { get; set; }
        //public int UserID { get; set; }
        [ForeignKey(nameof(GoalID))]
        public int GoalID { get; set; }
        //  public string Name { get; set; }
        //public int UserID { get; set; }
        //[ForeignKey(nameof(UserID))]
        public decimal MonthlySavings { get; set; }
        [Column(TypeName = "date")]
        public DateTime StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime EndDate { get; set; }

        public bool Status { get; set; }
        public decimal Groceries { get; set; }
        public decimal Transport { get; set; }
        public decimal EatingOut { get; set; }
        public decimal Entertainment { get; set; }
        public decimal Utilities { get; set; }
        public decimal Healthcare { get; set; }
        public decimal Education { get; set; }
        public decimal OtherMoney { get; set; }

        public enum PlanStatus : byte
        {
            Active = 1,
            Cancelled = 2
        }

        //public List<(string,int)> ? PlanItems { get; set; }



        // public bool 

        // public virtual User User { get; set; }

        //[ForeignKey("GoalID")]
        public virtual Goal Goal { get; set; }
        //public virtual User User { get; set; }
    }
}