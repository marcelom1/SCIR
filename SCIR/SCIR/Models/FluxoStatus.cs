using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCIR.Models
{
    public class FluxoStatus
    {
        public int StatusAtualId { get; set; }
        public StatusRequerimento StatusAtual { get; set; }

        public int StatusProximoId { get; set; }
        public StatusRequerimento StatusProximo { get; set; }

        public int TipoRequerimentoId { get; set; }
        public TipoRequerimento TipoRequerimento { get; set; }

        public FluxoStatus()
        {
            StatusAtual = new StatusRequerimento();
            StatusProximo = new StatusRequerimento();
            TipoRequerimento = new TipoRequerimento();
        }

        public override string ToString()
        {
            return "StatusAtual: " + StatusAtualId + " ProximoStatus: " + StatusProximoId + " TipoRequerimento: " + TipoRequerimentoId;
        }
    }
}