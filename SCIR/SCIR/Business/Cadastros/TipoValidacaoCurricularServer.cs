using SCIR.DAO.Cadastros;
using SCIR.Models;
using SCIR.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCIR.Business.Cadastros
{
    public class TipoValidacaoCurricularServer
    {
        private TipoValidacaoCurricularDao dbTipoValidacaoCurricular = new TipoValidacaoCurricularDao();

        public ConsisteUtils ConsisteNovo(TipoValidacaoCurricular tipoValidacaoCurricular)
        {
            var consiste = new ConsisteUtils();

            if (string.IsNullOrWhiteSpace(tipoValidacaoCurricular.Nome))
                consiste.Add("O campo Nome não pode ficar em branco", ConsisteUtils.Tipo.Inconsistencia);

            return consiste;
        }

        public ConsisteUtils ConsisteExcluir(TipoValidacaoCurricular tipoValidacaoCurricular)
        {
            var consiste = new ConsisteUtils();

            if (tipoValidacaoCurricular == null)
                consiste.Add("Não foi encontrado o registro para exclusão", ConsisteUtils.Tipo.Inconsistencia);

            return consiste;
        }

        public ConsisteUtils ConsisteAtualizar(TipoValidacaoCurricular tipoValidacaoCurricular)
        {
            var consiste = new ConsisteUtils();

            var pesquisa = dbTipoValidacaoCurricular.BuscarPorId(tipoValidacaoCurricular.Id);
            tipoValidacaoCurricular = pesquisa;

            if (pesquisa == null)
                consiste.Add("Não foi encontrado o registro para atualização", ConsisteUtils.Tipo.Inconsistencia);


            return consiste;
        }

        public TipoValidacaoCurricular Novo(TipoValidacaoCurricular tipoValidacaoCurricular)
        {
            var consiste = ConsisteNovo(tipoValidacaoCurricular);

            if (consiste.Inconsistencias.Any())
                throw new ArgumentException(consiste.Inconsistencias.ToString());
            else
                dbTipoValidacaoCurricular.Insert(tipoValidacaoCurricular);


            return tipoValidacaoCurricular;
        }

        public TipoValidacaoCurricular Excluir(TipoValidacaoCurricular tipoValidacaoCurricular)
        {
            var pesquisa = dbTipoValidacaoCurricular.BuscarPorId(tipoValidacaoCurricular.Id);
            tipoValidacaoCurricular = pesquisa;

            var consiste = ConsisteExcluir(tipoValidacaoCurricular);

            if (consiste.Inconsistencias.Any())
                throw new ArgumentException(consiste.Inconsistencias.ToString());
            else
                dbTipoValidacaoCurricular.Delete(tipoValidacaoCurricular);

            return tipoValidacaoCurricular;
        }

        public TipoValidacaoCurricular Atualizar(TipoValidacaoCurricular tipoValidacaoCurricular)
        {
            var consiste = ConsisteAtualizar(tipoValidacaoCurricular);

            if (consiste.Inconsistencias.Any())
                throw new ArgumentException(consiste.Inconsistencias.ToString());
            else
                dbTipoValidacaoCurricular.Update(tipoValidacaoCurricular);


            return tipoValidacaoCurricular;
        }

        public TipoValidacaoCurricular GetEntidade(int id)
        {
            return dbTipoValidacaoCurricular.BuscarPorId(id);
        }

        public IList<TipoValidacaoCurricular> GetFiltroEntidadeString(string coluna, string searchTerm)
        {
            return dbTipoValidacaoCurricular.FiltroPorColuna(coluna, searchTerm);
        }

    }
}
