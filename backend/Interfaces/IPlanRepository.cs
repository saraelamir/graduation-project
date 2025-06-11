using GraduProjj.DTOs.PlanDtos;
using GraduProjj.Models;
using GraduProjj.DTOs.PlanDtos;

namespace GraduProjj.Interfaces
{
    public interface IPlanRepository
    {
        public Task<Plan> GetCurrentPlanForCurrentGoal(int GoalId);
        //public Task<bool> UpdateCurrentPlanStatus(int GoalId, byte Status);
        // public Task<bool> DeleteCurrentPlan(int GoalId);
        // public Task<bool> AddPlanForCurrentGoal(PlanRequest planRequest, int GoalId);
        public Task<bool> AddPlanForCurrentGoal(PlanRequest planRequest, Goal existingGoal);

        public Task<bool> ChangeCurrentPlanStatus_EndDate(Plan CurrentPlan, bool status);

        public decimal CalculateTotalSavings_ForNow(Plan CurrentPlan);

        // public Task<bool> IsGoalExistForUser(int UserID);
        //  public Task<bool> IsCurrentPlanCompleted(int UserId);
        // public Task<PlanRequest>GetAllPlanValues(int GoalId);
        // public Task<decimal> GetMonthlySavings(int UserId);

    }
}