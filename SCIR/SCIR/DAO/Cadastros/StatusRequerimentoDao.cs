using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using Microsoft.EntityFrameworkCore;
using PagedList;
using SCIR.Models;
using SCIR.Utils;

namespace SCIR.DAO.Cadastros
{
    public class StatusRequerimentoDao : ICadastrosDao<StatusRequerimento, StatusRequerimento>
    {
        public StatusRequerimento BuscarPorId(int id)
        {
            using (var contexto = new ScirContext())
            {
                return contexto.StatusRequerimento.Where(e => e.Id == id).FirstOrDefault();
            }
        }

        public void Delete(StatusRequerimento entidade)
        {
            using (var contexto = new ScirContext())
            {
                contexto.StatusRequerimento.Remove(entidade);
                contexto.SaveChanges();
            }
        }

        public bool Exist(StatusRequerimento entidade)
        {
            using (var contexto = new ScirContext())
            {
                return contexto.StatusRequerimento.Contains(entidade);
            }
        }

        public IList<StatusRequerimento> FiltroPorColuna(string coluna, string searchPhrase)
        {
            throw new NotImplementedException();
        }

        public void Insert(StatusRequerimento entidade)
        {
            using (var context = new ScirContext())
            {
                context.StatusRequerimento.Add(entidade);
                context.SaveChanges();
            }
        }

        public IPagedList<StatusRequerimento> ListGrid(FormatGridUtils request)
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
                    where += " OR ";
                    where += string.Format("Cancelamento = {0} ", ativo);
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

                return contexto.StatusRequerimento.AsNoTracking().Where(where).OrderBy(request.CampoOrdenacao).ToPagedList(request.Current, request.RowCount);
            }
        }

        public int TotalRegistros()
        {
            using (var contexto = new ScirContext())
            {
                return contexto.StatusRequerimento.Count();
            }
        }

        public void Update(StatusRequerimento entidade)
        {
            using (var contexto = new ScirContext())
            {
                contexto.StatusRequerimento.Update(entidade);
                contexto.SaveChanges();
            }
        }
    }
}