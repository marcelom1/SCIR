using SCIR.DAO.Cadastros;
using SCIR.Datacontract.Grid;
using SCIR.Models;
using SCIR.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace SCIR.Business.Cadastros
{
    public class ArquivoRequerimentoServer
    {
        private ArquivoRequerimentoDao dbArquivoRequerimento = new ArquivoRequerimentoDao();

        public ConsisteUtils ConsisteNovo(ArquivoRequerimento arquivoRequerimento)
        {
            var consiste = new ConsisteUtils();

            return consiste;
        }

        private IList<ArquivoRequerimento> SalvarArquivos(HttpFileCollectionBase files, HttpServerUtilityBase server, string rootPatch, Requerimento requerimento)
        {
            var listArquivos = new List<ArquivoRequerimento>();
            string[] path = new string[files.Count];
            for (int i = 0; i < files.Count; i++)
            {
                var file = files[i];
                var root = rootPatch + "/" + file.FileName;
                Directory.CreateDirectory(server.MapPath(rootPatch));
                path[i] = rootPatch.Substring(1);
                file.SaveAs(server.MapPath(root));
                listArquivos.Add(new ArquivoRequerimento { Caminho = server.MapPath(root), Nome = file.FileName, RequerimentoId = requerimento.Id, Requerimento = requerimento});

            }

            return listArquivos;
            
        }


        public ConsisteUtils ConsisteAtualizar(ArquivoRequerimento arquivoRequerimento)
        {
            var consiste = new ConsisteUtils();

            return consiste;
        }


        public IList<ArquivoRequerimento> Novo(Requerimento requerimento, HttpFileCollectionBase files, HttpServerUtilityBase server)
        {
            var arquivosRequerimento = SalvarArquivos(files,server, "~/ArquivosRequerimento/"+ DateTime.Now.ToString("yyyy") + "/" + requerimento.TipoRequerimento.Sigla + "/" + requerimento.Protocolo.Replace("/", ""), requerimento);

            foreach (var arquivoRequerimento in arquivosRequerimento)
            {
                var consiste = ConsisteNovo(arquivoRequerimento);

                if (consiste.Inconsistencias.Any())
                    throw new ArgumentException(consiste.Inconsistencias.ToString());
                else
                    dbArquivoRequerimento.Insert(arquivoRequerimento);
            }
         
            return arquivosRequerimento;
        }

        public ArquivoRequerimento Excluir(ArquivoRequerimento arquivoRequerimento)
        {
            var consiste = ConsisteExcluir(arquivoRequerimento);

            if (consiste.Inconsistencias.Any())
                throw new ArgumentException(consiste.Inconsistencias.ToString());
            else
                dbArquivoRequerimento.Delete(arquivoRequerimento);

            return arquivoRequerimento;
        }

        public ConsisteUtils ConsisteExcluir(ArquivoRequerimento arquivoRequerimento)
        {
            var consiste = new ConsisteUtils();

            return consiste;
        }


        public ArquivoRequerimento Atualizar(ArquivoRequerimento arquivoRequerimento)
        {
            var consiste = ConsisteAtualizar(arquivoRequerimento);

            if (consiste.Inconsistencias.Any())
                throw new ArgumentException(consiste.Inconsistencias.ToString());
            else
                dbArquivoRequerimento.Update(arquivoRequerimento);


            return arquivoRequerimento;
        }

        public ResponseGrid<ArquivoRequerimento> Listar(FormatGridUtils<ArquivoRequerimento> request)
        {
            var response = new ResponseGrid<ArquivoRequerimento>();
            response.Entidades = dbArquivoRequerimento.ListGrid(request);
            response.QuantidadeRegistros = response.Entidades.TotalItemCount;

            return response;
        }
        

        public IList<ArquivoRequerimento> GetFiltroEntidadeString(string coluna, string searchTerm)
        {
            return dbArquivoRequerimento.FiltroPorColuna(coluna, searchTerm);
        }
    }
}