using SCIR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCIR.Datacontract.Grid
{
    public class RequerimentoGridDC : Requerimento
    {
        public string TipoRequerimentoNome { get; set; }
        public string StatusRequerimentoNome { get; set; }
        public string AberturaToString { get; set; }
        public int TotalItensGrid { get; set; }
    }
}