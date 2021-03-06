﻿using SCIR.DAO.Cadastros;
using SCIR.DAO.Formularios;
using SCIR.Datacontract.Grid;
using SCIR.Models;
using SCIR.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCIR.Business.Cadastros
{
    public class StatusRequerimentoServer
    {
        private StatusRequerimentoDao dbStatusRequerimento = new StatusRequerimentoDao();
        private FluxoStatusDao dbFluxoStatus = new FluxoStatusDao();
        private RequerimentoDao dbRequerimento = new RequerimentoDao();

        public ConsisteUtils ConsisteNovo(StatusRequerimento statusRequerimento)
        {
            var consiste = new ConsisteUtils();

            if (string.IsNullOrWhiteSpace(statusRequerimento.Nome))
                consiste.Add("O campo Nome não pode ficar em branco", ConsisteUtils.Tipo.Inconsistencia);

            return consiste;
        }

        public ConsisteUtils ConsisteExcluir(StatusRequerimento statusRequerimento)
        {
            var consiste = new ConsisteUtils();

            var pesquisa = dbStatusRequerimento.BuscarPorId(statusRequerimento.Id);
            statusRequerimento = pesquisa;

            if (statusRequerimento == null)
                consiste.Add("Não foi encontrado o registro para exclusão", ConsisteUtils.Tipo.Inconsistencia);

            var fluxoStatus = dbFluxoStatus.FiltroPorColuna(nameof(FluxoStatus.StatusAtualId), statusRequerimento.Id.ToString());
            if (fluxoStatus.Any())
                consiste.Add("Não foi possivel excluir o Status, pois o mesmo já se encontra atrelado a um fluxo de status (Fluxos de Status: " + string.Join(" - ", fluxoStatus) + ")", ConsisteUtils.Tipo.Inconsistencia);

            fluxoStatus = dbFluxoStatus.FiltroPorColuna(nameof(FluxoStatus.StatusProximoId), statusRequerimento.Id.ToString());
            if (fluxoStatus.Any())
                consiste.Add("Não foi possivel excluir o Status, pois o mesmo já se encontra atrelado a um fluxo de status (Fluxos de Status: " + string.Join(" - ", fluxoStatus) + ")", ConsisteUtils.Tipo.Inconsistencia);

            var requerimentos = dbRequerimento.FiltroPorColuna("STATUSREQUERIMENTO", statusRequerimento.Id.ToString());
            if (requerimentos.Any())
                consiste.Add("Não foi possivel excluir o Status, pois o mesmo já se encontra atrelado a um requerimento (Requerimento: " + string.Join(" - ", requerimentos) + ")", ConsisteUtils.Tipo.Inconsistencia);

            if (pesquisa.CodigoInterno != 0)
                consiste.Add("Não foi possivel excluir o Status, pois o mesmo é padrão do sistema", ConsisteUtils.Tipo.Inconsistencia);

            return consiste;
        }

        public ConsisteUtils ConsisteAtualizar(StatusRequerimento statusRequerimento)
        {
            var consiste = new ConsisteUtils();

            var pesquisa = dbStatusRequerimento.BuscarPorId(statusRequerimento.Id);
         
            if (pesquisa == null)
                consiste.Add("Não foi encontrado o registro para atualização", ConsisteUtils.Tipo.Inconsistencia);

            if (pesquisa.CodigoInterno != 0 && (statusRequerimento.Nome != pesquisa.Nome || statusRequerimento.CodigoInterno != pesquisa.CodigoInterno) )
                consiste.Add("Status Padrão do sistema não podem ser alterados", ConsisteUtils.Tipo.Inconsistencia);

            return consiste;
        }

        public StatusRequerimento Novo(StatusRequerimento statusRequerimento)
        {
            var consiste = ConsisteNovo(statusRequerimento);

            if (consiste.Inconsistencias.Any())
                throw new ArgumentException(consiste.Inconsistencias.ToString());
            else
                dbStatusRequerimento.Insert(statusRequerimento);


            return statusRequerimento;
        }

        public StatusRequerimento Excluir(StatusRequerimento statusRequerimento)
        {
            var consiste = ConsisteExcluir(statusRequerimento);

            if (consiste.Inconsistencias.Any())
                throw new ArgumentException(consiste.Inconsistencias.ToString());
            else
                dbStatusRequerimento.Delete(statusRequerimento);

            return statusRequerimento;
        }

        public StatusRequerimento Atualizar(StatusRequerimento statusRequerimento)
        {
            var consiste = ConsisteAtualizar(statusRequerimento);

            if (consiste.Inconsistencias.Any())
                throw new ArgumentException(consiste.Inconsistencias.ToString());
            else
                dbStatusRequerimento.Update(statusRequerimento);


            return statusRequerimento;
        }

        public ResponseGrid<StatusRequerimento> Listar(FormatGridUtils<StatusRequerimento> request)
        {
            var response = new ResponseGrid<StatusRequerimento>();
            response.Entidades = dbStatusRequerimento.ListGrid(request);
            response.QuantidadeRegistros = response.Entidades.TotalItemCount;

            return response;
        }

        public StatusRequerimento GetEntidade(int id)
        {
            return dbStatusRequerimento.BuscarPorId(id);
        }

        public StatusRequerimento GetEntidadeCodigoInterno(int id)
        {
            return StatusRequerimentoDao.BuscarPorCodigoInterno(id);
        }

        public IList<StatusRequerimento> GetFiltroEntidadeString(string coluna, string searchTerm)
        {
            return dbStatusRequerimento.FiltroPorColuna(coluna, searchTerm);
        }
    }
}