using SCIR.DAO.Cadastros;
using SCIR.Datacontract.Grid;
using SCIR.Models;
using SCIR.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCIR.Business.Cadastros
{
    public class UnidadeCurricularServer
    {
        private UnidadeCurricularDao dbUnidadeCurricular = new UnidadeCurricularDao();

        public ConsisteUtils ConsisteNovo(UnidadeCurricular unidadeCurricular)
        {
            var consiste = new ConsisteUtils();

            if (string.IsNullOrWhiteSpace(unidadeCurricular.Nome))
                consiste.Add("O campo Nome não pode ficar em branco", ConsisteUtils.Tipo.Inconsistencia);

            if (unidadeCurricular.CursoId == 0)
                consiste.Add("O campo Curso não pode ficar em branco", ConsisteUtils.Tipo.Inconsistencia);

            return consiste;
        }

        public ConsisteUtils ConsisteExcluir(UnidadeCurricular unidadeCurricular)
        {
            var consiste = new ConsisteUtils();

            if (unidadeCurricular == null)
                consiste.Add("Não foi encontrado o registro para exclusão", ConsisteUtils.Tipo.Inconsistencia);

            //FAZER NO FUTURO VALIDAÇÃO NA EXCLUSÃO CASO A UNIDADE CURRICULAR ESTIVER ATRELADO A UM FORMULÁRIO NÃO DEIXAR EXCLUIR

            return consiste;
        }

        public ConsisteUtils ConsisteAtualizar(UnidadeCurricular unidadeCurricular)
        {
            var consiste = new ConsisteUtils();

            var pesquisa = dbUnidadeCurricular.BuscarPorId(unidadeCurricular.Id);
            unidadeCurricular = pesquisa;

            if (pesquisa == null)
                consiste.Add("Não foi encontrado o registro para atualização", ConsisteUtils.Tipo.Inconsistencia);

            if (unidadeCurricular.CursoId == 0)
                consiste.Add("O campo Curso não pode ficar em branco", ConsisteUtils.Tipo.Inconsistencia);

            return consiste;
        }

        public UnidadeCurricular Novo(UnidadeCurricular unidadeCurricular)
        {
            var consiste = ConsisteNovo(unidadeCurricular);

            if (consiste.Inconsistencias.Any())
                throw new ArgumentException(consiste.Inconsistencias.ToString());
            else
                dbUnidadeCurricular.Insert(unidadeCurricular);


            return unidadeCurricular;
        }

        public UnidadeCurricular Excluir(UnidadeCurricular unidadeCurricular)
        {
            var pesquisa = dbUnidadeCurricular.BuscarPorId(unidadeCurricular.Id);
            unidadeCurricular = pesquisa;

            var consiste = ConsisteExcluir(unidadeCurricular);

            if (consiste.Inconsistencias.Any())
                throw new ArgumentException(consiste.Inconsistencias.ToString());
            else
                dbUnidadeCurricular.Delete(unidadeCurricular);

            return unidadeCurricular;
        }

        public UnidadeCurricular Atualizar(UnidadeCurricular unidadeCurricular)
        {
            var consiste = ConsisteAtualizar(unidadeCurricular);

            if (consiste.Inconsistencias.Any())
                throw new ArgumentException(consiste.Inconsistencias.ToString());
            else
                dbUnidadeCurricular.Update(unidadeCurricular);


            return unidadeCurricular;
        }

        public UnidadeCurricular GetEntidade(int id)
        {
            return dbUnidadeCurricular.BuscarPorId(id);
        }

        public ResponseGrid<UnidadeCurricularGridDC> Listar(FormatGridUtils<UnidadeCurricular> request)
        {
            var response = new ResponseGrid<UnidadeCurricularGridDC>();
            response.Entidades = dbUnidadeCurricular.ListGrid(request);
            response.QuantidadeRegistros = response.Entidades.Any() ? response.Entidades.FirstOrDefault().TotalItensGrid : 0;

            return response;
        }

        public IList<UnidadeCurricular> GetFiltroEntidadeString(string coluna, string searchTerm)
        {
            return dbUnidadeCurricular.FiltroPorColuna(coluna, searchTerm);
        }

        public IList<UnidadeCurricular> GetFiltroEntidadeString(string coluna, string searchTerm, int cursoId)
        {
            return dbUnidadeCurricular.FiltroPorColuna(coluna, searchTerm, cursoId);
        }

    }
}