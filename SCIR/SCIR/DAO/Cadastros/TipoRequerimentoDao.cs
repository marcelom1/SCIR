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
    public class TipoRequerimentoDao : ICadastrosDao<TipoRequerimento, TipoRequerimentoGridDC>
    {
        public TipoRequerimento BuscarPorId(int id)
        {
            using (var contexto = new ScirContext())
            {
                return contexto.TipoRequerimento.Include(e => e.TipoFormulario).Where(e => e.Id == id).FirstOrDefault();
            }
        }

        public void Delete(TipoRequerimento entidade)
        {
            using (var contexto = new ScirContext())
            {
                contexto.TipoRequerimento.Remove(entidade);
                contexto.SaveChanges();
            }
        }

        public bool Exist(TipoRequerimento entidade)
        {
            using (var contexto = new ScirContext())
            {
                return contexto.TipoRequerimento.Contains(entidade);
            }
        }

        public IList<TipoRequerimento> FiltroPorColuna(string coluna, string searchPhrase)
        {
            var where = "";
            if (!string.IsNullOrWhiteSpace(searchPhrase))
            {
                where += string.Format(coluna + ".Contains(\"{0}\")", searchPhrase);
            }
            else
            {
                where = "1=1";
            }

            using (var contexto = new ScirContext())
            {
                var ordenacao = coluna + " ASC";
                return contexto.TipoRequerimento.Where(where).OrderBy(ordenacao).ToList();
            }
        }

        public void Insert(TipoRequerimento entidade)
        {
            using (var context = new ScirContext())
            {
                entidade.TipoFormulario = context.TipoFormulario.Find(entidade.TipoFormularioId);
                context.TipoRequerimento.Add(entidade);
                context.SaveChanges();
            }
        }

        public IPagedList<TipoRequerimentoGridDC> ListGrid(FormatGridUtils<TipoRequerimento> request)
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

                var listUnidadeCurricular = contexto.TipoRequerimento.Include(e => e.TipoFormulario).AsNoTracking().Where(where).OrderBy(request.CampoOrdenacao).ToPagedList(request.Current, request.RowCount);

                var lista = new List<TipoRequerimentoGridDC>();
                foreach (var item in listUnidadeCurricular)
                {
                    lista.Add(new TipoRequerimentoGridDC
                    {
                        Id = item.Id,
                        Ativo = item.Ativo,
                        Nome = item.Nome,
                        TipoFormulario = item.TipoFormulario.Id + " - " + item.TipoFormulario.Nome
                    });
                }
                return lista.ToPagedList(request.Current, request.RowCount);
            }
        }

        public int TotalRegistros()
        {
            using (var contexto = new ScirContext())
            {
                return contexto.UnidadeCurricular.Count();
            }
        }

        public void Update(TipoRequerimento entidade)
        {
            using (var contexto = new ScirContext())
            {
                entidade.TipoFormulario = contexto.TipoFormulario.Find(entidade.TipoFormularioId);
                contexto.TipoRequerimento.Update(entidade);
                contexto.SaveChanges();
            }
        }

    }
}