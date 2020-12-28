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
    public class FluxoStatusDao : ICadastrosDao<FluxoStatus, FluxoStatusGridDC>
    {
        public FluxoStatus BuscarPorId(int id)
        {
            using (var contexto = new ScirContext())
            {
                return contexto.FluxoStatus.Include(e => e.StatusAtual).Include(e=>e.StatusProximo).Include(e=>e.TipoRequerimento).FirstOrDefault();
            }
        }

        public void Delete(FluxoStatus entidade)
        {
            using (var contexto = new ScirContext())
            {
                contexto.FluxoStatus.Remove(entidade);
                contexto.SaveChanges();
            }
        }

        public bool Exist(FluxoStatus entidade)
        {
            using (var contexto = new ScirContext())
            {
                return contexto.FluxoStatus.Contains(entidade);
            }
        }

        public IList<FluxoStatus> FiltroPorColuna(string coluna, string searchPhrase)
        {
            throw new NotImplementedException();
        }

        public void Insert(FluxoStatus entidade)
        {
            using (var context = new ScirContext())
            {
                entidade.StatusAtual = context.StatusRequerimento.Find(entidade.StatusAtualId);
                entidade.StatusProximo = context.StatusRequerimento.Find(entidade.StatusProximoId);
                entidade.TipoRequerimento = context.TipoRequerimento.Find(entidade.TipoRequerimentoId);
                context.FluxoStatus.Add(entidade);
                context.SaveChanges();
            }
        }

        public IPagedList<FluxoStatusGridDC> ListGrid(FormatGridUtils request)
        {
            using (var contexto = new ScirContext())
            {
                var innerJoin = from a in contexto.FluxoStatus
                                join b in contexto.StatusRequerimento on a.StatusAtualId equals b.Id
                                join c in contexto.StatusRequerimento on a.StatusProximoId equals c.Id
                                join d in contexto.TipoRequerimento on a.TipoRequerimentoId equals d.Id
                                where (!string.IsNullOrWhiteSpace(request.SearchPhrase)? EF.Functions.Like(b.Nome, "%"+request.SearchPhrase+"%") || EF.Functions.Like(d.Nome, "%" + request.SearchPhrase + "%") : a.StatusAtualId == a.StatusAtualId)
                                select new
                                {
                                    a.StatusAtualId,
                                    StatusAtualNome = b.Nome,
                                    a.StatusProximoId,
                                    StatusProximoNome = c.Nome,
                                    a.TipoRequerimentoId,
                                    TipoRequerimentoNome = d.Nome
                                };

                
                if (string.IsNullOrWhiteSpace(request.CampoOrdenacao))
                    request.CampoOrdenacao = "Id asc";

                
                var listFluxoStatus = innerJoin.OrderBy(request.CampoOrdenacao).ToPagedList(request.Current, request.RowCount);
                var lista = new List<FluxoStatusGridDC>();
                foreach (var item in listFluxoStatus)
                {
                    lista.Add(new FluxoStatusGridDC
                    {
                        StatusAtualId = item.StatusAtualId,
                        StatusAtualNome = item.StatusAtualNome,
                        StatusProximoId = item.StatusProximoId,
                        StatuProximoNome = item.StatusProximoNome,
                        TipoRequerimentoId = item.TipoRequerimentoId,
                        TipoRequerimentoNome = item.TipoRequerimentoNome
                    });
                }
                return lista.ToPagedList(request.Current, request.RowCount);
            }
        }

        public int TotalRegistros()
        {
            using (var contexto = new ScirContext())
            {
                return contexto.FluxoStatus.Count();
            }
        }

        public void Update(FluxoStatus entidade)
        {
            using (var contexto = new ScirContext())
            {
                entidade.StatusAtual = contexto.StatusRequerimento.Find(entidade.StatusAtualId);
                entidade.StatusProximo = contexto.StatusRequerimento.Find(entidade.StatusProximoId);
                entidade.TipoRequerimento = contexto.TipoRequerimento.Find(entidade.TipoRequerimentoId);
                contexto.FluxoStatus.Update(entidade);
                contexto.SaveChanges();
            }
        }
    }
}