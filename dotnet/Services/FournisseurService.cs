using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using newApp.Models;
using newApp.Helpers; // Importer le helper

namespace newApp.Services
{
    public class FournisseurService
    {
        private readonly HttpClient _httpClient;

        public FournisseurService(HttpClient httpClient)
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

        public async Task<List<AppelOffre>> GetDevisParFournisseur(string fournisseur)
        {
            FrappeAuthHelper.AjouterAuthorization(_httpClient);

            string url = $"http://erpnext.localhost:8000/api/method/erpnext.buying.fournisseur.get_request_for_quotation_by_supplier?supplier_name={fournisseur}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(responseBody);
            var data = json["message"].ToObject<List<AppelOffre>>();

            return data;
        }

        public async Task<List<Devis>> GetDevisParAppelOffreEtFournisseur(string rfqName, string fournisseur)
        {
            FrappeAuthHelper.AjouterAuthorization(_httpClient);

            string url = $"http://erpnext.localhost:8000/api/method/erpnext.buying.fournisseur.get_supplier_quotations_by_rfq_and_supplier?rfq_name={rfqName}&supplier_name={fournisseur}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(responseBody);

            var data = json["message"].ToObject<List<Devis>>();

            return data;
        }

        public async Task<List<Devis>> GetDetailsDevis(string name)
        {
            FrappeAuthHelper.AjouterAuthorization(_httpClient);

            string url = $"http://erpnext.localhost:8000/api/method/erpnext.buying.fournisseur.get_supplier_quotation_details?name={name}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(responseBody);

            var data = json["message"].ToObject<List<Devis>>();
            return data;
        }

        public async Task<List<BonCommande>> GetBonsCommandeParFournisseur(string fournisseur)
        {
            FrappeAuthHelper.AjouterAuthorization(_httpClient);

            string url = $"http://erpnext.localhost:8000/api/method/erpnext.buying.fournisseur.get_purchase_orders_by_supplier?supplier_name={fournisseur}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(responseBody);

            var data = json["message"].ToObject<List<BonCommande>>();

            return data;
        }

        public async Task<Devis> UpdateDevis(string numero_devis, string nouveauRate, string item_internal_name)
        {
            FrappeAuthHelper.AjouterAuthorization(_httpClient);

            var updateUrl = $"http://erpnext.localhost:8000/api/resource/Supplier Quotation Item/{item_internal_name}";
            var payload = new
            {
                rate = Convert.ToDecimal(nouveauRate)
            };
            var updateResponse = await _httpClient.PutAsJsonAsync(updateUrl, payload);
            updateResponse.EnsureSuccessStatusCode();

            var jsonString = await updateResponse.Content.ReadAsStringAsync();
            var json = JObject.Parse(jsonString);
            var devis = json["data"].ToObject<Devis>();

            return devis;
        }

        public async Task<List<Devis>> GetDevisDetails(string numero_devis)
        {
            FrappeAuthHelper.AjouterAuthorization(_httpClient);
            string url = $"http://erpnext.localhost:8000/api/method/erpnext.buying.fournisseur.get_supplier_quotation_details?name={numero_devis}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(responseBody);
            var data = json["message"].ToObject<List<Devis>>();

            return data;
        }
    }
}
