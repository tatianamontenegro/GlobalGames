using GlobalGames.Dados.Entidades;
using GlobalGames.Helpers;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalGames.Dados
{
    public class SeedDb
    {
        private readonly DataContext context;
        private readonly IUserHelper userHelper;
        private Random random;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            this.context = context;
            this.userHelper = userHelper;
            this.random = new Random();
        }

        public async Task SeedAsync()
        {
            await this.context.Database.EnsureCreatedAsync();

            var user = await this.userHelper.GetUserByEmailAsync("tatiana.caldeira.tm@gmail.com");
            if (user == null)
            {
                user = new User
                {
                    FirstName = "Tatiana",
                    LastName = "Montenegro",
                    Email = "tatiana.caldeira.tm@gmail.com",
                    UserName = "tatiana.caldeira.tm@gmail.com",
                    PhoneNumber = "966038082"
                };

                var result = await this.userHelper.AddUserAsync(user, "123456");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Não foi possível criar o utilizador no Seeder");
                }
            }

            if (!this.context.Inscricoes.Any())
            {
                this.AddInscricao("Afonso Tobias", user);
                this.AddInscricao("Carlota Charrua", user);
                this.AddInscricao("Ana Pipoca", user);
                this.AddInscricao("Jonas Pistolas", user);
                this.AddInscricao("Cláudio Varela", user);
                await this.context.SaveChangesAsync();
            }
        }

        private void AddInscricao(string name, User user)
        {
            this.context.Inscricoes.Add(new Inscricao
            {
                Nome = name,
                Cc = this.random.Next(0, 9).ToString(),
                User = user
            });
        }

    }
}
