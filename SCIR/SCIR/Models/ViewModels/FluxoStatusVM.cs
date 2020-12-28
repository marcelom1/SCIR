using SCIR.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCIR.Models.ViewModels
{
    public class FluxoStatusVM
    {
        public FluxoStatus FluxoStatus { get; set; }
        public ConsisteUtils Consistencia { get; set; }

        public FluxoStatusVM()
        {
            FluxoStatus = new FluxoStatus();
            Consistencia = new ConsisteUtils();
        }
    }
}