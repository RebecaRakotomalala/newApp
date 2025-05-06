using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using newApp.Models;
using newApp.Services;

namespace newApp.Controllers
{
    public class CommandeStatutController : Controller
    {
        private readonly FournisseurService _fournisseurService;
        private readonly CommandeService _commandeService;

        public CommandeStatutController(FournisseurService fournisseurService, CommandeService commandeService)
        {
            _fournisseurService = fournisseurService;
            _commandeService = commandeService;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string id, string statut)
        {
            try
            {
                // Récupérer la liste des fournisseurs
                var fournisseurs = await _fournisseurService.GetAllFournisseur();
                ViewBag.Fournisseur = id;

                // Définir les statuts disponibles
                var statuts = new List<string> { "Reçu", "Payé" };
                ViewBag.Statuts = statuts;

                List<Statut> commandes = new List<Statut>();

                if (!string.IsNullOrEmpty(id))
                {
                    commandes = await _commandeService.GetPurchaseOrdersWithStatusBySupplier(id, statut);
                    ViewBag.Commandes = commandes;
                    Console.WriteLine("Nombre de commandes : " + commandes.Count);
                }

                ViewBag.SelectedStatut = statut;

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
