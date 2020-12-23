﻿using SCIR.DAO.Cadastros;
using SCIR.Datacontract.Grid;
using SCIR.Models;
using SCIR.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCIR.Business.Cadastros
{
    public class TipoRequerimentoServer
    {
        private TipoRequerimentoDao dbTipoRequerimento = new TipoRequerimentoDao();
        private TipoFormularioDao dbTipoFormulario = new TipoFormularioDao();

        public ConsisteUtils ConsisteNovo(TipoRequerimento tipoRequerimento)
        {
            var consiste = new ConsisteUtils();

            if (string.IsNullOrWhiteSpace(tipoRequerimento.Nome))
                consiste.Add("O campo Nome não pode ficar em branco", ConsisteUtils.Tipo.Inconsistencia);

            return consiste;
        }

        public ConsisteUtils ConsisteExcluir(TipoRequerimento tipoRequerimento)
        {
            var consiste = new ConsisteUtils();

            if (tipoRequerimento == null)
                consiste.Add("Não foi encontrado o registro para exclusão", ConsisteUtils.Tipo.Inconsistencia);

            return consiste;
        }

        public ConsisteUtils ConsisteAtualizar(TipoRequerimento tipoRequerimento)
        {
            var consiste = new ConsisteUtils();

            var pesquisa = dbTipoRequerimento.BuscarPorId(tipoRequerimento.Id);
            tipoRequerimento = pesquisa;

            if (pesquisa == null)
                consiste.Add("Não foi encontrado o registro para atualização", ConsisteUtils.Tipo.Inconsistencia);

            return consiste;
        }

        public TipoRequerimento Novo(TipoRequerimento tipoRequerimento)
        {
            var consiste = ConsisteNovo(tipoRequerimento);

            if (consiste.Inconsistencias.Any())
                throw new ArgumentException(consiste.Inconsistencias.ToString());
            else
                dbTipoRequerimento.Insert(tipoRequerimento);


            return tipoRequerimento;
        }

        public TipoRequerimento Excluir(TipoRequerimento tipoRequerimento)
        {
            var pesquisa = dbTipoRequerimento.BuscarPorId(tipoRequerimento.Id);
            tipoRequerimento = pesquisa;

            var consiste = ConsisteExcluir(tipoRequerimento);

            if (consiste.Inconsistencias.Any())
                throw new ArgumentException(consiste.Inconsistencias.ToString());
            else
                dbTipoRequerimento.Delete(tipoRequerimento);

            return tipoRequerimento;
        }

        public TipoRequerimento Atualizar(TipoRequerimento tipoRequerimento)
        {
            var consiste = ConsisteAtualizar(tipoRequerimento);

            if (consiste.Inconsistencias.Any())
                throw new ArgumentException(consiste.Inconsistencias.ToString());
            else
                dbTipoRequerimento.Update(tipoRequerimento);


            return tipoRequerimento;
        }

        public TipoRequerimento GetEntidade(int id)
        {
            return dbTipoRequerimento.BuscarPorId(id);
        }

        public ResponseGrid<TipoRequerimentoGridDC> Listar(FormatGridUtils request)
        {
            var response = new ResponseGrid<TipoRequerimentoGridDC>();
            response.Entidades = dbTipoRequerimento.ListGrid(request);
            response.QuantidadeRegistros = response.Entidades.TotalItemCount;

            return response;
        }

        public IList<TipoFormulario> GetFiltroTipoFormularioString(string coluna, string searchTerm)
        {
            return dbTipoFormulario.FiltroPorColuna(coluna, searchTerm);
        }
    }
}