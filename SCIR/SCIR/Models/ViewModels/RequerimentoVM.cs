using SCIR.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCIR.Models.ViewModels
{
    public class RequerimentoVM : Requerimento
    {

        public ConsisteUtils Consistencia { get; set; }

        public RequerimentoVM()
        {
            Consistencia = new ConsisteUtils();
        }

        public RequerimentoVM(Requerimento requerimento) : base(requerimento)
        {
            Consistencia = new ConsisteUtils();
        }
    }
}