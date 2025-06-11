namespace GraduProjj.DTOs.PlanDtos
{
    public class PlanItems
    {
        public decimal groceries { get; set; } // Matches "groceriesSavings" in JSON
        public decimal transport { get; set; }
        public decimal eatingOut { get; set; }
        public decimal entertainment { get; set; }
        public decimal utilities { get; set; }
        public decimal healthcare { get; set; }
        public decimal education { get; set; }
        public decimal otherMoney { get; set; }
        public decimal monthlySavings { get; set; } // Matches "monthlySavings" in JSON
		public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string status { get; set; }
    }

}
