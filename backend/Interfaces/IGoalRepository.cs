using GraduProjj.Models;

namespace GraduProjj.Interfaces
{
    public interface IGoalRepository
    {
        public Task<Goal> GetCurrentGoal(int UserId);
        public Task<Goal> GetCurrentGoal(string Name);

        public Task<Goal> GetCurrentGoal_with_Current_Plan(int UserId);

        public Task<IEnumerable<Goal>> GetAllCompletedGoals_WithPlans(int UserId);
        public Task<IEnumerable<Goal>> GetAllGoals_WithPlans(int UserId);
        // public Task<bool> UpdateCurrentGoal(int UserId, Goal goal);
        public Task<bool> DeleteCurrentGoalWithPlans(int UserId);
        public Task<int> AddGoal(Goal goal);
        public Task<bool> IsExist_CurrentGoal_ForUser(int UserID);
        public Task<bool> IsCurrentGoalCompleted(int UserId, decimal Amount);
        // public Task<bool> AddGoalWithPlan(Goal goal,Plan plan);
        // public Task<bool>UpdateGoalAmount(int UserId,decimal NewGoalAmount);

        public Task<bool> ChangeCurrentGoalStatus_EndDate(bool status);

        public Task<bool> UpdateGoalName_Amount(int UserID, string goalName, decimal NewGoalAmount);

    }
}