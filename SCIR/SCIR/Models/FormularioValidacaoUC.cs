using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SCIR.Models
{
    public class FormularioValidacaoUC : Requerimento
    {
        public string Nome { get; set; }
        public string Motivo { get; set; }

        public int UnidadeCurricularId { get; set; }
        public UnidadeCurricular UnidadeCurricular { get; set; }

        public int TipoValidacaoCurricularId { get; set; }
        public TipoValidacaoCurricular TipoValidacaoCurricular { get; set; }

       public FormularioValidacaoUC()
        {
            UnidadeCurricular = new UnidadeCurricular();
            TipoValidacaoCurricular = new TipoValidacaoCurricular();
        }

        public FormularioValidacaoUC(FormularioValidacaoUC entidade) : base(entidade)
        {
            UnidadeCurricularId = entidade.UnidadeCurricularId;
            UnidadeCurricular = entidade.UnidadeCurricular;
            Motivo = entidade.Motivo;
            TipoValidacaoCurricularId = entidade.TipoValidacaoCurricularId;
            TipoValidacaoCurricular = entidade.TipoValidacaoCurricular;
            Nome = entidade.Nome;

        }

    }
}