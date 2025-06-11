using GraduProjj.Data;
using GraduProjj.DTOs.PlanDtos;
using GraduProjj.Interfaces;
using GraduProjj.Mapping;
using GraduProjj.Models;
using GraduProjj.DTOs.PlanDtos;
using GraduProjj.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Numerics;
using System.Text;

namespace GraduProjj.Repository
{


    public class PlanRepository : IPlanRepository
    {


        private readonly ApplicationDbContext _Context;
        private readonly IAiModelRepository _aiModelRepository;
        public PlanRepository(ApplicationDbContext context, IAiModelRepository aiModelRepository)
        {
            _Context = context;
            _aiModelRepository = aiModelRepository;
        }

		/*   public async Task<bool> AddPlanForCurrentGoal(PlanRequest planRequest, int GoalId)
           {

               bool res = false;

               bool request = false;

               PlanResponse Response = new PlanResponse();
               Plan plan = new Plan();

               try
               {

                   request = await _aiModelRepository.SendPlan(planRequest);

                   if (request)
                   {
                       Response = await _aiModelRepository.GetPlan();
                   }


                   plan.Transport = planRequest.transport - Response.transportSavings;
                   plan.Healthcare = planRequest.healthcare - Response.healthcareSavings;
                   plan.Education = planRequest.education - Response.educationSavings;
                   plan.EatingOut = planRequest.eatingOut - Response.educationSavings;
                   plan.Groceries = planRequest.groceries - Response.groceriesSavings;
                   plan.Utilities = planRequest.utilities - Response.utilitiesSavings;
                   plan.OtherMoney = planRequest.otherMoney - Response.otherMoneySavings;
                   plan.Entertainment = planRequest.entertainment - Response.entertainmentSavings;

                   plan.StartDate = DateTime.Now;
                   plan.EndDate = plan.StartDate.AddMonths(Response.endDate);
                   plan.Status = (byte)Plan.PlanStatus.Active;

                   plan.MonthlySavings =

                       Response.entertainmentSavings + Response.eatingOutSavings + Response.educationSavings +
                       Response.groceriesSavings + Response.utilitiesSavings + Response.transportSavings +
                       Response.healthcareSavings + Response.transportSavings;

                   // plan.GoalID = GoalId;

                   var trackedGoal = await _Context.Goals.FindAsync(GoalId);
                   if (trackedGoal != null)
                   {
                       plan.Goal = trackedGoal;
                       trackedGoal.EndDate = plan.EndDate;
                   }

                   await _Context.Plans.AddAsync(plan);
                   res = await SaveAsync(_Context);

                   return res;

               }
               catch (Exception ex)
               {
                   // Log the error
                   Console.WriteLine("Error: " + ex.Message);

                   res = false;
               }

               return res;
           }
        */
		public async Task<bool> AddPlanForCurrentGoal(PlanRequest planRequest, Goal existingGoal)
		{

			bool res = false;

			bool request = false;

			PlanResponse Response = new PlanResponse();
			Plan plan = new Plan();

			try
			{

				request = await _aiModelRepository.SendPlan(planRequest);

				if (request)
				{
					Response = await _aiModelRepository.GetPlan();
				}


				plan.Transport = planRequest.transport - Response.transportSavings;
				plan.Healthcare = planRequest.healthcare - Response.healthcareSavings;
				plan.Education = planRequest.education - Response.educationSavings;
				plan.EatingOut = planRequest.eatingOut - Response.eatingOutSavings;
				plan.Groceries = planRequest.groceries - Response.groceriesSavings;
				plan.Utilities = planRequest.utilities - Response.utilitiesSavings;
				plan.OtherMoney = planRequest.otherMoney - Response.otherMoneySavings;
				plan.Entertainment = planRequest.entertainment - Response.entertainmentSavings;
				//plan.MonthlySavings = Response.monthlySavings;

				plan.StartDate = DateTime.Now;
				plan.EndDate = plan.StartDate.AddMonths(Response.endDate);
				plan.Status = true;

				plan.MonthlySavings =

					Response.entertainmentSavings + Response.eatingOutSavings + Response.educationSavings +
					Response.groceriesSavings + Response.utilitiesSavings + Response.transportSavings +
					Response.healthcareSavings + Response.otherMoneySavings;





				plan.Goal = existingGoal;
				existingGoal.EndDate = plan.EndDate;


				await _Context.Plans.AddAsync(plan);
				res = await SaveAsync(_Context);

				return res;

			}
			catch (Exception ex)
			{

				Console.WriteLine("Error: " + ex.Message);

				res = false;
			}

			return res;
		}

		//private async Task<bool> SaveAsync(DbContext context)
		//{
		//	return await context.SaveChangesAsync() > 0;
		//}





		public async Task<IEnumerable<Plan>> GetAllPlansForGoal(int GoalId)
        {
            List<Plan> Plans = new();

            try
            {
                Plans = await _Context.Plans.Where(P => P.GoalID == GoalId).ToListAsync();
            }

            catch (Exception ex)
            {
                return Plans;
            }

            return Plans;
        }




        public async Task<Plan> GetCurrentPlanForCurrentGoal(int GoalId)
        {
            Plan? plan = null;

            try
            {
                plan = await _Context.Plans.FirstOrDefaultAsync(P => P.GoalID == GoalId && P.EndDate >= DateTime.Now && P.Status == true);

                // plan = await _Context.Plans.FirstOrDefaultAsync(P => P.GoalID == GoalId && P.Status == (byte)Plan.PlanStatus.Active);
            }

            catch (Exception ex)
            {

            }

            return plan;
        }




        public async Task<bool> ChangeCurrentPlanStatus_EndDate(Plan CurrentPlan, bool status)
        {
            bool res = false;

            try
            {

                CurrentPlan.Status = status;
                CurrentPlan.EndDate = DateTime.Now;

                res = await SaveAsync(_Context);

                return res;
            }

            catch (Exception ex)
            {
                res = false;
            }

            return res;
        }





		public decimal CalculateTotalSavings_ForNow(Plan CurrentPlan)
		{
			DateTime endDate = DateTime.Now;
			DateTime startDate = CurrentPlan.StartDate;

			int fullMonths = 0;

			decimal RemaingDaysMoney = 0;

			if (endDate.Month < startDate.Month)
			{
				fullMonths = ((endDate.Year - 1) - (startDate.Year)) * 12;

				if (endDate.Day < startDate.Day)
				{
					fullMonths += (endDate.Month % startDate.Month) - 1;
				}

				else if (endDate.Day == startDate.Day)
				{
					fullMonths += (endDate.Month % startDate.Month);
				}

				else
				{
					RemaingDaysMoney += (decimal)(endDate.Day - startDate.Day) / (DateTime.DaysInMonth(endDate.Year, endDate.Month)) * CurrentPlan.MonthlySavings;
				}
			}

			else if (endDate.Month == startDate.Month)
			{
				if (endDate.Day < startDate.Day)
				{
					fullMonths = ((endDate.Year) - (startDate.Year)) * 12 - 1;
					RemaingDaysMoney += (decimal)endDate.Day / (DateTime.DaysInMonth(endDate.Year, endDate.Month)) * CurrentPlan.MonthlySavings;
				}

				else if (endDate.Day == startDate.Day)
				{
					fullMonths = ((endDate.Year) - (startDate.Year)) * 12;
				}

				else
				{
					RemaingDaysMoney += (decimal)(endDate.Day - startDate.Day) / (DateTime.DaysInMonth(endDate.Year, endDate.Month)) * CurrentPlan.MonthlySavings;
				}
			}

			else
			{
				fullMonths = ((endDate.Year) - (startDate.Year)) * 12;

				if (endDate.Day < startDate.Day)
				{
					fullMonths += ((endDate.Month) - (startDate.Month)) - 1;
					RemaingDaysMoney += (decimal)endDate.Day / (DateTime.DaysInMonth(endDate.Year, endDate.Month)) * CurrentPlan.MonthlySavings;
				}

				else if (endDate.Day == startDate.Day)
				{
					fullMonths += (endDate.Month) - (startDate.Month);
				}

				else
				{
					RemaingDaysMoney += (decimal)(endDate.Day - startDate.Day) / (DateTime.DaysInMonth(endDate.Year, endDate.Month)) * CurrentPlan.MonthlySavings;
				}

			}

			return decimal.Floor(fullMonths * CurrentPlan.MonthlySavings + RemaingDaysMoney);
		}






		private async Task<bool> SaveAsync(DbContext context)
        {
            return await context.SaveChangesAsync() > 0;
        }







    }
}