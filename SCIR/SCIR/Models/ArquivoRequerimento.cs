using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SCIR.Models
{
    public class ArquivoRequerimento
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Caminho { get; set; }
        
        public int RequerimentoId { get; set; }
        public Requerimento Requerimento { get; set; }

        
    }
}