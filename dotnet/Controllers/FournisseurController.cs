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
                List<BonCommande> commande = new List<BonCommande>();
                if (!string.IsNullOrEmpty(id))
                {
                    devis = await _fournisseurService.GetDevisParFournisseur(id); 
                    commande = await _fournisseurService.GetBonsCommandeParFournisseur(id); 
                }

                ViewBag.Devis = devis;   
                ViewBag.BonCommande = commande;   
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

        [HttpGet]
        public async Task<IActionResult> DevisFournisseur(string rfqName, string fournisseur)
        {
            if (string.IsNullOrEmpty(rfqName) || string.IsNullOrEmpty(fournisseur))
            {
                return BadRequest("Paramètres manquants.");
            }

            try
            {
                var devis = await _fournisseurService.GetDevisParAppelOffreEtFournisseur(rfqName, fournisseur);
                return View("DevisFournisseur", devis); // Assurez-vous que la vue "DevisFournisseur.cshtml" existe
            }
            catch (HttpRequestException ex)
            {
                ViewBag.Erreur = $"Erreur HTTP : {ex.Message}";
                return View("DevisFournisseur", new List<Devis>());
            }
            catch (System.Exception ex)
            {
                ViewBag.Erreur = $"Erreur interne : {ex.Message}";
                return View("DevisFournisseur", new List<Devis>());
            }
        }   

        [HttpGet]
        public async Task<IActionResult> DevisDetails(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest(new { message = "Le nom du devis est requis." });
            }

            try
            {
                var details = await _fournisseurService.GetDetailsDevis(name);
                if (details == null || details.Count == 0)
                {
                    return NotFound(new { message = "Le devis n'a pas été trouvé." });
                }

                return Ok(details); 
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, new { message = $"Erreur HTTP : {ex.Message}" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Erreur interne : {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateDevis(string idNumDevis, string rate, string id)
        {
            try
            {
                List<Devis> devis = new List<Devis>();
                var cancelResponse = await _fournisseurService.UpdateDevis(idNumDevis, rate, id);
                devis = await _fournisseurService.GetDevisDetails(idNumDevis);

                if (cancelResponse == null)
                {
                    ViewBag.Erreur = $"erreur de la mise à jours";
                    return View("DevisFournisseur", devis);
                }
                ViewBag.Devis = devis; 
                return View("DevisFournisseur", devis); 
            }
            catch (Exception ex)
            {
                ViewBag.Erreur = $"Erreur interne : {ex.Message}";
                return View("DevisFournisseur");
            }
        }
    }
}
