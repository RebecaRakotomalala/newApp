using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using newApp.Models;
using newApp.Services;

namespace newApp.Controllers
{
    public class ModuleController : Controller
    {
        private readonly ModuleService _moduleService;

        public ModuleController(ModuleService moduleService)
        {
            _moduleService = moduleService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var modules = await _moduleService.GetAllModulesAsync();
                return View(modules);
            }
            catch (HttpRequestException e)
            {
                ViewBag.Error = e.Message;
                return View(new List<Module>());
            }
        }
    }
}
