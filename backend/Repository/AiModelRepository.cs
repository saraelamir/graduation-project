using GraduProjj.DTOs.PlanDtos;
using GraduProjj.Interfaces;
using GraduProjj.Models;
using System.Text.Json;
using System.Text;
using GraduProjj.DTOs.PlanDtos;
using GraduProjj.Interfaces;

namespace GraduProjj.Repository
{
    public class AiModelRepository : IAiModelRepository
    {
        private readonly HttpClient _http;

        public AiModelRepository(HttpClient http)
        {
            _http = http;
        }


        public async Task<bool> SendPlan(PlanRequest planRequest)
        {
            var response = await _http.PostAsJsonAsync("receive-data", planRequest);

            bool res = false;

            if (!response.IsSuccessStatusCode)
            {
                res = false;
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to get plans from AI API: {errorContent}");
            }

            else res = true;

            return res;
            /*
                var result = await response.Content.ReadFromJsonAsync<PlanResponse>();
            return result;*/
        }

        public async Task<PlanResponse> GetPlan()
        {
            var response = await _http.GetFromJsonAsync<PlanResponse>("send-data");

            bool res = false;

            return response;
        }
        /*
        public async Task<PlanResponse> ReadPlan()
        {
            var response = await _http.GetFromJsonAsync("/plan", planRequest);

            bool res = false;
            
            if (!response.IsSuccessStatusCode)
            {
                res = false;
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to get plans from AI API: {errorContent}");
            }

            else res = true;

            return res;
            /*
                var result = await response.Content.ReadFromJsonAsync<PlanResponse>();
            return result;
        }*/
    }
}