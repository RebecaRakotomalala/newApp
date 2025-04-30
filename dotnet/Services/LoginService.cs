using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using newApp.Models;
using System.Text;

namespace newApp.Services
{
    public class LoginService
    {
        private readonly HttpClient _httpClient;

        public LoginService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<JObject> LoginAsync(string username, string password)
        {
            var loginData = new Dictionary<string, string>
            {
                { "usr", username },
                { "pwd", password }
            };

            var content = new FormUrlEncodedContent(loginData);
            var response = await _httpClient.PostAsync("http://erpnext.localhost:8000/api/method/login", content);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(responseBody);

            return json;
        }
    }
}
