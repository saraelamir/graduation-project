namespace GraduProjj.DTOs.PlanDtos
{
    public class PlanResponse
    {
        public decimal groceriesSavings { get; set; } // Matches "groceriesSavings" in JSON
        public decimal transportSavings { get; set; }
        public decimal eatingOutSavings { get; set; }
        public decimal entertainmentSavings { get; set; }
        public decimal utilitiesSavings { get; set; }
        public decimal healthcareSavings { get; set; }
        public decimal educationSavings { get; set; }
        public decimal otherMoneySavings { get; set; }
        public decimal monthlySavings { get; set; } // Matches "monthlySavings" in JSON
		public int endDate { get; set; }

        public override string ToString()
        {
            return $@"Savings Summary:
                  Groceries: {groceriesSavings:C2}
                  Transport: {transportSavings:C2}
                  Eating Out: {eatingOutSavings:C2}
                  Entertainment: {entertainmentSavings:C2}
                  Utilities: {utilitiesSavings:C2}
                  Healthcare: {healthcareSavings:C2}
                  Education: {educationSavings:C2}
                  Other: {otherMoneySavings:C2}
				  Monthly Savings: {monthlySavings:C2}
                 ";// End Date: {endDate:yyyy-MM-dd}";
        }

    }

}
