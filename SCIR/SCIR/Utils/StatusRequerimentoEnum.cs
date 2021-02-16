using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCIR.Utils
{
    public class StatusRequerimentoEnum
    {
        public enum StatusPadrao
        {
            Deferido = 1,
            Indeferido = 2,
            Protocolado = 3,
            AguardandoEsclarecimento = 4,
            Cancelado = 5
        }


    }
}