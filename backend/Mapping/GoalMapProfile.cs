using GraduProjj.Dtos;
using GraduProjj.Dtos.InputDto;
using GraduProjj.Models;
using GraduProjj.DTOs.GoalDtos;

namespace GraduProjj.Mapping
{
    public static class GoalMapProfile
    {

        public static GoalResponse Convert_To_Dto(this Goal Goal)
        {
            return new GoalResponse
            {
                GoalId = Goal.GoalID,
                GoalName = Goal.GoalName,
                GoalAmount = Goal.GoalAmount,
                EndDate = Goal.EndDate,
                StartDate = Goal.StartDate,
                Status = Goal.Status,
            };
        }
        public static GoalRequest Convert_To_GoalRequest(this Goal Goal)
        {
            return new GoalRequest
            {
                GoalName = Goal.GoalName,
                GoalAmount = Goal.GoalAmount,
                EndDate = Goal.EndDate,
                StartDate = Goal.StartDate,
                Status = Goal.Status,
            };
        }

        public static GoalRequestWithPlans Convert_To_GoalRequestWithPlans(this Goal goal)
        {
            return new GoalRequestWithPlans
            {
                GoalAmount = goal.GoalAmount,
                GoalName = goal.GoalName,
                EndDate = goal.EndDate,
                StartDate = goal.StartDate,
                status = goal.Status ? "Active" : "Completed",
                Has_Savings = goal.Has_Savings.HasValue ? goal.Has_Savings : 0,
                Plans = PlanMapProfile.ConvertPlansToPlanItems(goal.Plans),
            };

        }

        public static IEnumerable<GoalRequestWithPlans> Convert_To_GoalRequestWithPlans(this IEnumerable<Goal> Goals)
        {
            return Goals.Select(Convert_To_GoalRequestWithPlans);
        }

        public static IEnumerable<GoalResponse> Convert_To_Dto(this IEnumerable<Goal> Goals)
        {
            // For null     return Goals?.Select(Convert_To_Dto) ?? Enumerable.Empty<GoalResponse>();
            return Goals.Select(Convert_To_Dto);
        }

        public static Goal Convert_To_Goal(this GoalRequest GoalDto)
        {
            return new Goal
            {
                GoalName = GoalDto.GoalName,
                GoalAmount = GoalDto.GoalAmount,
                StartDate = GoalDto.StartDate,
                EndDate = GoalDto.EndDate,
                Status = GoalDto.Status,
            };
        }
        public static GoalRequest Convert_To_GoalRequest(this GoalNameAmountRequest GoalDto)
        {
            return new GoalRequest
            {
                GoalName = GoalDto.GoalName,
                GoalAmount = GoalDto.GoalAmount,
            };
        }
    }
}