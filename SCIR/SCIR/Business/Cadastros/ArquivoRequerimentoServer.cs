﻿using SCIR.Business.Requerimentos;
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
        private RequerimentoServer RequerimentoServer = new RequerimentoServer();

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

        private void ExcluirArquivos(string caminho)
        {
            File.Delete(caminho);
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

        public void Excluir(ArquivoRequerimento arquivoRequerimento)
        {
            var consiste = ConsisteExcluir(arquivoRequerimento);

            if (consiste.Inconsistencias.Any())
                throw new ArgumentException(consiste.Inconsistencias.ToString());
            else
            {
                var pesquisa = dbArquivoRequerimento.BuscarPorId(arquivoRequerimento.Id);
                var caminho = pesquisa.Caminho;
                dbArquivoRequerimento.Delete(pesquisa);
                ExcluirArquivos(caminho);
            }
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

        public ResponseGrid<ArquivoRequerimento> Listar(FormatGridUtils<ArquivoRequerimento> request, Usuario usuario)
        {
            var requerimentoRequest = new RequerimentoGridDC { Id = request.Entidade.RequerimentoId };
            var entityRequerimento = RequerimentoServer.GetRequerimentoId(requerimentoRequest, usuario); // Caso o usuário não ter permissão de visualizar os Arquivos irá gerar uma inconsistencia 

            request.Entidade.RequerimentoId = entityRequerimento.Id;
            var arquivos = dbArquivoRequerimento.ListGrid(request);
            
            var response = new ResponseGrid<ArquivoRequerimento>();
            response.Entidades = arquivos;
            response.QuantidadeRegistros = response.Entidades.TotalItemCount;

            return response;
        }
        
        public IList<ArquivoRequerimento> GetFiltroEntidadeString(string coluna, string searchTerm)
        {
            return dbArquivoRequerimento.FiltroPorColuna(coluna, searchTerm);
        }

        public ArquivoRequerimento GetArquivo(int id)
        {
            var arquivo = dbArquivoRequerimento.BuscarPorId(id);
            return arquivo;
        }

        public ConsisteUtils ExcluirPorStringList(string list)
        {
            var consiste = new ConsisteUtils();
            var spltArquivos = list.Split('|');
            foreach (var item in spltArquivos)
            {
                if (item != "")
                {
                    var arquivo = new ArquivoRequerimento
                    {
                        Id = Int32.Parse(item)
                    };

                    Excluir(arquivo);
                }
            }

            return consiste;
        }
    }
}