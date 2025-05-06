using Newtonsoft.Json;

namespace newApp.Models
{
    public class Statut
    {
        [JsonProperty("purchase_order")]
        public string numero_commande { get; set; }

        [JsonProperty("purchase_invoice")]
        public string numero_facture { get; set; }

        [JsonProperty("supplier")]
        public string supplier { get; set; }

        [JsonProperty("transaction_date")]
        public string date_transaction { get; set; }

        [JsonProperty("date_livraison_prevue")]
        public string date_livraison_prevue { get; set; }

        [JsonProperty("grand_total")]
        public decimal grand_total { get; set; }

        [JsonProperty("per_received")]
        public decimal per_received { get; set; }

        [JsonProperty("status_po")]
        public string status_po { get; set; }

        [JsonProperty("status_pi")]
        public string status_pi { get; set; }

        [JsonProperty("outstanding_amount")]
        public decimal outstanding_amount { get; set; }

        [JsonProperty("item_code")]
        public string item_code { get; set; }

        [JsonProperty("item_name")]
        public string item_name { get; set; }

        [JsonProperty("qty")]
        public decimal qty { get; set; }

        [JsonProperty("rate")]
        public decimal rate { get; set; }

        [JsonProperty("amount")]
        public decimal amount { get; set; }

        [JsonProperty("custom_status")]
        public string custom_status { get; set; }

        [JsonProperty("is_paid")]
        public bool is_paid { get; set; }

        [JsonProperty("is_received")]
        public bool is_received { get; set; }
    }
}