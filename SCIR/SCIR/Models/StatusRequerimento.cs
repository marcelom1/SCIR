using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SCIR.Models
{
    public class StatusRequerimento
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Nome é um campo obrigatório.")]
        public string Nome { get; set; }
        public bool Ativo { get; set; }
        public bool Cancelamento { get; set; }
        public int CodigoInterno { get; set; }
    }
}