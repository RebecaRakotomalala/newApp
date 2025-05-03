using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using newApp.Models;
using newApp.Services;

namespace newApp.Controllers
{
    public class FactureController : Controller
    {
        private readonly FactureService _factureService;

        public FactureController (FactureService factureService)
        {
            _factureService = factureService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Facture>>> Index()
        {
            try
            {
                var factures = await _factureService.GetFacturesAsync();
                return View(factures);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur serveur : {ex.Message}");
            }
        }
    }
}
