using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SCIR.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Nome é um campo obrigatório.")]
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public bool Ativo { get; set; }
        public int PapelId { get; set; }
        public Papel Papel { get; set; }

        public Usuario()
        {
            Papel = new Papel();
        }
    }
}