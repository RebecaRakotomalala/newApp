using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace newApp.Models
{
    public class AppelOffre
    {
        public string NumeroDevis { get; set; }
        public string Supplier { get; set; }
        public DateTime DateTransaction { get; set; }
        public DateTime DateLivraisonPrevue { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
    }
}