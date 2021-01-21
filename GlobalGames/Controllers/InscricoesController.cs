using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GlobalGames.Dados;
using GlobalGames.Dados.Entidades;
using GlobalGames.Helpers;
using GlobalGames.Models;
using System.IO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace GlobalGames.Controllers
{
    public class InscricoesController : Controller
    {
        private readonly DataContext _context;
        private readonly IInscricaoRepository inscricaoRepository;
        private readonly IUserHelper userHelper;

        public InscricoesController(DataContext context, IInscricaoRepository inscricaoRepository, IUserHelper userHelper)
        {
            _context = context;
            this.inscricaoRepository = inscricaoRepository;
            this.userHelper = userHelper;
        }

        // GET: Inscricoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Inscricoes.ToListAsync());
        }

        [Authorize]
        // GET: Inscricoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inscricao = await _context.Inscricoes
                .FirstOrDefaultAsync(m => m.Id == id);
            inscricao = await this.inscricaoRepository.GetByIdAsync(id.Value);
            if (inscricao == null)
            {
                return NotFound();
            }

            return View(inscricao);
        }

        [Authorize]
        // GET: Inscricoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Inscricoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                inscricao.User = await this.userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                await this.inscricaoRepository.CreateAsync(inscricao);
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

        private FotoViewModel ToInscricaoViewModel(Inscricao inscricao)
        {
            return new FotoViewModel
            {
                Id = inscricao.Id,
                UrlDaImagem = inscricao.UrlDaImagem,
                Nome = inscricao.Nome,
                Apelido = inscricao.Apelido,
                Morada = inscricao.Morada,
                Localidade = inscricao.Localidade,
                Cc = inscricao.Cc,
                DataNascimento = inscricao.DataNascimento,
                User = inscricao.User
            };
        }

        public IActionResult Register()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterNewUserViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await this.userHelper.GetUserByEmailAsync(model.UserName);
                if (user == null)
                {
                    user = new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.UserName,
                        UserName = model.UserName
                    };

                    var result = await this.userHelper.AddUserAsync(user, model.Password);
                    if (result != IdentityResult.Success)
                    {
                        this.ModelState.AddModelError(string.Empty, "Não foi possível criar este utilizador.");
                        return this.View(model);
                    }

                    var loginViewModel = new LoginViewModel
                    {
                        Password = model.Password,
                        RememberMe = false,
                        Username = model.UserName
                    };

                    var result2 = await this.userHelper.LoginAsync(loginViewModel);

                    if (result2.Succeeded)
                    {
                        return this.RedirectToAction("Index", "Home");
                    }

                    this.ModelState.AddModelError(string.Empty, "O utilizador não fez login.");
                    return this.View(model);
                }

                this.ModelState.AddModelError(string.Empty, "Este utilizador já está registado.");
            }

            return this.View(model);
        }

        [Authorize]
        // GET: Inscricoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inscricao = await _context.Inscricoes.FindAsync(id);
            inscricao = await this.inscricaoRepository.GetByIdAsync(id.Value);
            if (inscricao == null)
            {
                return NotFound();
            }
            return View(inscricao);
        }

        // POST: Inscricoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Apelido,UrlDaImagem,Morada,Localidade,Cc,DataNascimento")] Inscricao inscricao)
        {
            if (id != inscricao.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inscricao);
                    await _context.SaveChangesAsync();
                    await this.inscricaoRepository.UpdateAsync(inscricao);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await this.inscricaoRepository.ExistAsync(inscricao.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(inscricao);
        }

        // GET: Inscricoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inscricao = await _context.Inscricoes
                .FirstOrDefaultAsync(m => m.Id == id);
            inscricao = await this.inscricaoRepository.GetByIdAsync(id.Value);
            if (inscricao == null)
            {
                return NotFound();
            }

            return View(inscricao);
        }

        // POST: Inscricoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inscricao = await _context.Inscricoes.FindAsync(id);
            _context.Inscricoes.Remove(inscricao);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InscricaoExists(int id)
        {
            return _context.Inscricoes.Any(e => e.Id == id);
        }

    }
}
