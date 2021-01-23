using Microsoft.EntityFrameworkCore;
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
    public class ArquivoRequerimentoDao : ICadastrosDao<ArquivoRequerimento, ArquivoRequerimento>
    {
        public ArquivoRequerimento BuscarPorId(int id)
        {
            using (var contexto = new ScirContext())
            {
                return contexto.ArquivoRequerimento.Where(e => e.Id == id).FirstOrDefault();
            }
        }

        public void Delete(ArquivoRequerimento entidade)
        {
            using (var contexto = new ScirContext())
            {
                contexto.ArquivoRequerimento.Remove(entidade);
                contexto.SaveChanges();
            }
        }

        public bool Exist(ArquivoRequerimento entidade)
        {
            using (var contexto = new ScirContext())
            {
                return contexto.ArquivoRequerimento.Contains(entidade);
            }
        }

        public IList<ArquivoRequerimento> FiltroPorColuna(string coluna, string searchPhrase)
        {
            throw new NotImplementedException();
        }

        public void Insert(ArquivoRequerimento entidade)
        {
            using (var context = new ScirContext())
            {
                context.ArquivoRequerimento.Add(entidade);
                context.SaveChanges();
            }
        }

        public IPagedList<ArquivoRequerimento> ListGrid(FormatGridUtils<ArquivoRequerimento> request)
        {
            var where = "REQUERIMENTOID = ";
            if (request.Entidade.RequerimentoId != 0)
                where += request.Entidade.RequerimentoId;
            else
                where = "1 = 2";
            if (!string.IsNullOrWhiteSpace(request.SearchPhrase))
            {
                int id = 0;
                if (int.TryParse(request.SearchPhrase, out id))
                    where = string.Format("Id = {0}", id);


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

                return contexto.ArquivoRequerimento.AsNoTracking().Where(where).Include(p=> p.Requerimento).OrderBy(request.CampoOrdenacao).ToPagedList(request.Current, request.RowCount);
            }
        }

        public int TotalRegistros()
        {
            using (var contexto = new ScirContext())
            {
                return contexto.FluxoStatus.Count();
            }
        }

        public void Update(ArquivoRequerimento entidade)
        {
            using (var contexto = new ScirContext())
            {
                entidade.Requerimento = contexto.Requerimento.Find(entidade.RequerimentoId);
                contexto.ArquivoRequerimento.Update(entidade);
                contexto.SaveChanges();
            }
        }
    }
}