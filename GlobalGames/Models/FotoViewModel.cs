using Microsoft.AspNetCore.Http;
using GlobalGames.Dados.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalGames.Models
{
    public class FotoViewModel : Inscricao
    {
        [Display(Name = "Imagem")]
        public IFormFile ImageFile { get; set; }
    }
}
