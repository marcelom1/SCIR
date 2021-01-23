using SCIR.Business.Cadastros;
using SCIR.DAO.Cadastros;
using SCIR.DAO.Formularios;
using SCIR.Datacontract.Grid;
using SCIR.Models;
using SCIR.Utils;
using SCIR.Utils.TipoFormularioUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCIR.Business.Requerimentos
{
    public class FormularioValidacaoUCServer
    {
        private FormularioValidacaoUCDao dbFormularioValidacaoUC = new FormularioValidacaoUCDao();
        private ArquivoRequerimentoServer ArquivosRequerimentoServer = new ArquivoRequerimentoServer();

        public ConsisteUtils ConsisteNovo(FormularioValidacaoUC formularioValidacaoUC)
        {
            var consiste = new ConsisteUtils();

            if (string.IsNullOrWhiteSpace(formularioValidacaoUC.Motivo))
                consiste.Add("O campo Motivo não pode ficar em branco", ConsisteUtils.Tipo.Inconsistencia);

            return consiste;
        }

        public ConsisteUtils ConsisteExcluir(FormularioValidacaoUC formularioValidacaoUC)
        {
            var consiste = new ConsisteUtils();

            var pesquisa = dbFormularioValidacaoUC.BuscarPorId(formularioValidacaoUC.Id);
            formularioValidacaoUC = pesquisa;

            if (formularioValidacaoUC == null)
                consiste.Add("Não foi encontrado o registro para exclusão", ConsisteUtils.Tipo.Inconsistencia);

            return consiste;
        }

        public ConsisteUtils ConsisteAtualizar(FormularioValidacaoUC formularioValidacaoUC)
        {
            var consiste = new ConsisteUtils();

            var pesquisa = dbFormularioValidacaoUC.BuscarPorId(formularioValidacaoUC.Id);
            formularioValidacaoUC = pesquisa;

            if (pesquisa == null)
                consiste.Add("Não foi encontrado o registro para atualização", ConsisteUtils.Tipo.Inconsistencia);

            return consiste;
        }

        public FormularioValidacaoUC Novo(FormularioValidacaoUC formularioValidacaoUC, HttpFileCollectionBase files, HttpServerUtilityBase server)
        {
            var consiste = ConsisteNovo(formularioValidacaoUC);

            if (consiste.Inconsistencias.Any())
                throw new ArgumentException(consiste.Inconsistencias.ToString());
            else
            {
                formularioValidacaoUC.Abertura = DateTime.Now;
                formularioValidacaoUC.Protocolo = RequerimentoServer.GerarNovoProtocolo(formularioValidacaoUC);
                formularioValidacaoUC.TipoFormularioId = TipoFormularioUtils.FormlarioEnum.ValidacaoUC.GetHashCode();
                dbFormularioValidacaoUC.Insert(formularioValidacaoUC);
                ArquivosRequerimentoServer.Novo(formularioValidacaoUC, files, server);

            }

            return formularioValidacaoUC;
        }

        public FormularioValidacaoUC Excluir(FormularioValidacaoUC formularioValidacaoUC)
        {
            var consiste = ConsisteExcluir(formularioValidacaoUC);

            if (consiste.Inconsistencias.Any())
                throw new ArgumentException(consiste.Inconsistencias.ToString());
            else
                dbFormularioValidacaoUC.Delete(formularioValidacaoUC);

            return formularioValidacaoUC;
        }

        public FormularioValidacaoUC Atualizar(FormularioValidacaoUC formularioValidacaoUC, HttpFileCollectionBase files)
        {
            var consiste = ConsisteAtualizar(formularioValidacaoUC);

            if (consiste.Inconsistencias.Any())
                throw new ArgumentException(consiste.Inconsistencias.ToString());
            else
                dbFormularioValidacaoUC.Update(formularioValidacaoUC);


            return formularioValidacaoUC;
        }

        public ResponseGrid<FormularioValidacaoUC> Listar(FormatGridUtils<FormularioValidacaoUC> request)
        {
            var response = new ResponseGrid<FormularioValidacaoUC>();
            response.Entidades = dbFormularioValidacaoUC.ListGrid(request);
            response.QuantidadeRegistros = response.Entidades.TotalItemCount;

            return response;
        }

        public FormularioValidacaoUC GetEntidade(int id)
        {
            return dbFormularioValidacaoUC.BuscarPorId(id);
        }

        public IList<FormularioValidacaoUC> GetFiltroEntidadeString(string coluna, string searchTerm)
        {
            return dbFormularioValidacaoUC.FiltroPorColuna(coluna, searchTerm);
        }

        
    }
}
