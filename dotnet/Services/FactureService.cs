using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using newApp.Models;
using newApp.Helpers; 
using Newtonsoft.Json;

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

        public async Task<string> GetSupplierAccountForInvoice(string invoiceName)
        {
            FrappeAuthHelper.AjouterAuthorization(_httpClient);

            try
            {
                var url = $"http://erpnext.localhost:8000/api/resource/Purchase%20Invoice/{invoiceName}";
                var response = await _httpClient.GetAsync(url);
                var jsonString = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    throw new Exception($"Erreur lors de la récupération de la facture {invoiceName} : {jsonString}");

                dynamic invoiceData = JsonConvert.DeserializeObject(jsonString);
                return invoiceData.data.credit_to;
            }
            catch (Exception ex)
            {
                // Log ou autre traitement d'erreur possible
                throw new Exception("Erreur dans GetSupplierAccountForInvoice : " + ex.Message, ex);
            }
        }

        public async Task<bool> CreatePaiementAsync(string invoiceName, decimal montant, string supplier)
        {
            FrappeAuthHelper.AjouterAuthorization(_httpClient);

            string partyAccount = await GetSupplierAccountForInvoice(invoiceName);

            var paymentData = new
            {
                payment_type = "Pay",
                posting_date = DateTime.Now.ToString("yyyy-MM-dd"),
                party_type = "Supplier",
                party = supplier,
                paid_from = "533-Caisse succursale (ou usine) B - IC",
                party_account = partyAccount,
                paid_amount = montant,
                received_amount = montant,
                mode_of_payment = "Cash", 
                source_exchange_rate = 1,
                references = new[]
                {
                    new {
                        reference_doctype = "Purchase Invoice",
                        reference_name = invoiceName,
                        total_amount = montant,
                        outstanding_amount = montant,
                        allocated_amount = montant
                    }
                }
            };

            var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(paymentData), System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("http://erpnext.localhost:8000/api/resource/Payment Entry", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            
            if (response.IsSuccessStatusCode)
            {
                // Récupérer le nom du Payment Entry créé
                var responseObj = JObject.Parse(responseContent);
                var paymentEntryName = responseObj["data"]?["name"]?.ToString();

                if (!string.IsNullOrEmpty(paymentEntryName))
                {
                    // Soumettre le document
                    var submitUrl = $"http://erpnext.localhost:8000/api/resource/Payment%20Entry/{paymentEntryName}?run_method=submit";
                    var submitResponse = await _httpClient.PostAsync(submitUrl, null);
                    var submitContent = await submitResponse.Content.ReadAsStringAsync();

                    if (!submitResponse.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Erreur lors de la soumission :");
                        Console.WriteLine(submitContent);
                        return false;
                    }
                }

                return true;
            }

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Erreur API ERPNext :");
                Console.WriteLine(responseContent); 
                return false;
            }

            return response.IsSuccessStatusCode;
        }

        public async Task<byte[]> ExportFacturePdfAsync(string factureName)
        {
            FrappeAuthHelper.AjouterAuthorization(_httpClient);

            var pdfUrl = $"http://erpnext.localhost:8000/api/method/frappe.utils.print_format.download_pdf?doctype=Purchase Invoice&name={factureName}&format=Standard&no_letterhead=0";

            var response = await _httpClient.GetAsync(pdfUrl);

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Erreur lors de la génération du PDF : " + content);
                return null;
            }

            return await response.Content.ReadAsByteArrayAsync();
        }
    }
}
