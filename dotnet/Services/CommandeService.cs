using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using newApp.Models;
using newApp.Helpers; // Importer le helper

namespace newApp.Services
{
    public class CommandeService
    {
        private readonly HttpClient _httpClient;

        public CommandeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Fournisseur>> GetAllFournisseur()
        {
            FrappeAuthHelper.AjouterAuthorization(_httpClient); 

            string url = "http://erpnext.localhost:8000/api/resource/Supplier";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(responseBody);

            var data = json["data"].ToObject<List<Fournisseur>>();

            return data;
        }

        public async Task<List<BonCommande>> GetBonsCommandeParFournisseur(string fournisseur, string status)
        {
            FrappeAuthHelper.AjouterAuthorization(_httpClient);

            string url = $"http://erpnext.localhost:8000/api/method/erpnext.buying.fournisseur.get_purchase_orders_by_supplier_status?supplier_name={fournisseur}&status={status}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(responseBody);

            var data = json["message"].ToObject<List<BonCommande>>();

            return data;
        }
    }
}
