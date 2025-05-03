using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using newApp.Models;
using newApp.Helpers; // Importer le helper

namespace newApp.Services
{
    public class FactureService
    {
        private readonly HttpClient _httpClient;

        public FactureService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Facture>> GetFacturesAsync()
        {
            FrappeAuthHelper.AjouterAuthorization(_httpClient);

            var url = "http://erpnext.localhost:8000/api/resource/Purchase%20Invoice?fields=[\"name\"]";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(jsonString);

            var factureNames = json["data"].ToObject<List<Dictionary<string, string>>>();

            var factures = new List<Facture>();

            foreach (var f in factureNames)
            {
                var name = f["name"];
                var detailUrl = $"http://erpnext.localhost:8000/api/resource/Purchase%20Invoice/{name}";

                var detailResponse = await _httpClient.GetAsync(detailUrl);
                detailResponse.EnsureSuccessStatusCode();

                var detailJson = await detailResponse.Content.ReadAsStringAsync();
                var detailObj = JObject.Parse(detailJson);

                var facture = detailObj["data"].ToObject<Facture>();
                factures.Add(facture);
            }

            return factures;
        }
    }
}
