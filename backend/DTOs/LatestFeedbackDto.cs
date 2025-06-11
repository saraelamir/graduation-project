namespace GraduProjj.DTOs
{
	public class LatestFeedbackDto
	{
		public string NavigationEase { get; set; }
		public int BudgetHelpfulnessRating { get; set; }
		public string? ConfusingFeatures { get; set; }
		public string? DesiredFeatures { get; set; }
		public string? RecommendationReason { get; set; }
		public string? ImprovementSuggestion { get; set; }
		public int OverallSatisfaction { get; set; }
		//public DateTime SubmissionDate { get; set; }
	}

}
