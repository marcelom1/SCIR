using SCIR.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCIR.Models
{
    public class Auditoria
    {
        public int Id { get; set; }
        public DateTime DataModificacao { get; set; }
        public string Campo { get; set; }
        public string Antes { get; set; }
        public string Depois { get; set; }

        public int RequerimentoId { get; set; }
        public Requerimento Requerimento { get; set; }

        public Auditoria()
        {
            Requerimento = new RequerimentoVM();
        }
        
    }
}