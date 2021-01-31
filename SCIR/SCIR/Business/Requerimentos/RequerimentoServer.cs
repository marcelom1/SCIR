using SCIR.Business.Cadastros;
using SCIR.DAO.Cadastros;
using SCIR.DAO.Formularios;
using SCIR.Datacontract.Grid;
using SCIR.Models;
using SCIR.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCIR.Business.Requerimentos
{
    public class RequerimentoServer
    {
        private RequerimentoDao dbRequerimento = new RequerimentoDao();

        public static string GerarNovoProtocolo(Requerimento requerimento)
        {
            var ServerTipoRequerimento = new TipoRequerimentoServer();
            var NovoProtocolo = "";
            var tipoRequerimento = ServerTipoRequerimento.GetEntidade(requerimento.TipoRequerimentoId);
            var sigla = tipoRequerimento.Sigla.ToUpper();
            if (string.IsNullOrWhiteSpace(sigla))
                throw new Exception("Sigla não cadastrada para esse tipo de requerimento");
            var sequenciaText = new string[] { "0000","00"};
            if (tipoRequerimento.SequenciaProtocolo != null)
                sequenciaText = tipoRequerimento.SequenciaProtocolo.Split('/');
            var sequenciaAno = "";
            int sequencia = 0;
            if (int.TryParse(sequenciaText[0], out sequencia))
            {
                if (DateTime.Now.ToString("yy") != sequenciaText[1])
                {
                    sequencia = 0;
                }

                sequenciaAno = (sequencia + 1).ToString("D4") + "/" + DateTime.Now.ToString("yy");
                NovoProtocolo += sigla + sequenciaAno;
            }
           
            if (string.IsNullOrWhiteSpace(NovoProtocolo))
                throw new Exception("Não foi possivel determinar a próxima sequencia válida do protocolo");

            tipoRequerimento.SequenciaProtocolo = sequenciaAno;
            ServerTipoRequerimento.Atualizar(tipoRequerimento);
            return NovoProtocolo;

        }

        public static int PrimeiroAtendimento(Requerimento requerimento)
        {
            var ServerTipoRequerimento = new TipoRequerimentoServer();
            var tipoRequerimento = ServerTipoRequerimento.GetEntidade(requerimento.TipoRequerimentoId);
            return tipoRequerimento.PrimeiroAtendimentoId;
        }

        public ResponseGrid<RequerimentoGridDC> ListarTudo(FormatGridUtils<Requerimento> request)
        {
            var response = new ResponseGrid<RequerimentoGridDC>();
            response.Entidades = dbRequerimento.ListGrid(request,false,false);
            response.QuantidadeRegistros = response.Entidades.TotalItemCount;

            return response;
        }

        public ResponseGrid<RequerimentoGridDC> ListarPorAtendente(FormatGridUtils<Requerimento> request)
        {
            var response = new ResponseGrid<RequerimentoGridDC>();
            response.Entidades = dbRequerimento.ListGrid(request, true, false);
            response.QuantidadeRegistros = response.Entidades.TotalItemCount;

            return response;
        }

        public ResponseGrid<RequerimentoGridDC> ListarPorRequerente(FormatGridUtils<Requerimento> request)
        {
            var response = new ResponseGrid<RequerimentoGridDC>();
            response.Entidades = dbRequerimento.ListGrid(request, false, true);
            response.QuantidadeRegistros = response.Entidades.TotalItemCount;

            return response;
        }

        public ResponseGrid<RequerimentoGridDC> ListarPorRequerenteOuAtendente(FormatGridUtils<Requerimento> request)
        {
            var response = new ResponseGrid<RequerimentoGridDC>();
            response.Entidades = dbRequerimento.ListGrid(request, true, true);
            response.QuantidadeRegistros = response.Entidades.TotalItemCount;

            return response;
        }

        public Requerimento GetRequerimentoId(Requerimento requerimento, Usuario usuario)
        {
            var entityRequerimento = dbRequerimento.GetRequerimentoId(requerimento);
            if (usuario.Id == entityRequerimento.UsuarioRequerenteId || usuario.Id == entityRequerimento.UsuarioAtendenteId || usuario.PapelId == (int)PapelDao.PapelUsuario.Administrador)
                return entityRequerimento;

            throw new Exception("Usuário não tem permissão de visualizar o requerimento, Protocolo: " + entityRequerimento.Protocolo);
        }


    }
}