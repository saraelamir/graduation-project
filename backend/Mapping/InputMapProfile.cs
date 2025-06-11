using GraduProjj.Dtos;
using GraduProjj.Dtos.InputDto;
using GraduProjj.DTOs.PlanDtos;
using GraduProjj.Models;
using Microsoft.AspNetCore.Components.Forms;
namespace GraduProjj.Mapping
{
    public static class InputMapProfile
    {
        //public static FromInputDto 

        /*
       public static InputResponse Convert_To_Dto(this Input input)
       {
            return new InputResponse
            {
                Id= input.InputID,
                income= input.income,
                insurance=input.insurance,
                education=input.education,
                eatingOut=input.eatingOut,
                transport=input.transport,
                entertainment=input.entertainment,
                groceries=input.groceries,
                healthcare=input.healthcare,
                loanPayment=input.loanPayment,
                rent=input.rent,
                utilities=input.utilities,
                otherMoney=input.otherMoney,
            };
       }
        */

        public static Input Convert_To_Input(this InputRequest inputDto)
        {
            return new Input
            {
                income = inputDto.income,
                insurance = inputDto.insurance,
                loanPayment = inputDto.loanPayment,
                rent = inputDto.rent,

                education = inputDto.education,
                eatingOut = inputDto.eatingOut,
                transport = inputDto.transport,
                entertainment = inputDto.entertainment,
                groceries = inputDto.groceries,
                healthcare = inputDto.healthcare,
                utilities = inputDto.utilities,
                otherMoney = inputDto.otherMoney,

                age = inputDto.age,
                occupation = inputDto.occupation,
                city_tier = inputDto.city_tier,
                dependents = inputDto.dependents,

                //goalAmount = inputDto.goalAmount,
                //goalName = inputDto.goalName,
            };
        }

        //public static InputRequestWithNoNameAmount Convert_To_Input_With_NoNameAmount(this Input inputDto)
        //{
        //    return new InputRequestWithNoNameAmount
        //    {
        //        income = inputDto.income,
        //        insurance = inputDto.insurance,
        //        loanPayment = inputDto.loanPayment,
        //        rent = inputDto.rent,

        //        education = inputDto.education,
        //        eatingOut = inputDto.eatingOut,
        //        transport = inputDto.transport,
        //        entertainment = inputDto.entertainment,
        //        groceries = inputDto.groceries,
        //        healthcare = inputDto.healthcare,
        //        utilities = inputDto.utilities,
        //        otherMoney = inputDto.otherMoney,

        //        age = inputDto.age,
        //        occupation = inputDto.occupation,
        //        city_tier = inputDto.city_tier,
        //        dependents = inputDto.dependents,
        //    };
        //}


        /*
            public static Input Convert_To_Input(this PlanRequest planDto)
            {
            return new Input
            {
                  // Income = planDto.Income,
                   //Insurance = planDto.Insurance,
                   // LoanPayment = planDto.LoanPayment,
                   // Rent = planDto.Rent,

                    Education = planDto.education,
                    EatingOut = planDto.eatingOut,
                    Transport = planDto.transport,
                    Entertainment = planDto.entertainment,
                    Groceries = planDto.groceries,
                    Healthcare = planDto.healthcare,
                    Utilities = planDto.utilities,
                    OtherMoney = planDto.otherMoney,
                };
            }   
        */
    }
}