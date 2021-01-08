using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GlobalGames.Models;
using Microsoft.EntityFrameworkCore;
using GlobalGames.Dados;
using GlobalGames.Dados.Entidades;

namespace GlobalGames.Controllers
{
    public class HomeController : Controller
    {
        private readonly DataContext _context;

        public HomeController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Sobre()
        {
            return View();
        }

        public IActionResult Servicos()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Servicos([Bind("Id,Nome,Email,Mensagem")] Orcamento orcamento)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orcamento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(orcamento);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
