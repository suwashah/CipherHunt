using CipherHunt.Models;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Repository.HelperFunction;

namespace CipherHunt.API
{
    public class APIService
    {
        private readonly HttpClient _httpClient;
        private IUtilityHelper _func;
        public APIService(IUtilityHelper func)
        {
            _httpClient = new HttpClient();
            _func = func;
        }

        public async Task<dynamic> PostDataAsync(RunDockerModel model, bool start=true)
        {
            string runType = "start";
            if (!start)
                runType = "stop";

            var url = _func.GetAppsettingValue("DockerInstance_URL")+ runType;

            // Serialize your model to JSON using Newtonsoft.Json
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Set Basic Authentication header
            var username = _func.GetAppsettingValue("DockerInstance_Username");
            var password = _func.GetAppsettingValue("DockerInstance_Password"); 
            var byteArray = Encoding.ASCII.GetBytes($"{username}:{password}");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            // Send POST request
            var response = await _httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                // Read response content as string
                var responseContent = await response.Content.ReadAsStringAsync();

                // Deserialize the response JSON to a dynamic object
                return JsonConvert.DeserializeObject<dynamic>(responseContent);
            }

            // In case of error, return a dynamic object indicating the failure
            return new { success = false, message = "Error sending data.", statusCode = response.StatusCode };
        }
    }
}