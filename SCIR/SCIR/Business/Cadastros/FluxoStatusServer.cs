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

            //if (string.IsNullOrWhiteSpace(fluxoStatus.Nome))
            //    consiste.Add("O campo Nome não pode ficar em branco", ConsisteUtils.Tipo.Inconsistencia);

            return consiste;
        }

        public ConsisteUtils ConsisteExcluir(FluxoStatus fluxoStatus)
        {
            var consiste = new ConsisteUtils();

            var pesquisa = dbFluxoStatus.BuscarPorId(1);
            fluxoStatus = pesquisa;

            if (fluxoStatus == null)
                consiste.Add("Não foi encontrado o registro para exclusão", ConsisteUtils.Tipo.Inconsistencia);

            return consiste;
        }

        public ConsisteUtils ConsisteAtualizar(FluxoStatus fluxoStatus)
        {
            var consiste = new ConsisteUtils();

            var pesquisa = dbFluxoStatus.BuscarPorId(1);
            fluxoStatus = pesquisa;

            if (pesquisa == null)
                consiste.Add("Não foi encontrado o registro para atualização", ConsisteUtils.Tipo.Inconsistencia);

            return consiste;
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

        public FluxoStatus Atualizar(FluxoStatus fluxoStatus)
        {
            var consiste = ConsisteAtualizar(fluxoStatus);

            if (consiste.Inconsistencias.Any())
                throw new ArgumentException(consiste.Inconsistencias.ToString());
            else
                dbFluxoStatus.Update(fluxoStatus);


            return fluxoStatus;
        }

        public ResponseGrid<FluxoStatusGridDC> Listar(FormatGridUtils request)
        {
            var response = new ResponseGrid<FluxoStatusGridDC>();
            response.Entidades = dbFluxoStatus.ListGrid(request);
            response.QuantidadeRegistros = response.Entidades.TotalItemCount;

            return response;
        }

        public FluxoStatus GetEntidade(int id)
        {
            return dbFluxoStatus.BuscarPorId(id);
        }

        public IList<FluxoStatus> GetFiltroEntidadeString(string coluna, string searchTerm)
        {
            return dbFluxoStatus.FiltroPorColuna(coluna, searchTerm);
        }
    }
}