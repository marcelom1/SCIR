using PagedList;
using SCIR.Models;
using SCIR.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;

namespace SCIR.DAO.Cadastros
{
    public class UnidadeCurricularDao : ICadastrosDao<UnidadeCurricular>
    {
        public UnidadeCurricular BuscarPorId(int id)
        {
            using (var contexto = new ScirContext())
            {
                return contexto.UnidadeCurricular.Where(e => e.Id == id).FirstOrDefault();
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

        public void Insert(UnidadeCurricular entidade)
        {
            using (var context = new ScirContext())
            {
                context.UnidadeCurricular.Add(entidade);
                context.SaveChanges();
            }
        }

        public IPagedList<UnidadeCurricular> ListGrid(FormatGridUtils request)
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

                return contexto.UnidadeCurricular.Where(where).OrderBy(request.CampoOrdenacao).ToPagedList(request.Current, request.RowCount);
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
                contexto.UnidadeCurricular.Update(entidade);
                contexto.SaveChanges();
            }
        }
    }
}