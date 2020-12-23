using SCIR.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCIR.Models.ViewModels
{
    public class StatusRequerimentoVM
    {
        public StatusRequerimento StatusRequerimento { get; set; }
        public ConsisteUtils Consistencia { get; set; }

        public StatusRequerimentoVM()
        {
            StatusRequerimento = new StatusRequerimento();
            Consistencia = new ConsisteUtils();
        }
    }
}