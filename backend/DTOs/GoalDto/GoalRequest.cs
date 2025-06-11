namespace GraduProjj.DTOs.GoalDtos
{
    public class GoalRequest
    {
        public string GoalName { get; set; }
        public decimal GoalAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Status { get; set; }
        public decimal? Has_Savings { get; set; }


    }

}
