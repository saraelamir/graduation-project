namespace GraduProjj.DTOs.GoalDtos
{
    public class GoalResponse
    {
        public int GoalId { get; set; }
        public string GoalName { get; set; }
        public decimal GoalAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Status { get; set; }

    }
}
