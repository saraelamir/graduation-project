using GraduProjj.Data;
using GraduProjj.Interfaces;
using GraduProjj.Mapping;
using GraduProjj.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;

namespace GraduProjj.Repository
{
    //Goal
    // Status == true ? Active : Completed

    //Plan
    // Status == true ? Active : Cancelled

    public class GoalRepository : IGoalRepository
    {
        private readonly ApplicationDbContext _Context;
        private readonly IPlanRepository _planRepository;
        private readonly IInputRepository _inputRepository;

        public GoalRepository(ApplicationDbContext context, IPlanRepository planRepository, IInputRepository inputRepository)
        {
            _Context = context;
            _planRepository = planRepository;
            _inputRepository = inputRepository;
        }

        /*
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure cascading delete for Goal-Plan relationship
            modelBuilder.Entity<Plan>()
                .HasOne(p => p.Goal)
                .WithMany(g => g.Plans)
                .HasForeignKey(p => p.GoalID)
                .OnDelete(DeleteBehavior.Cascade); // Enable cascading delete
        }
        */
        public async Task<int> AddGoal(Goal goal)
        {
            bool res = false;

            try
            {
                await _Context.Goals.AddAsync(goal);

                res = await SaveAsync(_Context);

                if (res) return goal.GoalID;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message);
                // Consider using a proper logging framework instead of Console.WriteLine
                return -1;
            }

            return -1;
        }

        public async Task<bool> DeleteCurrentGoalWithPlans(int UserId)
        {
            bool res = false;

            try
            {
                var goal = await GetCurrentGoal(UserId);
                if (goal == null)
                    return false;

                var Plans = _Context.Plans.Where(P => P.GoalID == goal.GoalID);

                _Context.Plans.RemoveRange(Plans);
                _Context.Goals.Remove(goal);


                res = await SaveAsync(_Context);
            }

            catch (Exception ex)
            {

            }

            return res;
        }

        public async Task<IEnumerable<Goal>> GetAllCompletedGoals_WithPlans(int UserId)
        {
            try
            {
                return await _Context.Goals
                  .Where(g => g.UserID == UserId && g.EndDate < DateTime.Today)
                  .Include(g => g.Plans)
                  .ToListAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return Enumerable.Empty<Goal>();
            }
        }


        public async Task<Goal?> GetCurrentGoal_with_Current_Plan(int userId)
        {
            try
            {
                return await _Context.Goals
                    .Where(g =>
                        g.UserID == userId &&
                        g.Status == true &&
                        g.StartDate <= DateTime.Today &&
                        g.EndDate >= DateTime.Today)
                    .OrderByDescending(g => g.StartDate)
                    .Select(g => new Goal
                    {
                        GoalID = g.GoalID,
                        GoalName = g.GoalName,
                        GoalAmount = g.GoalAmount,
                        StartDate = g.StartDate,
                        EndDate = g.EndDate,
                        Has_Savings = g.Has_Savings,
                        Status = g.Status,
                        UserID = g.UserID,
                        Plans = g.Plans
                            .Where(p => p.Status == true)
                            .Take(1)
                            .ToList()
                    })
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching current goal: " + ex.Message);
                return null;
            }
        }




        public async Task<Goal> GetCurrentGoal(int UserId)
        {
            Goal? Goal = null;

            try
            {
                Goal = await _Context.Goals
                    .FirstOrDefaultAsync(g =>
                        g.UserID == UserId &&
                        g.StartDate <= DateTime.Today &&
                        g.EndDate >= DateTime.Today && g.Status == true);

            }
            catch (Exception ex)
            {

            }

            return Goal;
        }

        public async Task<Goal> GetCurrentGoal(string Name)
        {
            Goal? Goal = null;

            Name = Name.Replace(" ", "").ToLower();

            try
            {
                Goal = await _Context.Goals.FirstOrDefaultAsync((G) => G.GoalName.Replace(" ", "").ToLower() == Name && G.Status == true);
            }
            catch (Exception ex)
            {

            }

            return Goal;
        }

        public async Task<bool> IsCurrentGoalCompleted(int UserId, decimal currentSavedAmount)
        {
            try
            {
                var goal = await GetCurrentGoal(UserId);

                if (goal == null) return false;

                // Use a small tolerance for decimal comparison
                return Math.Abs(goal.GoalAmount - currentSavedAmount) < 0.01m;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> IsExist_CurrentGoal_ForUser(int UserID)
        {
            bool res = false;

            Goal? goal = null;

            try
            {
                goal = await GetCurrentGoal(UserID);

                res = (goal != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message);

                return res;
            }

            return res;
        }


        /*
        public async Task<bool> UpdateGoalName_Amount(int UserID, string goalName, decimal NewGoalAmount)
        {
            bool res = false;
            bool Finalres = false;
            decimal TotalSavings = 0;

            try
            {

                var existingGoal = await GetCurrentGoal(UserID);

                if (existingGoal == null) return false;

                //Update Name

                if (existingGoal.GoalName.Replace(" ", "").ToLower() != goalName.Replace(" ", "").ToLower())
                {
                    existingGoal.GoalName = goalName;
                }

                //Update Amount

                if (existingGoal.GoalAmount != NewGoalAmount)
                {
                    var CurrentPlan = await _planRepository.GetCurrentPlanForCurrentGoal(existingGoal.GoalID);

                    res = await _planRepository.ChangeCurrentPlanStatus_EndDate(CurrentPlan, (byte)Plan.PlanStatus.Cancelled);

                    var PlanRequest = (await _inputRepository.GetInput(UserID)).Convert_To_PlanRequest();


                    //   TotalSavings=(CurrentPlan.EndDate.Day/(decimal)DateTime.DaysInMonth(CurrentPlan.EndDate.Year, CurrentPlan.EndDate.Month))*CurrentPlan.MonthlySavings;

                    //   DateTime BeforeLastMonth = new DateTime(CurrentPlan.EndDate.Year, CurrentPlan.EndDate.Month, CurrentPlan.StartDate.Day).AddMonths(-1);

                    TotalSavings = CalculateTotalSavings(CurrentPlan.StartDate, DateTime.Now, CurrentPlan.MonthlySavings);

                    if (NewGoalAmount > existingGoal.GoalAmount)
                    {
                        PlanRequest.goalAmount = NewGoalAmount - TotalSavings;

                    }

                    else if (NewGoalAmount < PlanRequest.goalAmount)
                    {
                        if (TotalSavings < NewGoalAmount)
                        {
                            PlanRequest.goalAmount = NewGoalAmount - TotalSavings;
                        }

                        else if (NewGoalAmount == TotalSavings)
                        {
                            ChangeCurrentGoalStatus_EndDate_Amount(existingGoal, false, null);
                        }

                        else
                        {
                            existingGoal.Has_Savings = TotalSavings - NewGoalAmount;

                            ChangeCurrentGoalStatus_EndDate_Amount(existingGoal, false, NewGoalAmount);
                        }

                    }

                    //= GetMonthsBetweenDates(CurrentPlan.StartDate, CurrentPlan.EndDate)*CurrentPlan.MonthlySavings;

                    //PlanRequest.goalAmount = NewGoalAmount;


                    if (res)
                    {
                        Finalres = await _planRepository.AddPlanForCurrentGoal(PlanRequest, existingGoal);
                    }

                    else return false;



                    if (Finalres) res = await SaveAsync(_Context);
                }
            }


            // _Context.Update(existingGoal);

            catch (Exception ex)
            {
                res = false;
                //Console.WriteLine("Error : " + ex.Message);
            }

            return res;
        }*/


        public async Task<bool> UpdateGoalName_Amount(int UserID, string goalName, decimal NewGoalAmount)
        {
            bool res = false;
            bool Finalres = false;
            decimal TotalSavings = 0;

            try
            {

                var existingGoal = await GetCurrentGoal(UserID);

                if (existingGoal == null) return false;

                //Update Name

                if (existingGoal.GoalName != null && goalName != null &&
                  !string.Equals(existingGoal.GoalName.Replace(" ", ""), goalName.Replace(" ", ""), StringComparison.OrdinalIgnoreCase))
                {
                    existingGoal.GoalName = goalName;
                }

                //Update Amount

                if (existingGoal.GoalAmount != NewGoalAmount)
                {
                    var CurrentPlan = await _planRepository.GetCurrentPlanForCurrentGoal(existingGoal.GoalID);

                    //  res = await _planRepository.ChangeCurrentPlanStatus_EndDate(CurrentPlan, (byte)Plan.PlanStatus.Cancelled);

                    var PlanRequest = (await _inputRepository.GetInput(UserID)).Convert_To_PlanRequest();


                    //   TotalSavings=(CurrentPlan.EndDate.Day/(decimal)DateTime.DaysInMonth(CurrentPlan.EndDate.Year, CurrentPlan.EndDate.Month))*CurrentPlan.MonthlySavings;

                    //   DateTime BeforeLastMonth = new DateTime(CurrentPlan.EndDate.Year, CurrentPlan.EndDate.Month, CurrentPlan.StartDate.Day).AddMonths(-1);


                    TotalSavings = _planRepository.CalculateTotalSavings_ForNow(CurrentPlan);

                    if (NewGoalAmount > existingGoal.GoalAmount)
                    {
                        res = await _planRepository.ChangeCurrentPlanStatus_EndDate(CurrentPlan, false); // Cancelled

                        PlanRequest.goalAmount = NewGoalAmount - TotalSavings;

                        existingGoal.GoalAmount = NewGoalAmount;

                        await SaveAsync(_Context);


                        if (res)
                        {
                            Finalres = await _planRepository.AddPlanForCurrentGoal(PlanRequest, existingGoal);
                        }
                    }

                    else if (NewGoalAmount < existingGoal.GoalAmount)
                    {
                        if (TotalSavings < NewGoalAmount)
                        {

                            res = await _planRepository.ChangeCurrentPlanStatus_EndDate(CurrentPlan, false); // Cancelled

                            PlanRequest.goalAmount = NewGoalAmount - TotalSavings;

                            existingGoal.GoalAmount = NewGoalAmount;

                            await SaveAsync(_Context);


                            if (res)
                            {
                                Finalres = await _planRepository.AddPlanForCurrentGoal(PlanRequest, existingGoal);
                            }
                        }

                        else if (TotalSavings == NewGoalAmount)
                        {
                            existingGoal.GoalAmount = NewGoalAmount;
                            existingGoal.EndDate = DateTime.Now;
                            existingGoal.Status = false;

                            return await SaveAsync(_Context);

                        }

                        else
                        {
                            existingGoal.GoalAmount = NewGoalAmount;
                            existingGoal.Has_Savings = TotalSavings - NewGoalAmount;
                            existingGoal.EndDate = DateTime.Now;
                            existingGoal.Status = false;

                            return await SaveAsync(_Context);

                        }
                    }



                    //= GetMonthsBetweenDates(CurrentPlan.StartDate, CurrentPlan.EndDate)*CurrentPlan.MonthlySavings;

                    //PlanRequest.goalAmount = NewGoalAmount;

                    /*

                    if (res)
                    {
                        Finalres = await _planRepository.AddPlanForCurrentGoal(PlanRequest, existingGoal);
                    }

                    */


                    if (Finalres) return true;
                    else return false;
                }
            }



            catch (Exception ex)
            {
                res = false;
                //Console.WriteLine("Error : " + ex.Message);
            }

            return res;
        }


        public async Task<IEnumerable<Goal>> GetAllGoals_WithPlans(int UserId)
        {
            try
            {
                return await _Context.Goals.Where(g => g.UserID == UserId).Include(g => g.Plans).ToListAsync();
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return Enumerable.Empty<Goal>();
            }

        }


        async Task<bool> SaveAsync(DbContext context)
        {
            return await context.SaveChangesAsync() > 0;
        }

        public Task<bool> ChangeCurrentGoalStatus_EndDate(bool status)
        {
            throw new NotImplementedException();
        }
    }
}