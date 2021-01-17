using SCIR.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCIR.Models.ViewModels
{
    public class FormularioValidacaoUCVM
    {
        public FormularioValidacaoUC FormularioValidacaoUC { get; set; }
        public ConsisteUtils Consistencia { get; set; }

        public FormularioValidacaoUCVM()
        {
            FormularioValidacaoUC = new FormularioValidacaoUC();
            Consistencia = new ConsisteUtils();
        }
    }
}