using SCIR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using PagedList;

namespace SCIR.DAO.Cadastros
{
    public class CursosDao
    {
        public void Add(Cursos curso)
        {
            using (var context = new ScirContext())
            {
                context.Cursos.Add(curso);
                context.SaveChanges();
            }
        }

        public IPagedList<Cursos> ListGrid(int pagina = 1, int registros = 10, string campoOrdenacao = "Id asc", string searchParser = "")
        {
            var where = "";
            if (!string.IsNullOrWhiteSpace(searchParser))
            {
                int id = 0;
                if (int.TryParse(searchParser, out id)) 
                    where = string.Format("Id = {0}", id);

                bool ativo = true;
                if (bool.TryParse(searchParser, out ativo))
                {
                    if (!string.IsNullOrWhiteSpace(where))
                        where += " OR ";
                    where += string.Format("Ativo = {0} ", ativo);
                }

                if (!string.IsNullOrWhiteSpace(where))
                    where += " OR ";

                where += string.Format("Nome.Contains(\"{0}\")", searchParser);
            }
            else
            {
                where = "1=1";
            }

            using (var contexto = new ScirContext())
            {
                return contexto.Cursos.Where(where).OrderBy(campoOrdenacao).ToPagedList(pagina, registros);
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

    }
}