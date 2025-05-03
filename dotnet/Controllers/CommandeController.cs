using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using newApp.Models;
using newApp.Services;

namespace newApp.Controllers
{
    public class CommandeController : Controller
    {
        private readonly CommandeService _commandeService;

        public CommandeController (CommandeService commandeService)
        {
            _commandeService = commandeService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string id, string status)
        {
            try
            {
                var fournisseurs = await _commandeService.GetAllFournisseur(); 
                ViewBag.Fournisseur = id;

                var statusOptions = new List<string>
                {
                    "Draft",
                    "On Hold",
                    "To Receive and Bill",
                    "To Bill",
                    "To Receive",
                    "Completed",
                    "Cancelled",
                    "Closed",
                    "Delivered"
                };

                List<BonCommande> commande = new List<BonCommande>();
                if (!string.IsNullOrEmpty(id))
                {
                    commande = await _commandeService.GetBonsCommandeParFournisseur(id,status); 
                }

                ViewBag.BonCommande = commande;  
                ViewBag.StatusOptions = statusOptions;
                return View("Index", fournisseurs); 
            }
            catch (HttpRequestException ex)
            {
                ViewBag.Erreur = $"Erreur HTTP : {ex.Message}";
                return View("Index", new List<Fournisseur>());
            }
            catch (System.Exception ex)
            {
                ViewBag.Erreur = $"Erreur interne : {ex.Message}";
                return View("Index", new List<Fournisseur>());
            }
        }
    }
}
