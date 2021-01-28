using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCIR.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic;
using PagedList;
using SCIR.Datacontract.Grid;
using SCIR.Utils;

namespace SCIR.DAO.Formularios
{
    public class RequerimentoDao 
    {
        public static string WhereFiltroPorColuna(string coluna, string searchPhrase)
        {
            var where = "";
            int id = 0;
            if (int.TryParse(searchPhrase, out id))
            {
                switch (coluna.ToUpper())
                {
                    case "USUARIOREQUERENTE":
                        where += string.Format("USUARIOREQUERENTEID = {0}", id);
                        break;
                    case "USUARIOATENDENTE":
                        where += string.Format("USUARIOATENDENTEID = {0}", id);
                        break;
                    case "STATUSREQUERIMENTO":
                        where += string.Format("STATUSREQUERIMENTOID = {0}", id);
                        break;
                    case "TIPOREQUERIMENTO":
                        where += string.Format("TIPOREQUERIMENTOID = {0}", id);
                        break;
                    case "TIPOFORMULARIO":
                        where += string.Format("TIPOFORMULARIOID = {0}", id);
                        break;
                }
            }
            return where;
        }

        public IList<FormularioValidacaoUC> FiltroPorColuna(string coluna, string searchPhrase)
        {
            var where = "";
            if (!string.IsNullOrWhiteSpace(searchPhrase))
            {
                int id = 0;
                if (int.TryParse(searchPhrase, out id))
                {
                    where += RequerimentoDao.WhereFiltroPorColuna(coluna, searchPhrase);
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
                return contexto.FormularioValidacaoUC.AsNoTracking().Where(where).OrderBy(ordenacao).ToList();
            }
        }

        public IPagedList<RequerimentoGridDC> ListGrid(FormatGridUtils<Requerimento> request, bool filtrarPorAtendente, bool filtrarPorRequerente)
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

                var innerJoin = from a in contexto.Requerimento
                                join b in contexto.Usuario on a.UsuarioAtendenteId equals b.Id
                                join c in contexto.Usuario on a.UsuarioRequerenteId equals c.Id
                                join d in contexto.StatusRequerimento on a.StatusRequerimentoId equals d.Id
                                join e in contexto.TipoRequerimento on a.TipoRequerimentoId equals e.Id
                                where (!string.IsNullOrWhiteSpace(request.SearchPhrase) ? EF.Functions.Like(d.Nome, "%" + request.SearchPhrase + "%") || EF.Functions.Like(e.Nome, "%" + request.SearchPhrase + "%") || EF.Functions.Like(a.Protocolo,"%"+request.SearchPhrase+"%")  && ((filtrarPorAtendente? a.UsuarioAtendenteId == request.Entidade.UsuarioAtendenteId : 1 == 2 ) || (filtrarPorRequerente? a.UsuarioRequerenteId == request.Entidade.UsuarioRequerenteId : 1 == 2)) : 1 == 1)
                                select new
                                {
                                    a.Id,
                                    a.Protocolo,
                                    a.Abertura,
                                    a.TipoRequerimentoId,
                                    TipoRequerimento = e.Nome,
                                    a.StatusRequerimentoId,
                                    StatusRequerimento = d.Nome
                                };


                if (string.IsNullOrWhiteSpace(request.CampoOrdenacao))
                    request.CampoOrdenacao = "Id asc";

                var listFluxoStatus = innerJoin.OrderBy(request.CampoOrdenacao).ToPagedList(request.Current, request.RowCount);
                var lista = new List<RequerimentoGridDC>();
                foreach (var item in listFluxoStatus)
                {
                    lista.Add(new RequerimentoGridDC
                    {
                       Id = item.Id,
                       Abertura = item.Abertura,
                       AberturaToString = item.Abertura.ToString("dd/MM/yyyy hh:mm"),
                       TipoRequerimentoNome = item.TipoRequerimento,
                       TipoRequerimentoId = item.TipoRequerimentoId,
                       StatusRequerimentoNome = item.StatusRequerimento,
                       StatusRequerimentoId = item.StatusRequerimentoId,
                       Protocolo = item.Protocolo
                    });
                }
                return lista.ToPagedList(request.Current, request.RowCount);
            }
        }


    }
}