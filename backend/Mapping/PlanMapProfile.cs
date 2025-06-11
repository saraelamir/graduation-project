using GraduProjj.DTOs.PlanDtos;
using GraduProjj.Models;
using GraduProjj.DTOs.PlanDtos;
using static GraduProjj.Models.Plan;

namespace GraduProjj.Mapping
{
    public static class PlanMapProfile
    {
        public static Plan Convert_To_Plan(this PlanRequest planDto)
        {
            return new Plan
            {
                Groceries = planDto.groceries,
                Healthcare = planDto.healthcare,
                Utilities = planDto.utilities,
                EatingOut = planDto.eatingOut,
                Education = planDto.education,
                Entertainment = planDto.entertainment,
                Transport = planDto.transport,
                OtherMoney = planDto.otherMoney,
                MonthlySavings = planDto.groceries + planDto.healthcare + planDto.utilities + planDto.eatingOut + planDto.education + planDto.entertainment + planDto.transport + planDto.otherMoney,
			};
        }

        public static PlanRequest Convert_To_PlanRequest(this Plan plan)
        {
            return new PlanRequest
            {
                groceries = plan.Groceries,
                healthcare = plan.Healthcare,
                utilities = plan.Utilities,
                eatingOut = plan.EatingOut,
                education = plan.Education,
                entertainment = plan.Entertainment,
                transport = plan.Transport,
                otherMoney = plan.OtherMoney,
                

            };
        }

        public static PlanRequest Convert_To_PlanRequest(this Input input)
        {
            return new PlanRequest
            {
                income = input.income,
                insurance = input.insurance,
                loanPayment = input.loanPayment,
                rent = input.rent,

                education = input.education,
                eatingOut = input.eatingOut,
                entertainment = input.entertainment,
                transport = input.transport,
                groceries = input.groceries,
                healthcare = input.healthcare,
                utilities = input.utilities,
                otherMoney = input.otherMoney,

                age = input.age,
                occupation = input.occupation,
                city_tier = input.city_tier,
                dependents = input.dependents,

                //goalAmount = input.goalAmount,
            };
        }

        public static Plan Convert_To_Plan(this PlanResponse planResponse)
        {
            return new Plan
            {

                Education = planResponse.educationSavings,
                EatingOut = planResponse.eatingOutSavings,
                Transport = planResponse.transportSavings,
                Entertainment = planResponse.entertainmentSavings,
                Groceries = planResponse.groceriesSavings,
                Healthcare = planResponse.healthcareSavings,
                Utilities = planResponse.utilitiesSavings,
                OtherMoney = planResponse.otherMoneySavings,
				MonthlySavings = planResponse.monthlySavings,
				// EndDate = planResponse.endDate,
			};
        }

        public static List<PlanItems> ConvertPlansToPlanItems(ICollection<Plan> plans)
        {
            return plans?.Select(p => new PlanItems
            {
                groceries = p.Groceries,  // Make sure these property names match
                transport = p.Transport,
                eatingOut = p.EatingOut,
                entertainment = p.Entertainment,
                utilities = p.Utilities,
                healthcare = p.Healthcare,
                education = p.Education,
                otherMoney = p.OtherMoney,
				monthlySavings = p.MonthlySavings,
				StartDate = p.StartDate,
                EndDate = p.EndDate,
                status = (p.Status ? "Active" : "Cancelled")
            }).ToList() ?? new List<PlanItems>();
        }
    }
}