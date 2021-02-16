using Microsoft.EntityFrameworkCore;
using PagedList;
using SCIR.Datacontract.Grid;
using SCIR.Models;
using SCIR.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;

namespace SCIR.DAO.Cadastros
{
    public class UnidadeCurricularDao : ICadastrosDao<UnidadeCurricular, UnidadeCurricularGridDC>
    {
        public UnidadeCurricular BuscarPorId(int id)
        {
            using (var contexto = new ScirContext())
            {
                return contexto.UnidadeCurricular.Include(e => e.Curso).Where(e => e.Id == id).FirstOrDefault();
            }
        }

        public void Delete(UnidadeCurricular entidade)
        {
            using (var contexto = new ScirContext())
            {
                contexto.UnidadeCurricular.Remove(entidade);
                contexto.SaveChanges();
            }
        }

        public bool Exist(UnidadeCurricular entidade)
        {
            using (var contexto = new ScirContext())
            {
                return contexto.UnidadeCurricular.Contains(entidade);
            }
        }

        public IList<UnidadeCurricular> FiltroPorColuna(string coluna, string searchPhrase)
        {
            var where = "";
            if (!string.IsNullOrWhiteSpace(searchPhrase))
            {
                int id = 0;
                if (coluna.ToUpper() == "CURSO" && int.TryParse(searchPhrase, out id))
                    where += string.Format("CURSOID = {0}", id);
                else
                    where += string.Format(coluna + ".Contains(\"{0}\")", searchPhrase);
            }
            else
            {
                where = "1=1";
            }

            using (var contexto = new ScirContext())
            {
                var ordenacao = coluna + " ASC";
                return contexto.UnidadeCurricular.AsNoTracking().Where(where).OrderBy(ordenacao).ToList();
            }
        }

        public IList<UnidadeCurricular> FiltroPorColuna(string coluna, string searchPhrase, int cursoId)
        {
            var where = "CURSOID = " + cursoId;
            if (!string.IsNullOrWhiteSpace(searchPhrase))
            {
                where += " AND " + string.Format(coluna + ".Contains(\"{0}\")", searchPhrase);
            }
            
            using (var contexto = new ScirContext())
            {
                var ordenacao = coluna + " ASC";
                return contexto.UnidadeCurricular.AsNoTracking().Where(where).OrderBy(ordenacao).ToList();
            }
        }

        public void Insert(UnidadeCurricular entidade)
        {
            using (var context = new ScirContext())
            {
                entidade.Curso = context.Cursos.Find(entidade.CursoId);
                context.UnidadeCurricular.Add(entidade);
                context.SaveChanges();
            }
        }

        public IPagedList<UnidadeCurricularGridDC> ListGrid(FormatGridUtils<UnidadeCurricular> request)
        {
            var where = "";
            if (!string.IsNullOrWhiteSpace(request.SearchPhrase))
            {
                int id = 0;
                if (int.TryParse(request.SearchPhrase, out id))
                    where = string.Format("Id = {0}", id);

                bool ativo = true;
                if (bool.TryParse(request.SearchPhrase, out ativo))
                {
                    if (!string.IsNullOrWhiteSpace(where))
                        where += " OR ";
                    where += string.Format("Ativo = {0} ", ativo);
                }

                if (!string.IsNullOrWhiteSpace(where))
                    where += " OR ";

                where += string.Format("Nome.Contains(\"{0}\")", request.SearchPhrase);
            }
            else
            {
                where = "1=1";
            }

            using (var contexto = new ScirContext())
            {
                if (string.IsNullOrWhiteSpace(request.CampoOrdenacao))
                    request.CampoOrdenacao = "Id asc";

                var listUnidadeCurricular = contexto.UnidadeCurricular.Include(e=>e.Curso).AsNoTracking().Where(where).OrderBy(request.CampoOrdenacao).ToPagedList(request.Current, request.RowCount);

                var lista = new List<UnidadeCurricularGridDC>();
                foreach (var item in listUnidadeCurricular)
                {
                    lista.Add(new UnidadeCurricularGridDC {
                        Id = item.Id,
                        Ativo = item.Ativo,
                        Nome = item.Nome,
                        Curso = item.Curso.Id +" - "+ item.Curso.Nome,
                        TotalItensGrid = listUnidadeCurricular.TotalItemCount
                    });
                }
                return lista.ToPagedList(1, request.RowCount);
            }
        }

        public int TotalRegistros()
        {
            using (var contexto = new ScirContext())
            {
                return contexto.UnidadeCurricular.Count();
            }
        }

        public void Update(UnidadeCurricular entidade)
        {
            using (var contexto = new ScirContext())
            {
                entidade.Curso = contexto.Cursos.Find(entidade.CursoId);
                contexto.UnidadeCurricular.Update(entidade);
                contexto.SaveChanges();
            }
        }

    }
}