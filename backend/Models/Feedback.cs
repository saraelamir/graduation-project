using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduProjj.Models
{
    [Table("Feedbacks")]
    public class Feedback
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FeedbackId { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(20)")]
        public string NavigationEase { get; set; }

        [Required]
        [Range(1, 5)]
        public int BudgetHelpfulnessRating { get; set; }

        [Column(TypeName = "nvarchar(1000)")]
        public string? ConfusingFeatures { get; set; }

        [Column(TypeName = "nvarchar(1000)")]
        public string? DesiredFeatures { get; set; }

        [Column(TypeName = "nvarchar(1000)")]
        public string? RecommendationReason { get; set; }

        [Column(TypeName = "nvarchar(1000)")]
        public string? ImprovementSuggestion { get; set; }

        [Required]
        [Range(1, 5)]
        public int OverallSatisfaction { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime SubmissionDate { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "varchar(50)")]
        public string? IpAddress { get; set; }
    }
}
