using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCIR.Datacontract.Grid
{
    public class FluxoStatusGridDC
    {
        public int Id { get; set; }

        public int StatusAtualId { get; set; }
        public string StatusAtualNome { get; set; }

        public int StatusProximoId { get; set; }
        public string StatusProximoNome { get; set; }

        public int TipoRequerimentoId { get; set; }
        public string TipoRequerimentoNome { get; set; }

        public int TotalItensGrid { get; set; }
    }
}