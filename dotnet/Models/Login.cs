using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace newApp.Models
{
    public class Login
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}