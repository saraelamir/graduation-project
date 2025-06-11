using GraduProjj.Data;
using GraduProjj.Models;
using GraduProjj.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace GraduProjj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FeedbackController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FeedbackController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> SubmitFeedback([FromBody] FeedbackDto feedbackDto)
        {
            try
            {
                if (feedbackDto == null)
                {
                    Console.WriteLine("⚠️ FeedbackDto is null.");
                    return BadRequest(new { Message = "Invalid feedback data" });
                }

                // استخرج الـ claim
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                Console.WriteLine($"🔍 Extracted ClaimTypes.NameIdentifier: {userIdClaim}");

                // تحقق من الـ claim
                if (string.IsNullOrEmpty(userIdClaim))
                {
                    Console.WriteLine("❌ userIdClaim is null or empty.");
                    return Unauthorized(new { Message = "User not authorized - claim missing" });
                }

                // تحقق من النوع
                if (!int.TryParse(userIdClaim, out var userIdFromToken))
                {
                    Console.WriteLine($"❌ Failed to parse userIdClaim: '{userIdClaim}' to int.");
                    return Unauthorized(new { Message = "User not authorized - invalid user id format" });
                }

                Console.WriteLine($"✅ User ID from token: {userIdFromToken}");

                var feedback = new Feedback
                {
                    UserId = userIdFromToken,
                    NavigationEase = feedbackDto.NavigationEase,
                    BudgetHelpfulnessRating = feedbackDto.BudgetHelpfulnessRating,
                    ConfusingFeatures = feedbackDto.ConfusingFeatures,
                    DesiredFeatures = feedbackDto.DesiredFeatures,
                    RecommendationReason = feedbackDto.RecommendationReason,
                    ImprovementSuggestion = feedbackDto.ImprovementSuggestion,
                    OverallSatisfaction = feedbackDto.OverallSatisfaction,
                    SubmissionDate = DateTime.UtcNow
                };

                _context.Feedbacks.Add(feedback);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Feedback submitted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred", Error = ex.Message });
            }
        }

		[HttpGet("latest-feedback")]
		[Authorize]
		public async Task<IActionResult> GetLatestFeedback()
		{
			var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (!int.TryParse(userIdStr, out var userId))
				return Unauthorized();

			var latestFeedback = await _context.Feedbacks
				.Where(f => f.UserId == userId)
				.OrderByDescending(f => f.FeedbackId)
				.Select(f => new LatestFeedbackDto
				{
					NavigationEase = f.NavigationEase,
					BudgetHelpfulnessRating = f.BudgetHelpfulnessRating,
					ConfusingFeatures = f.ConfusingFeatures,
					DesiredFeatures = f.DesiredFeatures,
					RecommendationReason = f.RecommendationReason,
					ImprovementSuggestion = f.ImprovementSuggestion,
					OverallSatisfaction = f.OverallSatisfaction,
					//SubmissionDate = f.SubmissionDate
				})
				.FirstOrDefaultAsync();

			if (latestFeedback == null)
				return NotFound("No feedback data found.");

			return Ok(latestFeedback);
		}





	}
}
