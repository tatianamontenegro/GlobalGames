using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GlobalGames.Models;
using Microsoft.EntityFrameworkCore;
using GlobalGames.Dados;
using GlobalGames.Dados.Entidades;
using GlobalGames.Helpers;
using System.IO;

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

        public IActionResult Inscricoes()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Inscricoes([Bind("Id,Nome,ImageFile,Apelido,Morada,Localidade,Cc,DataNascimento")] FotoViewModel view)
        {
            if (ModelState.IsValid)
            {
                var path = string.Empty;
                if (view.ImageFile != null && view.ImageFile.Length > 0)
                {
                    var guid = Guid.NewGuid().ToString();
                    var file = $"{guid}.jpg";

                    path = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot\\images\\Fotos",
                        file);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await view.ImageFile.CopyToAsync(stream);
                    }

                    path = $"~/images/Fotos/{file}";
                }

                var inscricao = this.ToFoto(view, path);
                _context.Add(inscricao);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(view);
        }

        private Inscricao ToFoto(FotoViewModel view, string path)
        {
            return new Inscricao
            {
                Id = view.Id,
                UrlDaImagem = path,
                Nome = view.Nome,
                Apelido = view.Apelido,
                Morada = view.Morada,
                Localidade = view.Localidade,
                Cc = view.Cc,
                DataNascimento = view.DataNascimento,
                User = view.User
            };
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
