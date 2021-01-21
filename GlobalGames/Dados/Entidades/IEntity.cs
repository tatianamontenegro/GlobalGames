using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalGames.Dados.Entidades
{
    public interface IEntity
    {
        int Id { get; set; }

        string Nome { get; set; }
    }
}
