using GraduProjj.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduProjj.Models
{
    public class Goal
    {
        [Key]
        public int GoalID { get; set; }
        public int UserID { get; set; }
        public string GoalName { get; set; }
        public decimal GoalAmount { get; set; }
        [Column(TypeName = "date")]
        public DateTime StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime EndDate { get; set; }

        public bool Status { get; set; }
        public decimal? Has_Savings { get; set; }
        public virtual User User { get; set; }
        //public virtual ICollection<Plan> Plan { get; set; }
        public virtual ICollection<Plan> Plans { get; set; } = new List<Plan>();

        public enum GoalStatus : byte
        {
            Active = 1,
            Completed = 2
        }
    }
}