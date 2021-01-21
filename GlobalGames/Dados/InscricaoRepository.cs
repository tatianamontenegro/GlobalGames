using GlobalGames.Dados.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalGames.Dados
{
    public class InscricaoRepository : GenericRepository<Inscricao>, IInscricaoRepository
    {
        public InscricaoRepository(DataContext context) : base(context)
        {

        }
    }
}
