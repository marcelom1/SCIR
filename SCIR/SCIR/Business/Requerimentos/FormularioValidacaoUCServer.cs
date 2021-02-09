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
    public class FormularioValidacaoUCServer : IAuditoriaRequerimento<FormularioValidacaoUC>
    {
        private FormularioValidacaoUCDao dbFormularioValidacaoUC = new FormularioValidacaoUCDao();
        private ArquivoRequerimentoServer ArquivosRequerimentoServer = new ArquivoRequerimentoServer();
        private StatusRequerimentoServer StatusRequerimentoServer = new StatusRequerimentoServer();

        public ConsisteUtils ConsisteNovo(FormularioValidacaoUC formularioValidacaoUC)
        {
            var consiste = new ConsisteUtils();

            if (string.IsNullOrWhiteSpace(formularioValidacaoUC.Motivo))
                consiste.Add("O campo Motivo não pode ficar em branco", ConsisteUtils.Tipo.Inconsistencia);

            if (formularioValidacaoUC.TipoValidacaoCurricularId == 0)
                consiste.Add("O campo Tipo Validação não pode ficar em branco", ConsisteUtils.Tipo.Inconsistencia);

            if (formularioValidacaoUC.UnidadeCurricularId == 0)
                consiste.Add("O campo Unidade Curricular não pode ficar em branco", ConsisteUtils.Tipo.Inconsistencia);

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

            if (string.IsNullOrWhiteSpace(formularioValidacaoUC.Motivo))
                consiste.Add("O campo Motivo não pode ficar em branco", ConsisteUtils.Tipo.Inconsistencia);

            if (formularioValidacaoUC.TipoValidacaoCurricularId == 0)
                consiste.Add("O campo Tipo Validação não pode ficar em branco", ConsisteUtils.Tipo.Inconsistencia);

            if (formularioValidacaoUC.UnidadeCurricularId == 0)
                consiste.Add("O campo Unidade Curricular não pode ficar em branco", ConsisteUtils.Tipo.Inconsistencia);

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
                formularioValidacaoUC.StatusRequerimentoId = StatusRequerimentoServer.GetEntidadeCodigoInterno(3).Id;
                formularioValidacaoUC.UsuarioAtendenteId = RequerimentoServer.PrimeiroAtendimento(formularioValidacaoUC);
                dbFormularioValidacaoUC.Insert(formularioValidacaoUC);
                ArquivosRequerimentoServer.Novo(formularioValidacaoUC, files, server);
                GerarAuditoria(formularioValidacaoUC, formularioValidacaoUC, AuditoriaServer.TipoAuditoria.Insert);

            }

            return formularioValidacaoUC;
        }

        public FormularioValidacaoUC Excluir(FormularioValidacaoUC formularioValidacaoUC)
        {
            var consiste = ConsisteExcluir(formularioValidacaoUC);

            if (consiste.Inconsistencias.Any())
                throw new ArgumentException(consiste.Inconsistencias.ToString());
            else
            {
                var pesquisa = GetEntidade(formularioValidacaoUC.Id);
                dbFormularioValidacaoUC.Delete(formularioValidacaoUC);
                GerarAuditoria(pesquisa, formularioValidacaoUC, AuditoriaServer.TipoAuditoria.Delete);
            }

            return formularioValidacaoUC;
        }

        public FormularioValidacaoUC Atualizar(FormularioValidacaoUC formularioValidacaoUC, HttpFileCollectionBase files)
        {
            var consiste = ConsisteAtualizar(formularioValidacaoUC);

            if (consiste.Inconsistencias.Any())
                throw new ArgumentException(consiste.Inconsistencias.ToString());
            else
            {
                var pesquisa = GetEntidade(formularioValidacaoUC.Id);
                dbFormularioValidacaoUC.Update(formularioValidacaoUC);
                GerarAuditoria(pesquisa, formularioValidacaoUC,AuditoriaServer.TipoAuditoria.Update);
            }

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

        public AuditoriaServer GerarAuditoria(FormularioValidacaoUC requerimentoAntes, FormularioValidacaoUC requerimentoDepois, AuditoriaServer.TipoAuditoria tipo)
        {
            var auditoria = RequerimentoServer.GerarCamposAuditoria(requerimentoAntes, requerimentoDepois, tipo);

            if (requerimentoAntes.Nome != requerimentoDepois.Nome)
                auditoria.IncluirAuditoriaEntidade(requerimentoDepois, "NOME", requerimentoAntes.Nome, requerimentoDepois.Nome);

            if (requerimentoAntes.Motivo != requerimentoDepois.Motivo)
                auditoria.IncluirAuditoriaEntidade(requerimentoDepois, "MOTIVO", requerimentoAntes.Motivo, requerimentoDepois.Motivo);

            if (requerimentoAntes.UnidadeCurricularId != requerimentoDepois.UnidadeCurricularId)
                auditoria.IncluirAuditoriaEntidade(requerimentoDepois, "USUÁRIO REQUERENTE", requerimentoAntes.UnidadeCurricularId + " - " + requerimentoAntes.UnidadeCurricular.Nome, requerimentoDepois.UnidadeCurricularId + " - " + requerimentoDepois.UnidadeCurricular.Nome);

            if (requerimentoAntes.TipoValidacaoCurricularId != requerimentoDepois.TipoValidacaoCurricularId)
                auditoria.IncluirAuditoriaEntidade(requerimentoDepois, "TIPO VALIDÇÃO CURRICULAR", requerimentoAntes.TipoValidacaoCurricularId + " - " + requerimentoAntes.TipoValidacaoCurricular.Nome, requerimentoDepois.TipoValidacaoCurricularId + " - " + requerimentoDepois.TipoValidacaoCurricular.Nome);

            auditoria.EnviarEmail(tipo);
            return auditoria;
        }
    }
}
