﻿using Microsoft.EntityFrameworkCore;
using PagedList;
using SCIR.Models;
using SCIR.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;

namespace SCIR.DAO.Formularios
{
    public class FormularioValidacaoUCDao : IFormulariosDao<FormularioValidacaoUC, FormularioValidacaoUC>
    {
        public FormularioValidacaoUC BuscarPorId(int id)
        {
            using (var contexto = new ScirContext())
            {
                return contexto.FormularioValidacaoUC.Include(e => e.StatusRequerimento)
                                                     .Include(e => e.TipoFormulario)
                                                     .Include(e => e.TipoRequerimento)
                                                     .Include(e => e.TipoValidacaoCurricular)
                                                     .Include(e => e.UnidadeCurricular)
                                                     .Include(e => e.UsuarioAtendente)
                                                     .Include(e => e.UsuarioRequerente)
                                                     .Where(e => e.Id == id)
                                                     .FirstOrDefault();
            }
        }

        public void Delete(FormularioValidacaoUC entidade)
        {
            using (var contexto = new ScirContext())
            {
                contexto.FormularioValidacaoUC.Remove(entidade);
                contexto.SaveChanges();
            }
        }

        public IList<FormularioValidacaoUC> FiltroPorColuna(string coluna, string searchPhrase)
        {
            var where = "";
            if (!string.IsNullOrWhiteSpace(searchPhrase))
            {
                int id = 0;
                if (int.TryParse(searchPhrase, out id))
                {
                    var filtroFormulario = RequerimentoDao.WhereFiltroPorColuna(coluna, searchPhrase);

                    if (string.IsNullOrWhiteSpace(filtroFormulario))
                        switch (coluna.ToUpper())
                        {
                            case "UNIDADECURRICULAR":
                                where += string.Format("UNIDADECURRICULARID = {0}", id);
                                break;
                            case "TIPOVALIDACAOCURRICULAR":
                                where += string.Format("TIPOVALIDACAOCURRICULARID = {0}", id);
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
                return contexto.FormularioValidacaoUC.AsNoTracking().Where(where).OrderBy(ordenacao).ToList();
            }
        }

        public void Insert(FormularioValidacaoUC entidade)
        {
            using (var context = new ScirContext())
            {
                entidade.StatusRequerimento = context.StatusRequerimento.Find(entidade.StatusRequerimentoId);
                entidade.TipoFormulario = context.TipoFormulario.Find(entidade.TipoFormularioId);
                entidade.TipoRequerimento = context.TipoRequerimento.Find(entidade.TipoRequerimentoId);
                entidade.TipoValidacaoCurricular = context.TipoValidacaoCurricular.Find(entidade.TipoValidacaoCurricularId);
                entidade.UnidadeCurricular = context.UnidadeCurricular.Find(entidade.UnidadeCurricularId);
                entidade.UsuarioAtendente = context.Usuario.Find(entidade.UsuarioAtendenteId);
                entidade.UsuarioRequerente = context.Usuario.Find(entidade.UsuarioRequerenteId);
                entidade.Abertura = DateTime.Now;

                context.FormularioValidacaoUC.Add(entidade);
                context.SaveChanges();
            }
        }

        public IPagedList<FormularioValidacaoUC> ListGrid(FormatGridUtils request)
        {
            throw new NotImplementedException();
        }

        public void Update(FormularioValidacaoUC entidade)
        {
            using (var context = new ScirContext())
            {
                entidade.StatusRequerimento = context.StatusRequerimento.Find(entidade.StatusRequerimentoId);
                entidade.TipoFormulario = context.TipoFormulario.Find(entidade.TipoFormularioId);
                entidade.TipoRequerimento = context.TipoRequerimento.Find(entidade.TipoRequerimentoId);
                entidade.TipoValidacaoCurricular = context.TipoValidacaoCurricular.Find(entidade.TipoValidacaoCurricularId);
                entidade.UnidadeCurricular = context.UnidadeCurricular.Find(entidade.UnidadeCurricularId);
                entidade.UsuarioAtendente = context.Usuario.Find(entidade.UsuarioAtendenteId);
                entidade.UsuarioRequerente = context.Usuario.Find(entidade.UsuarioRequerenteId);

                context.FormularioValidacaoUC.Update(entidade);
                context.SaveChanges();
            }
        }
    }
}