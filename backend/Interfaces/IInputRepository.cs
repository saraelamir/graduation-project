using GraduProjj.Models;
using GraduProjj.DTOs.PlanDtos;

namespace GraduProjj.Interfaces
{
    public interface IInputRepository
    {
        public Task<bool> AddInput(Input input);

        public Task<bool> RemoveInput(int UserID);

        public Task<Input> GetInput(int UserID);

        public Task<bool> UpdateInput(int UserID, Input input);
		Task<Input?> GetLatestInputByUserId(int userId);




		// public Task<decimal> GetTotalExpenses(int UserID);
	}
}