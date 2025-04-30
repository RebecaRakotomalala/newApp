using Microsoft.AspNetCore.Mvc;
using newApp.Models;
using newApp.Services;

namespace newApp.Controllers
{
    public class AccountController : Controller
    {

        private readonly LoginService _loginService;

        // Injection de la dépendance LoginService
        public AccountController(LoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            try
            {
                var result = await _loginService.LoginAsync(username, password);
                Console.WriteLine(result.ToString());

                if (result["message"] != null && result["message"].ToString().Contains("Logged In"))
                {
                    return RedirectToAction("Sidebar", "Home");
                }
                else
                {
                    ViewBag.Error = "Nom d'utilisateur ou mot de passe incorrect.";
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Erreur de connexion : {ex.Message}";
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
