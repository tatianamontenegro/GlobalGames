using System;
using System.ComponentModel.DataAnnotations;

namespace GlobalGames.Dados.Entidades
{
    public class Orcamento
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public string Email { get; set; }

        public string Mensagem { get; set; }

    }
}
