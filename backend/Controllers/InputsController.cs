using GraduProjj.DTOs.GoalDtos;
using GraduProjj.DTOs.PlanDtos;
using GraduProjj.Interfaces;
using GraduProjj.Mapping;
using GraduProjj.Models;
using GraduProjj.Dtos.InputDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using GraduProjj.Data;
using GraduProjj.DTOs;

namespace GraduProjj.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class InputController : ControllerBase
    {
        private readonly IInputRepository _inputRepository;
        private readonly IGoalRepository _goalRepository;
        private readonly IPlanRepository _planRepository;
        private readonly ApplicationDbContext _context;

		public InputController(IInputRepository inputRepository, IGoalRepository goalRepository, IPlanRepository planRepository , ApplicationDbContext context)
        {
            _inputRepository = inputRepository;
            _goalRepository = goalRepository;
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

		//// GET: api/Input
		//[HttpGet]
		//[ProducesResponseType(StatusCodes.Status200OK)]
		//[ProducesResponseType(StatusCodes.Status404NotFound)]
		//public async Task<ActionResult<Input>> GetInput()
		//{
		//    var userId = GetUserIdFromToken();
		//    if (userId == null) return Unauthorized("User not authorized");

		//    try
		//    {
		//        var input = await _inputRepository.GetInput(userId.Value);

		//        if (input == null)
		//        {
		//            return NotFound($"Input for user ID {userId.Value} not found");
		//        }

		//        return Ok(input);
		//    }
		//    catch (Exception ex)
		//    {
		//        return StatusCode(500, $"Internal server error: {ex.Message}");
		//    }
		//}

		// POST: api/Input
		[HttpPost("add_inputs")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> AddInput([FromBody] InputRequest inputDto)
		{
			var userId = GetUserIdFromToken();
			if (userId == null) return Unauthorized("User not authorized");

			try
			{
				// ✅ تحقق إذا كان لدى المستخدم Input موجود بالفعل
				var existingInput = await _inputRepository.GetLatestInputByUserId(userId.Value);
				if (existingInput != null)
				{
					return BadRequest("User already has input data.");
				}

				Input inputEntity = inputDto.Convert_To_Input();
				inputEntity.UserID = userId.Value;

				bool result = await _inputRepository.AddInput(inputEntity);

				if (result)
				{
					return Ok();
				}
				else
				{
					return BadRequest("Failed to add input");
				}
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Internal server error: {ex.Message}");
			}
		}


		// PUT: api/Input
		[HttpPut("update_inputs")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateInput([FromBody] InputRequest inputDto)
        {
            var userId = GetUserIdFromToken();
            if (userId == null) return Unauthorized("User not authorized");

            try
            {
                decimal TotalSavings = 0;

                var inputEntity = inputDto.Convert_To_Input();
                inputEntity.UserID = userId.Value;

                bool result = await _inputRepository.UpdateInput(userId.Value, inputEntity);

                bool Changed = false;
                bool Added = false;

                if (result)
                {
                    var CurrentGoal = await _goalRepository.GetCurrentGoal(userId.Value);
                    var CurrentPlan = await _planRepository.GetCurrentPlanForCurrentGoal(CurrentGoal.GoalID);

                    Changed = await _planRepository.ChangeCurrentPlanStatus_EndDate(CurrentPlan, false); //Cancelled

                    var PlanRequest = inputEntity.Convert_To_PlanRequest();

                    if (Changed)
                    {
                        TotalSavings = _planRepository.CalculateTotalSavings_ForNow(CurrentPlan);

                        PlanRequest.goalAmount = CurrentGoal.GoalAmount - TotalSavings;

                        Added = await _planRepository.AddPlanForCurrentGoal(PlanRequest, CurrentGoal);
                    }

                    return NoContent();
                }
                else
                {
                    return NotFound($"Input for user ID {userId.Value} not found or update failed");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

		// DELETE: api/Input
		//[HttpDelete]
		//[ProducesResponseType(StatusCodes.Status200OK)]
		//public async Task<IActionResult> DeleteInput()
		//{
		//    var userId = GetUserIdFromToken();
		//    if (userId == null) return Unauthorized("User not authorized");

		//    try
		//    {
		//        bool result = await _inputRepository.RemoveInput(userId.Value);

		//        if (result)
		//        {
		//            return NoContent();
		//        }
		//        else
		//        {
		//            return NotFound($"Input for user ID {userId.Value} not found or delete failed");
		//        }
		//    }
		//    catch (Exception ex)
		//    {
		//        return StatusCode(500, $"Internal server error: {ex.Message}");
		//    }
		//}

		[HttpGet("latest-input")]
		[Authorize]
		public async Task<IActionResult> GetLatestInput()
		{
			var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (!int.TryParse(userIdStr, out var userId))
				return Unauthorized();

			var latestInput = await _context.Inputs
				.Where(i => i.UserID == userId)
				.OrderByDescending(i => i.InputID)
				.Select(i => new LatestInputDto
				{
					Age = i.age,
					Dependents = i.dependents,
					Occupation = i.occupation,
					city_tier = i.city_tier,
					Income = i.income,
					Rent = i.rent,
					LoanPayment = i.loanPayment,
					Insurance = i.insurance,
					Groceries = i.groceries,
					Transport = i.transport,
					EatingOut = i.eatingOut,
					Entertainment = i.entertainment,
					Utilities = i.utilities,
					Healthcare = i.healthcare,
					Education = i.education,
					OtherMoney = i.otherMoney,
					//CreatedAt = i.CreatedAt
				})
				.FirstOrDefaultAsync();

			if (latestInput == null)
				return NotFound("No input data found.");

			return Ok(latestInput);
		}




	}
}
