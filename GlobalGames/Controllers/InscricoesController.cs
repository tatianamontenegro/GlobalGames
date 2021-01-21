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
    [Authorize]

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
