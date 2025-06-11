using GraduProjj.Dtos;
using GraduProjj.DTOs.GoalDtos;
using GraduProjj.Interfaces;
using GraduProjj.Mapping;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using GraduProjj.Data;

namespace GraduProjj.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class Goalcontroller : ControllerBase
    {
        private readonly IGoalRepository _goalRepository;
        private readonly IInputRepository _inputRepository;
        private readonly IPlanRepository _planRepository;
        private readonly ApplicationDbContext _context;

        public Goalcontroller(IGoalRepository goalRepository, IInputRepository inputRepository, IPlanRepository planRepository, ApplicationDbContext context)
        {
            _goalRepository = goalRepository;
            _inputRepository = inputRepository;
            _planRepository = planRepository;
            _context = context; 
		}

        // استخراج userId من التوكن
        private int? GetUserIdFromToken()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return null;
            return userId;
        }




        [HttpGet("current_goal_with_Current_Plan")]
        public async Task<ActionResult<GoalRequestWithPlans>> GetCurrentGoalDetails()
        {
            var userId = GetUserIdFromToken();
            if (userId == null) return Unauthorized("User not authorized");

            var goal = await _goalRepository.GetCurrentGoal_with_Current_Plan(userId.Value);
            if (goal == null) return NotFound("There is No CurrentGoal for user");

            return Ok(goal.Convert_To_GoalRequestWithPlans());
        }

       


        [HttpGet("AllCompleted_goals")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<GoalRequestWithPlans>> GetAllCompletedGoals()
        {
            var CompletedGoals = await _goalRepository.GetAllCompletedGoals_WithPlans(GetUserIdFromToken().Value);

            if (!CompletedGoals.Any())
            {

                return NotFound("No Completed Goal For this User");
            }

            return Ok(CompletedGoals.Convert_To_GoalRequestWithPlans());

        }



        [HttpPost("add_goal")]
        public async Task<ActionResult> AddGoal(GoalNameAmountRequest goalDto)
        {
            var userId = GetUserIdFromToken();
            if (userId == null) return Unauthorized("User not authorized");

            bool isExist = await _goalRepository.IsExist_CurrentGoal_ForUser(userId.Value);
            if (isExist)
                return BadRequest("You can't add a new goal while having a current goal");

            var goalRequest = goalDto.Convert_To_GoalRequest();
            goalRequest.StartDate = DateTime.Now;
            goalRequest.Status = true;
            goalRequest.EndDate = DateTime.Now;

            var goalEntity = goalRequest.Convert_To_Goal();
            goalEntity.UserID = userId.Value;

            int goalId = await _goalRepository.AddGoal(goalEntity);
            if (goalId == -1) return BadRequest("Failed to add goal");

            var input = await _inputRepository.GetInput(userId.Value);
            if (input == null) return BadRequest("No input found for user");

            var planRequest = input.Convert_To_PlanRequest();
            planRequest.goalAmount = goalRequest.GoalAmount;

            bool planAdded = await _planRepository.AddPlanForCurrentGoal(planRequest, goalEntity);
            if (!planAdded) return BadRequest("Failed to add goal with plan");

            //return CreatedAtAction(nameof(GetCurrentGoalDetails), goalEntity.Convert_To_GoalRequest());
            return Ok();
        }

        [HttpPut("update_goal")]
        public async Task<IActionResult> UpdateGoal(GoalNameAmountRequest goalDto)
        {
            var userId = GetUserIdFromToken();
            if (userId == null) return Unauthorized("User not authorized");

            bool updated = await _goalRepository.UpdateGoalName_Amount(userId.Value, goalDto.GoalName, goalDto.GoalAmount);
            if (!updated) return NotFound("Goal didn't update");

            return NoContent();
        }

		[Authorize]
		[HttpGet("last")]
		public async Task<IActionResult> GetLastGoal()
		{
			var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

			if (userId == null)
				return Unauthorized("User ID not found");

			int parsedUserId = int.Parse(userId);

			var lastGoal = await _context.Goals
				.Where(g => g.UserID == parsedUserId)
				.OrderByDescending(g => g.GoalID)
				.Select(g => new
				{
					g.GoalName,
					g.GoalAmount
				})
				.FirstOrDefaultAsync();

			if (lastGoal == null)
				return NotFound("No goal found");

			return Ok(lastGoal);
		}

	}
}
