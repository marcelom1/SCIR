using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCIR.Models
{
    public class FluxoStatus
    {
        public int Id { get; set; }
        public StatusRequerimento StatusAtual { get; set; }
        public StatusRequerimento StatusProximo { get; set; }

        public FluxoStatus()
        {
            StatusAtual = new StatusRequerimento();
            StatusProximo = new StatusRequerimento();
        }
    }
}