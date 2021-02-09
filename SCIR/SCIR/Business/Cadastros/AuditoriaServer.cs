using SCIR.DAO.Cadastros;
using SCIR.Datacontract.Grid;
using SCIR.Models;
using SCIR.Models.ViewModels;
using SCIR.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCIR.Business.Cadastros
{
    public class AuditoriaServer
    {
        private AuditoriaDao dbAuditoria = new AuditoriaDao();
        private string MsgEmail = "";
        private Requerimento Requerimento;

        public enum TipoAuditoria
        {
            Insert,
            Update,
            Delete
        }

        public AuditoriaServer()
        {
            Requerimento = new RequerimentoVM();
        }

        public AuditoriaServer(Requerimento requerimento)
        {
            Requerimento = requerimento;
        }

        public void IncluirAuditoriaEntidade(Requerimento requerimento, string campo, string campoValorAntes = "", string campoValorDepois = "")
        {
            MsgEmail += campo.ToUpper() + " Antes: " + campoValorAntes + " - " + "Depois: " + campoValorDepois +"|";
            IncluirAuditoria(requerimento, campo, campoValorAntes, campoValorDepois);
        }

        public void EnviarEmail(TipoAuditoria tipo)
        {
            
        }

        public static void IncluirAuditoria (Requerimento requerimento, string campo, string campoValorAntes, string campoValorDepois)
        {
            var consiste = new ConsisteUtils();

            var novaAuditoria = new Auditoria
            {
                RequerimentoId = requerimento.Id,
                DataModificacao = DateTime.Now,
                Campo = campo,
                Antes = campoValorAntes,
                Depois = campoValorDepois
            };

            Novo(novaAuditoria);
        }
        public static ConsisteUtils ConsisteNovo(Auditoria entidade)
        {
            var consiste = new ConsisteUtils();

            return consiste;
        }

        public ConsisteUtils ConsisteExcluir(Auditoria entidade)
        {
            var consiste = new ConsisteUtils();

            var pesquisa = dbAuditoria.BuscarPorId(entidade.Id);
            entidade = pesquisa;

            if (pesquisa == null)
                consiste.Add("Não foi encontrado o registro para exclusão", ConsisteUtils.Tipo.Inconsistencia);

            return consiste;
        }

        public ConsisteUtils ConsisteAtualizar(Auditoria entidade)
        {
            var consiste = new ConsisteUtils();

            var pesquisa = dbAuditoria.BuscarPorId(entidade.Id);
            entidade = pesquisa;

            if (pesquisa == null)
                consiste.Add("Não foi encontrado o registro para atualização", ConsisteUtils.Tipo.Inconsistencia);

            return consiste;
        }

        private static Auditoria Novo(Auditoria entidade)
        {
            var consiste = ConsisteNovo(entidade);
            var dbAuditoria = new AuditoriaDao();

            if (consiste.Inconsistencias.Any())
                throw new ArgumentException(consiste.Inconsistencias.ToString());
            else
                dbAuditoria.Insert(entidade);


            return entidade;
        }

        private Auditoria Excluir(Auditoria entidade)
        {
            var consiste = ConsisteExcluir(entidade);

            if (consiste.Inconsistencias.Any())
                throw new ArgumentException(consiste.Inconsistencias.ToString());
            else
                dbAuditoria.Delete(entidade);

            return entidade;
        }

        private Auditoria Atualizar(Auditoria entidade)
        {
            var consiste = ConsisteAtualizar(entidade);

            if (consiste.Inconsistencias.Any())
                throw new ArgumentException(consiste.Inconsistencias.ToString());
            else
                dbAuditoria.Update(entidade);


            return entidade;
        }

        public ResponseGrid<Auditoria> Listar(FormatGridUtils<Auditoria> request)
        {
            var response = new ResponseGrid<Auditoria>();
            response.Entidades = dbAuditoria.ListGrid(request);
            response.QuantidadeRegistros = response.Entidades.TotalItemCount;

            return response;
        }

        public Auditoria GetEntidade(int id)
        {
            return dbAuditoria.BuscarPorId(id);
        }

        public IList<Auditoria> GetFiltroEntidadeString(string coluna, string searchTerm)
        {
            return dbAuditoria.FiltroPorColuna(coluna, searchTerm);
        }

    }
}