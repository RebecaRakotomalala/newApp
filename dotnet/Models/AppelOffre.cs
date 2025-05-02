using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace newApp.Models
{
    public class AppelOffre
    {
        [JsonProperty("numero_devis")]
        public string NumeroDevis { get; set; }

        [JsonProperty("supplier")]
        public string Supplier { get; set; }

        [JsonProperty("date_transaction")]
        public DateTime DateTransaction { get; set; }

        [JsonProperty("date_livraison_prevue")]
        public DateTime DateLivraisonPrevue { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }

}