using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalGames.Dados.Entidades
{
    public class Country : IEntity
    {
        public int Id { get; set; }

        public string Nome { get; set; }
    }
}
