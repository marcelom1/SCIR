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
            throw new NotImplementedException();
        }

        public FluxoStatus GetEntidade(FluxoStatus entidade)
        {
            using (var contexto = new ScirContext())
            {
                return contexto.FluxoStatus
                               .Where(e => e.StatusAtualId == entidade.StatusAtualId && e.TipoRequerimentoId == entidade.TipoRequerimentoId && e.StatusProximoId == entidade.StatusProximoId)
                               .Include(e => e.StatusAtual)
                               .Include(e => e.StatusProximo)
                               .Include(e => e.TipoRequerimento).FirstOrDefault();
            }
        }

        public IList<FluxoStatus> BuscarPorId(int statusAtual, int tipoRequerimento)
        {
            using (var contexto = new ScirContext())
            {
                return contexto.FluxoStatus.Where(e=>e.StatusAtualId == statusAtual && e.TipoRequerimentoId == tipoRequerimento).Include(e => e.StatusAtual).Include(e => e.StatusProximo).Include(e => e.TipoRequerimento).AsTracking().ToList();
            }
        }

        public void Delete(FluxoStatus entidade)
        {
            using (var contexto = new ScirContext())
            {
                contexto.FluxoStatus.Remove(contexto.FluxoStatus
                               .Where(e => e.StatusAtualId == entidade.StatusAtualId && e.TipoRequerimentoId == entidade.TipoRequerimentoId && e.StatusProximoId == entidade.StatusProximoId)
                               .Include(e => e.StatusAtual)
                               .Include(e => e.StatusProximo)
                               .Include(e => e.TipoRequerimento).FirstOrDefault());
                contexto.SaveChanges();
            }
        }

        public void DeleteAll(FluxoStatus entidades)
        {
            using (var contexto = new ScirContext())
            {
                contexto.FluxoStatus.RemoveRange(contexto.FluxoStatus.Where(e => e.StatusAtualId == entidades.StatusAtualId && e.TipoRequerimentoId == entidades.TipoRequerimentoId).Include(e => e.StatusAtual).Include(e => e.StatusProximo).Include(e => e.TipoRequerimento));
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
            var where = "";
            if (!string.IsNullOrWhiteSpace(searchPhrase))
            {
                int id = 0;
                if (int.TryParse(searchPhrase, out id))
                {
                    switch (coluna.ToUpper())
                    {
                        case "TIPOREQUERIMENTO":
                            where += string.Format("TIPOREQUERIMENTOID = {0}", id);
                            break;
                        case "STATUSATUALID":
                            where += string.Format("StatusAtualId = {0}", id);
                            break;
                        case "STATUSPROXIMOID":
                            where += string.Format("StatusProximoId = {0}", id);
                            break;
                    }
                }
                else
                {
                    where += string.Format(coluna + ".Contains(\"{0}\")", searchPhrase);
                }
            }
            else
            {
                where = "1=1";
            }

            using (var contexto = new ScirContext())
            {
                var ordenacao = coluna + " ASC";
                return contexto.FluxoStatus.AsNoTracking().Where(where).OrderBy(ordenacao).ToList();
            }
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

        public IPagedList<FluxoStatusGridDC> ListProximosGrid(FormatGridUtils<FluxoStatus> request, int statusAtualId, int tipoRequerimentoId)
        {
            using (var contexto = new ScirContext())
            {
                var innerJoin = from a in contexto.FluxoStatus
                                join b in contexto.StatusRequerimento on a.StatusAtualId equals b.Id
                                join c in contexto.StatusRequerimento on a.StatusProximoId equals c.Id
                                join d in contexto.TipoRequerimento on a.TipoRequerimentoId equals d.Id
                                where (a.StatusAtualId == statusAtualId && a.TipoRequerimentoId == tipoRequerimentoId) && (!string.IsNullOrWhiteSpace(request.SearchPhrase) ? (EF.Functions.Like(c.Nome, "%" + request.SearchPhrase + "%") || EF.Functions.Like(d.Nome, "%" + request.SearchPhrase + "%")) : (true))
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
                        StatusProximoNome = item.StatusProximoNome,
                        TipoRequerimentoId = item.TipoRequerimentoId,
                        TipoRequerimentoNome = item.TipoRequerimentoNome
                    });
                }
                return lista.ToPagedList(request.Current, request.RowCount);
            }
        }

        public IList<FluxoStatusGridDC> ListProximos(string searchPhrase, int statusAtualId, int tipoRequerimentoId)
        {
            using (var contexto = new ScirContext())
            {
                var innerJoin = from a in contexto.FluxoStatus
                                join b in contexto.StatusRequerimento on a.StatusAtualId equals b.Id
                                join c in contexto.StatusRequerimento on a.StatusProximoId equals c.Id
                                join d in contexto.TipoRequerimento on a.TipoRequerimentoId equals d.Id
                                where (a.StatusAtualId == statusAtualId && a.TipoRequerimentoId == tipoRequerimentoId) && (!string.IsNullOrWhiteSpace(searchPhrase) ? (EF.Functions.Like(c.Nome, "%" + searchPhrase + "%") || EF.Functions.Like(d.Nome, "%" + searchPhrase + "%")) : (true))
                                select new
                                {
                                    a.StatusAtualId,
                                    StatusAtualNome = b.Nome,
                                    a.StatusProximoId,
                                    StatusProximoNome = c.Nome,
                                    a.TipoRequerimentoId,
                                    TipoRequerimentoNome = d.Nome
                                };


               
                var listFluxoStatus = innerJoin.ToList();
                var lista = new List<FluxoStatusGridDC>();
                foreach (var item in listFluxoStatus)
                {
                    lista.Add(new FluxoStatusGridDC
                    {
                        StatusAtualId = item.StatusAtualId,
                        StatusAtualNome = item.StatusAtualNome,
                        StatusProximoId = item.StatusProximoId,
                        StatusProximoNome = item.StatusProximoNome,
                        TipoRequerimentoId = item.TipoRequerimentoId,
                        TipoRequerimentoNome = item.TipoRequerimentoNome
                    });
                }
                return lista;
            }
        }

        public IPagedList<FluxoStatusGridDC> ListGrid(FormatGridUtils<FluxoStatus> request)
        {
            using (var contexto = new ScirContext())
            {
                //var innerJoin = from a in contexto.FluxoStatus
                //                join b in contexto.StatusRequerimento on a.StatusAtualId equals b.Id
                //                join c in contexto.StatusRequerimento on a.StatusProximoId equals c.Id
                //                join d in contexto.TipoRequerimento on a.TipoRequerimentoId equals d.Id
                //                where (!string.IsNullOrWhiteSpace(request.SearchPhrase)? EF.Functions.Like(b.Nome, "%"+request.SearchPhrase+"%") || EF.Functions.Like(d.Nome, "%" + request.SearchPhrase + "%") : a.StatusAtualId == a.StatusAtualId)
                //                select new
                //                {
                //                    a.StatusAtualId,
                //                    StatusAtualNome = b.Nome,
                //                    a.StatusProximoId,
                //                    StatusProximoNome = c.Nome,
                //                    a.TipoRequerimentoId,
                //                    TipoRequerimentoNome = d.Nome
                //                };

                var innerJoin = from a in contexto.FluxoStatus
                                join b in contexto.StatusRequerimento on a.StatusAtualId equals b.Id
                                join c in contexto.StatusRequerimento on a.StatusProximoId equals c.Id
                                join d in contexto.TipoRequerimento on a.TipoRequerimentoId equals d.Id
                                where (!string.IsNullOrWhiteSpace(request.SearchPhrase) ? EF.Functions.Like(b.Nome, "%" + request.SearchPhrase + "%") || EF.Functions.Like(d.Nome, "%" + request.SearchPhrase + "%") : a.StatusAtualId == a.StatusAtualId)
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

                
                var listFluxoStatus = innerJoin.GroupBy(g=>new {g.StatusAtualId, g.StatusAtualNome, g.TipoRequerimentoId, g.TipoRequerimentoNome }).Select(s=> new {s.Key.StatusAtualId, s.Key.StatusAtualNome,s.Key.TipoRequerimentoId,s.Key.TipoRequerimentoNome }).OrderBy(request.CampoOrdenacao).ToPagedList(request.Current, request.RowCount);
                var lista = new List<FluxoStatusGridDC>();
                foreach (var item in listFluxoStatus)
                {
                    lista.Add(new FluxoStatusGridDC
                    {
                        StatusAtualId = item.StatusAtualId,
                        StatusAtualNome = item.StatusAtualNome,
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