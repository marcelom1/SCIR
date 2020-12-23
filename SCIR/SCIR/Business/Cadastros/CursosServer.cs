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
    public class CursosServer 
    {
        private CursosDao dbCursos = new CursosDao();

        public ConsisteUtils ConsisteNovo(Cursos curso)
        {
            var consiste = new ConsisteUtils();

            if (string.IsNullOrWhiteSpace(curso.Nome))
                consiste.Add("O campo Nome não pode ficar em branco",ConsisteUtils.Tipo.Inconsistencia);

            return consiste;
        }

        public ConsisteUtils ConsisteExcluir(Cursos curso)
        {
            var consiste = new ConsisteUtils();

            var pesquisa = dbCursos.BuscarPorId(curso.Id);
            curso = pesquisa;

            if (curso == null)
                consiste.Add("Não foi encontrado o registro para exclusão", ConsisteUtils.Tipo.Inconsistencia);

            return consiste;
        }

        public ConsisteUtils ConsisteAtualizar(Cursos curso)
        {
            var consiste = new ConsisteUtils();

            var pesquisa = dbCursos.BuscarPorId(curso.Id);
            curso = pesquisa;

            if (pesquisa == null)
                consiste.Add("Não foi encontrado o registro para atualização", ConsisteUtils.Tipo.Inconsistencia);

            return consiste;
        }

        public Cursos Novo(Cursos curso)
        {
            var consiste = ConsisteNovo(curso);

            if (consiste.Inconsistencias.Any())
                throw new ArgumentException(consiste.Inconsistencias.ToString());
            else
                dbCursos.Insert(curso);


            return curso;
        }

        public Cursos Excluir(Cursos curso)
        {
            var consiste = ConsisteExcluir(curso);

            if (consiste.Inconsistencias.Any())
                throw new ArgumentException(consiste.Inconsistencias.ToString()); 
            else
                dbCursos.Delete(curso);

            return curso;
        }

        public Cursos Atualizar(Cursos curso)
        {
            var consiste = ConsisteAtualizar(curso);

            if (consiste.Inconsistencias.Any())
                throw new ArgumentException(consiste.Inconsistencias.ToString());
            else
                dbCursos.Update(curso);


            return curso;
        }

        public ResponseGrid<Cursos> Listar(FormatGridUtils request)
        {
            var response = new ResponseGrid<Cursos>();
            response.Entidades = dbCursos.ListGrid(request);
            response.QuantidadeRegistros = response.Entidades.TotalItemCount;

            return response;
        }

        public Cursos GetEntidade(int id)
        {
            return dbCursos.BuscarPorId(id);
        }

        public IList<Cursos> GetFiltroEntidadeString(string coluna ,string searchTerm)
        {
            return dbCursos.FiltroPorColuna(coluna,searchTerm);
        }

    }
}