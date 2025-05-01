using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using newApp.Models;
using newApp.Services;

namespace newApp.Controllers
{
    public class FournisseurController : Controller
    {
        private readonly FournisseurService _fournisseurService;

        public FournisseurController(FournisseurService fournisseurService)
        {
            _fournisseurService = fournisseurService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string id)
        {
            try
            {
                var fournisseurs = await _fournisseurService.GetAllFournisseur(); 
                ViewBag.Fournisseur = id;

                List<AppelOffre> devis = new List<AppelOffre>();
                if (!string.IsNullOrEmpty(id))
                {
                    devis = await _fournisseurService.GetDevisParFournisseur(id); 
                }

                ViewBag.Devis = devis; 
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
