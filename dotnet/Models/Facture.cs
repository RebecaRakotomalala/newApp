using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace newApp.Models
{
    public class Facture
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("posting_date")]
        public DateTime PostingDate { get; set; }

        [JsonProperty("due_date")]
        public DateTime DueDate { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("supplier")]
        public string Supplier { get; set; }

        [JsonProperty("items")]
        public List<FactureItem> Items { get; set; }
    }

    public class FactureItem
    {
        [JsonProperty("item_code")]
        public string ItemCode { get; set; }

        [JsonProperty("item_name")]
        public string ItemName { get; set; }

        [JsonProperty("qty")]
        public decimal Quantity { get; set; }

        [JsonProperty("rate")]
        public decimal Rate { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }
    }

}