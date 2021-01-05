using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCIR.Models
{
    public class HistoricoRequerimento
    {
        public int Id { get; set; }
        public DateTime Modificado { get; set; }
        public string Antes { get; set; }
        public string Depois { get; set; }

        public int RequerimentoId { get; set; }
        public Requerimento Requerimento { get; set; }
    }
}