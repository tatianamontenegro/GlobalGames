using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalGames.Dados.Entidades
{
    public class Inscricao : IEntity
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public string Apelido { get; set; }

        [Display(Name = "Imagem")]
        public string UrlDaImagem { get; set; }

        public string Morada { get; set; }

        public string Localidade { get; set; }

        [Display(Name = "Nº ID Civil")]
        public string Cc { get; set; }

        [Display(Name = "Data de Nascimento")]
        public DateTime DataNascimento { get; set; }

        public User User { get; set; }
    }
}
