using GraduProjj.DTOs.PlanDtos;
using GraduProjj.Models;
namespace GraduProjj.Interfaces
{
    public interface IAiModelRepository
    {
        // public Task<PlanResponse> GetPlan(PlanRequest planRequest);
        //  public Task<PlanResponse> ReadPlan();
        //public decimal GetTotalExpenses(PlanResponse planResponse);
        public Task<bool> SendPlan(PlanRequest planRequest);
        public Task<PlanResponse> GetPlan();

    }

}
