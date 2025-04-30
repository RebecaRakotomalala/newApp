using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using newApp.Models;

namespace newApp.Services
{
    public class ModuleService
    {
        private readonly HttpClient _httpClient;

        public ModuleService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Module>> GetAllModulesAsync()
        {
            string url = "http://erpnext.localhost:8000/api/method/erpnext.init_data.page.data.get_all_modules";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(responseBody);
            var modules = json["message"].ToObject<List<Module>>();

            return modules;
        }
    }
}
