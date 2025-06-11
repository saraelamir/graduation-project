
using GraduProjj.DTOs.PlanDtos;

namespace GraduProjj.DTOs.GoalDtos

{
    public class GoalRequestWithPlans
    {
        public string GoalName { get; set; }
        public decimal GoalAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string status { get; set; }
        public decimal? Has_Savings { get; set; }
        public List<PlanItems> Plans { get; set; } = new List<PlanItems>();

    }

}
