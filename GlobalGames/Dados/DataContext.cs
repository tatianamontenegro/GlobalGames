using GlobalGames.Dados.Entidades;
using Microsoft.EntityFrameworkCore;

namespace GlobalGames.Dados
{
    public class DataContext : DbContext
    {
        public DbSet<Orcamento> Orcamentos { get; set; }
        public DbSet<Inscricao> Inscricoes { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

    }

}