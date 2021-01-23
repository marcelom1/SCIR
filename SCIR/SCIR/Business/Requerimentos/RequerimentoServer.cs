using SCIR.Business.Cadastros;
using SCIR.DAO.Cadastros;
using SCIR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCIR.Business.Requerimentos
{
    public class RequerimentoServer
    {
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
    }
}