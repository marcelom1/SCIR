using Microsoft.EntityFrameworkCore;
using SCIR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCIR.DAO
{
    public class ScirContext : DbContext
    {
        public DbSet<Cursos> Cursos { get; set; }
        public DbSet<UnidadeCurricular> UnidadeCurricular { get; set; }
        public DbSet<TipoValidacaoCurricular> TipoValidacaoCurricular { get; set; }
        public DbSet<TipoFormulario> TipoFormulario { get; set; }
        public DbSet<TipoRequerimento> TipoRequerimento { get; set; }
        public DbSet<StatusRequerimento> StatusRequerimento { get; set; }
        public DbSet<Papel> Papel { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<FluxoStatus> FluxoStatus { get; set; }
        public DbSet<Requerimento> Requerimento { get; set; }
        public DbSet<FormularioValidacaoUC> FormularioValidacaoUC { get; set; }
        public DbSet<HistoricoRequerimento> HistoricoRequerimento { get; set; }
        public DbSet<ArquivoRequerimento> ArquivoRequerimento { get; set; }
        public DbSet<Auditoria> Auditoria { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("Server=localhost;User Id=root;Database=dbscir;Uid=root;Pwd=1475963m"); ;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TipoValidacaoCurricular>().HasData(new TipoValidacaoCurricular { Id = 1, Ativo = true, Nome = "Reconhecimento de Estudos" },
                                                                   new TipoValidacaoCurricular { Id = 2, Ativo = true, Nome = "Reconhecimento de Saberes" });

            modelBuilder.Entity<TipoFormulario>().HasData(new TipoFormulario { Id = 1, Nome = "Formulário Validação Unidade Curricular", Codigo = 1 });

            modelBuilder.Entity<Papel>().HasData(new Papel { Id = 1, Ativo = true, Nome = "Administrador" },
                                                 new Papel { Id = 2, Ativo = true, Nome = "Servidor" },
                                                 new Papel { Id = 3, Ativo = true, Nome = "Discente" });

            modelBuilder.Entity<StatusRequerimento>().HasData(new StatusRequerimento    {Id = 96, Ativo = true, Nome = "Deferido", Cancelamento = false, CodigoInterno = 1 },
                                                                 new StatusRequerimento {Id = 97, Ativo = true, Nome = "Indeferido", Cancelamento = false, CodigoInterno = 2 },
                                                                 new StatusRequerimento {Id = 98, Ativo = true, Nome = "Protocolado", Cancelamento = true, CodigoInterno = 3 },
                                                                 new StatusRequerimento {Id = 99, Ativo = true, Nome = "Aguardando Esclarecimento", Cancelamento = true, CodigoInterno = 4 });

            modelBuilder.Entity<FluxoStatus>()
                        .HasKey(c => new { c.StatusAtualId, c.StatusProximoId, c.TipoRequerimentoId });

     
            //modelBuilder.Entity<Usuario>().HasData(new Usuario { Id = 1, Ativo = true, Nome = "Administrador", Email = "marcelo.miglioli@hotmail.com", PapelId = 1, Senha = "123", Papel = Papel.Find(1) });

        }



        

    }
}