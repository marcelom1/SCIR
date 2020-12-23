using SCIR.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCIR.Models.ViewModels
{
    public class UnidadeCurricularVM
    {
        public UnidadeCurricular UnidadeCurricular { get; set; }
        public ConsisteUtils Consistencia { get; set; }

        public UnidadeCurricularVM()
        {
            UnidadeCurricular = new UnidadeCurricular();
            Consistencia = new ConsisteUtils();
        }
    }
}