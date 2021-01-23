using SCIR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using PagedList;
using SCIR.Utils;
using Microsoft.EntityFrameworkCore;

namespace SCIR.DAO.Cadastros
{
    internal class CursosDao : ICadastrosDao<Cursos,Cursos>
    {
        public void Insert(Cursos curso)
        {
            using (var context = new ScirContext())
            {
                context.Cursos.Add(curso);
                context.SaveChanges();
            }
        }

        public IPagedList<Cursos> ListGrid(FormatGridUtils<Cursos> request)
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

                return contexto.Cursos.AsNoTracking().Where(where).OrderBy(request.CampoOrdenacao).ToPagedList(request.Current, request.RowCount);
            }
        }

        public int TotalRegistros()
        {
            using (var contexto = new ScirContext())
            {
                return contexto.Cursos.Count();
            }
        }

        public Cursos BuscarPorId(int id)
        {
            using (var contexto = new ScirContext())
            {
                return contexto.Cursos.Where(e => e.Id == id).FirstOrDefault();
            }
        }

        public void Update(Cursos curso)
        {
            using (var contexto = new ScirContext())
            {
                contexto.Cursos.Update(curso);
                contexto.SaveChanges();
            }
        }

        public void Delete(Cursos curso)
        {
            using (var contexto = new ScirContext())
            {
                contexto.Cursos.Remove(curso);
                contexto.SaveChanges();
            }
        }

        public bool Exist(Cursos curso)
        {
            using (var contexto = new ScirContext())
            {
                return contexto.Cursos.Contains(curso);
            }
        }

        public IList<Cursos> FiltroPorColuna(string coluna, string searchPhrase)
        {
            var where = "";
            if (!string.IsNullOrWhiteSpace(searchPhrase))
            {
                where += string.Format(coluna+".Contains(\"{0}\")", searchPhrase);
            }
            else
            {
                where = "1=1";
            }

            using (var contexto = new ScirContext())
            {
                var ordenacao = coluna + " ASC";
                return contexto.Cursos.Where(where).OrderBy(ordenacao).ToList();
            }
        }
    }
}