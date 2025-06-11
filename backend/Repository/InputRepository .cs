using GraduProjj.Data;
using GraduProjj.Interfaces;
using GraduProjj.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace GraduProjj.Repository
{
    public class InputRepository : IInputRepository
    {
        private readonly ApplicationDbContext _Context;
        public InputRepository(ApplicationDbContext context)
        {
            _Context = context;
        }

        public async Task<bool> AddInput(Input input)
        {
            bool res = false;

            try
            {
                /*
                var entry = _Context.Entry(input);
                if (entry.State != EntityState.Detached)
                {
                    entry.State = EntityState.Detached;
                }*/

                await _Context.Inputs.AddAsync(input);

                // val = await _Context.SaveChangesAsync();

                res = await SaveAsync(_Context);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message);
                // Consider using a proper logging framework instead of Console.WriteLine
            }
            //return val>0 ;

            return res;
        }



		public async Task<Input?> GetLatestInputByUserId(int userId)
		{
			try
			{
				return await _Context.Inputs
					.Where(i => i.UserID == userId)
					.OrderByDescending(i => i.InputID) // أو حسب تاريخ الإدخال لو عندك عمود تاريخ
					.FirstOrDefaultAsync();
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error: " + ex.Message);
				return null;
			}
		}


		public async Task<Input> GetInput(int UserID)
        {
            Input? input = null;

            try
            {
                input = await _Context.Inputs.FirstOrDefaultAsync((In) => In.UserID == UserID);
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message);
            }

            return input;
        }

        public async Task<bool> RemoveInput(int UserID)
        {
            bool res = false;

            try
            {
                var input = await _Context.Inputs.FirstOrDefaultAsync((In) => In.UserID == UserID);

                if (input != null)
                {

                    _Context.Inputs.Remove(input);

                    res = await SaveAsync(_Context);

                }
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message);
            }

            return res;
        }

        public async Task<bool> UpdateInput(int UserID, Input input)
        {
            bool res = false;

            try
            {
                //Input existingInput = _Context.Inputs.FirstOrDefault((In) => In.UserID == UserID);
                Input existingInput = await GetInput(UserID);

                if (existingInput != null)
                {
                    existingInput.income = input.income;
                    existingInput.rent = input.rent;
                    existingInput.loanPayment = input.loanPayment;
                    existingInput.insurance = input.insurance;
                    existingInput.groceries = input.groceries;
                    existingInput.transport = input.transport;
                    existingInput.eatingOut = input.eatingOut;
                    existingInput.entertainment = input.entertainment;
                    existingInput.utilities = input.utilities;
                    existingInput.healthcare = input.healthcare;
                    existingInput.education = input.education;
                    existingInput.otherMoney = input.otherMoney;
                    existingInput.age = input.age;
                    existingInput.city_tier = input.city_tier;
                    existingInput.dependents = input.dependents;
                    existingInput.occupation = input.occupation;

                    // _Context.Update(existingInput);

                    res = await SaveAsync(_Context);
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message);
            }

            return res;
        }

        async Task<bool> SaveAsync(DbContext context)
        {
            return await context.SaveChangesAsync() > 0;
        }
    }
}