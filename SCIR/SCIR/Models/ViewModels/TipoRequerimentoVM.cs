using SCIR.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCIR.Models.ViewModels
{
    public class TipoRequerimentoVM
    {
        public TipoRequerimento TipoRequerimento { get; set; }
        public ConsisteUtils Consistencia { get; set; }

        public TipoRequerimentoVM()
        {
            TipoRequerimento = new TipoRequerimento();
            Consistencia = new ConsisteUtils();
        }
    }
}