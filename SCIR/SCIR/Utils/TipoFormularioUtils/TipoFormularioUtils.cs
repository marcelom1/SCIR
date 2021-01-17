using SCIR.Controllers;
using SCIR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCIR.Utils.TipoFormularioUtils
{
    public enum FormlarioEnum
    {
        ValidacaoUC = 1
    }

    public class TipoFormularioUtils
    {
        public string Action { get; set; }
        public string Route { get; set; }

        public static TipoFormularioUtils RetornarFormulario(int codigo)
        {
            switch ((FormlarioEnum)codigo)
            {
                case FormlarioEnum.ValidacaoUC:
                    return new FormularioValidacaoUCController().GetActionForm();
                default:
                    throw new ArgumentException("Não foi encontrado o layout do formulário desejado");
            }
        }
    }
}