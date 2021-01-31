using SCIR.DAO.Cadastros;
using SCIR.Datacontract.Grid;
using SCIR.Models;
using SCIR.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCIR.Business.Cadastros
{
    public class FluxoStatusServer
    {
        private FluxoStatusDao dbFluxoStatus = new FluxoStatusDao();

        public ConsisteUtils ConsisteNovo(FluxoStatus fluxoStatus)
        {
            var consiste = new ConsisteUtils();

            var fluxoStatusBusca = dbFluxoStatus.GetEntidade(fluxoStatus);
            if (fluxoStatusBusca != null)
                consiste.Add("Já existe um fluxo cadastrado igual no sistema", ConsisteUtils.Tipo.Inconsistencia);

            if (fluxoStatus.StatusAtualId == 0)
                consiste.Add("Status Atual não pode ficar em branco", ConsisteUtils.Tipo.Inconsistencia);

            if (fluxoStatus.StatusProximoId == 0)
                consiste.Add("Proximo Status não pode ficar em branco", ConsisteUtils.Tipo.Inconsistencia);

            if (fluxoStatus.TipoRequerimentoId == 0)
                consiste.Add("O Tipo de Requerimento não pode ficar em branco", ConsisteUtils.Tipo.Inconsistencia);

            return consiste;
        }

        public ConsisteUtils ConsisteExcluirTodosProximos(FluxoStatus fluxoStatus)
        {
            var consiste = new ConsisteUtils();

            var pesquisa = dbFluxoStatus.BuscarPorId(fluxoStatus.StatusAtualId,fluxoStatus.TipoRequerimentoId);

            if (pesquisa == null)
                consiste.Add("Não foi encontrado o registro para exclusão", ConsisteUtils.Tipo.Inconsistencia);

            //foreach (var item in pesquisa)
            //{
                
            //}
           
            return consiste;
        }

        public ConsisteUtils ConsisteAtualizar(FluxoStatus fluxoStatus)
        {
            var consiste = new ConsisteUtils();

            var pesquisa = dbFluxoStatus.BuscarPorId(1);
            fluxoStatus = pesquisa;

            if (pesquisa == null)
                consiste.Add("Não foi encontrado o registro para atualização", ConsisteUtils.Tipo.Inconsistencia);

            var fluxoStatusBusca = dbFluxoStatus.GetEntidade(fluxoStatus);
            if (fluxoStatusBusca != null)
                consiste.Add("Já existe um fluxo cadastrado igual no sistema", ConsisteUtils.Tipo.Inconsistencia);

            if (fluxoStatus.StatusAtualId == 0)
                consiste.Add("Status Atual não pode ficar em branco", ConsisteUtils.Tipo.Inconsistencia);

            if (fluxoStatus.StatusProximoId == 0)
                consiste.Add("Proximo Status não pode ficar em branco", ConsisteUtils.Tipo.Inconsistencia);

            if (fluxoStatus.TipoRequerimentoId == 0)
                consiste.Add("O Tipo de Requerimento não pode ficar em branco", ConsisteUtils.Tipo.Inconsistencia);

            return consiste;
        }

        public ResponseGrid<FluxoStatusGridDC> ListarProximos(FormatGridUtils<FluxoStatus> request, int statusAtualId, int tipoRequerimentoId)
        {
            var response = new ResponseGrid<FluxoStatusGridDC>();
            response.Entidades = dbFluxoStatus.ListProximosGrid(request, statusAtualId, tipoRequerimentoId);
            response.QuantidadeRegistros = response.Entidades.TotalItemCount;

            return response;
        }

        public FluxoStatus Novo(FluxoStatus fluxoStatus)
        {
            var consiste = ConsisteNovo(fluxoStatus);

            if (consiste.Inconsistencias.Any())
                throw new ArgumentException(consiste.Inconsistencias.ToString());
            else
                dbFluxoStatus.Insert(fluxoStatus);


            return fluxoStatus;
        }

        public FluxoStatus Excluir(FluxoStatus fluxoStatus)
        {
            var consiste = ConsisteExcluir(fluxoStatus);

            if (consiste.Inconsistencias.Any())
                throw new ArgumentException(consiste.Inconsistencias.ToString());
            else
                dbFluxoStatus.Delete(fluxoStatus);

            return fluxoStatus;
        }

        public ConsisteUtils ConsisteExcluir(FluxoStatus fluxoStatus)
        {
            var consiste = new ConsisteUtils();

            var pesquisa = dbFluxoStatus.GetEntidade(fluxoStatus);

            if (pesquisa == null)
                consiste.Add("Não foi encontrado o registro para exclusão", ConsisteUtils.Tipo.Inconsistencia);

            //foreach (var item in pesquisa)
            //{

            //}

            return consiste;
        }

        public FluxoStatus ExcluirAll(FluxoStatus fluxoStatus)
        {
           var consiste = ConsisteExcluirTodosProximos(fluxoStatus);

            if (consiste.Inconsistencias.Any())
                throw new ArgumentException(consiste.Inconsistencias.ToString());

            //var pesquisa = dbFluxoStatus.BuscarPorId(fluxoStatus.StatusAtualId, fluxoStatus.TipoRequerimentoId);
            dbFluxoStatus.DeleteAll(fluxoStatus);

            return fluxoStatus;
        }

        public FluxoStatus Atualizar(FluxoStatus fluxoStatus)
        {
            var consiste = ConsisteAtualizar(fluxoStatus);

            if (consiste.Inconsistencias.Any())
                throw new ArgumentException(consiste.Inconsistencias.ToString());
            else
                dbFluxoStatus.Update(fluxoStatus);


            return fluxoStatus;
        }

        public ResponseGrid<FluxoStatusGridDC> Listar(FormatGridUtils<FluxoStatus> request)
        {
            var response = new ResponseGrid<FluxoStatusGridDC>();
            response.Entidades = dbFluxoStatus.ListGrid(request);
            response.QuantidadeRegistros = response.Entidades.TotalItemCount;

            return response;
        }

        public FluxoStatus GetEntidade(FluxoStatus fluxoStatus)
        {
            return dbFluxoStatus.GetEntidade(fluxoStatus);
        }

        public IList<FluxoStatus> GetFiltroEntidadeString(string coluna, string searchTerm)
        {
            return dbFluxoStatus.FiltroPorColuna(coluna, searchTerm);
        }

        public IList<FluxoStatusGridDC> GetProximoStatus(int statusAtualId, string searchTerm, int tipoRequerimentoId)
        {
            return dbFluxoStatus.ListProximos(searchTerm, statusAtualId, tipoRequerimentoId );
        }
    }
}