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
    public class AuditoriaDao : ICadastrosDao<Auditoria, Auditoria>
    {
        public Auditoria BuscarPorId(int id)
        {
            using (var contexto = new ScirContext())
            {
                return contexto.Auditoria.Include(e => e.Requerimento).Where(e => e.Id == id).AsNoTracking().FirstOrDefault();
            }
        }

        public void Delete(Auditoria entidade)
        {
            using (var contexto = new ScirContext())
            {
                contexto.Auditoria.Remove(entidade);
                contexto.SaveChanges();
            }
        }

        public bool Exist(Auditoria entidade)
        {
            throw new NotImplementedException();
        }

        public IList<Auditoria> FiltroPorColuna(string coluna, string searchPhrase)
        {
            throw new NotImplementedException();
        }

        public IList<Auditoria> FiltroPorRequerimento(int requerimentoId)
        {
            var where = string.Format("RequerimentoId = {0}", requerimentoId);
           
            using (var contexto = new ScirContext())
            {
                var campoOrdenacao = "DataModificacao desc";

                var list = contexto.Auditoria.Include(e => e.Requerimento).AsNoTracking().Where(where).OrderBy(campoOrdenacao).ToList();

                return list;
            }
        }

        public void Insert(Auditoria entidade)
        {
            using (var context = new ScirContext())
            {
                entidade.Requerimento = context.Requerimento.Find(entidade.RequerimentoId);
                context.Auditoria.Add(entidade);
                context.SaveChanges();
            }
        }

        public IPagedList<Auditoria> ListGrid(FormatGridUtils<Auditoria> request)
        {
            var where = string.Format("RequerimentoId = {0}", request.Entidade.RequerimentoId);
            if (!string.IsNullOrWhiteSpace(request.SearchPhrase))
            {
                int id = 0;
                if (int.TryParse(request.SearchPhrase, out id))
                    where += string.Format("Id = {0}", id);

                if (!string.IsNullOrWhiteSpace(where))
                    where += " OR ";

                where += string.Format("Campo.Contains(\"{0}\")", request.SearchPhrase);
                where += " OR ";
                where += string.Format("Antes.Contains(\"{0}\")", request.SearchPhrase);
                where += " OR ";
                where += string.Format("Depois.Contains(\"{0}\")", request.SearchPhrase);            
            }

            using (var contexto = new ScirContext())
            {
                if (string.IsNullOrWhiteSpace(request.CampoOrdenacao))
                    request.CampoOrdenacao = "Id asc";

                var list = contexto.Auditoria.Include(e => e.Requerimento).AsNoTracking().Where(where).OrderBy(request.CampoOrdenacao).ToPagedList(request.Current, request.RowCount);
                
                return list.ToPagedList(request.Current, request.RowCount);
            }
        }

       

        public int TotalRegistros()
        {
            using (var contexto = new ScirContext())
            {
                return contexto.Auditoria.Count();
            }
        }

        public void Update(Auditoria entidade)
        {
            using (var context = new ScirContext())
            {
                entidade.Requerimento = context.Requerimento.Find(entidade.RequerimentoId);
                context.Auditoria.Update(entidade);
                context.SaveChanges();
            }
        }
    }
}