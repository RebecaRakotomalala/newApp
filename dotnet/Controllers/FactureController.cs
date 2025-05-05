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

        [HttpGet]
        public async Task<ActionResult<List<Facture>>> Calendar()
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

        [HttpPost]
        public async Task<IActionResult> Paiement(string id, string supplier, decimal rate)
        {
            try
            {
                Console.WriteLine("Name" + id);
                Console.WriteLine("Supplier" + supplier);
                Console.WriteLine("Rate" + rate);
                
                var success = await _factureService.CreatePaiementAsync(id, rate, supplier);

                if (!success)
                {
                    Console.WriteLine("Erreur oooo");
                    TempData["Error"] = "Échec du paiement.";
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Erreur : {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ExportFromErpNext(string factureName)
        {
            if (string.IsNullOrEmpty(factureName))
                return BadRequest("Le nom de la facture est requis.");

            var pdfBytes = await _factureService.ExportFacturePdfAsync(factureName);

            if (pdfBytes == null)
                return NotFound("Impossible de générer le PDF de la facture.");

            return File(pdfBytes, "application/pdf", $"{factureName}_copie.pdf");
        }
    }
}
