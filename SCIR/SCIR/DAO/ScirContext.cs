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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("Server=localhost;User Id=root;Database=dbscir;Uid=root;Pwd=1475963m"); ;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TipoValidacaoCurricular>().HasData(new TipoValidacaoCurricular { Id = 1, Ativo = true, Nome = "Reconhecimento de Estudos" },
                                                                   new TipoValidacaoCurricular { Id = 2, Ativo = true, Nome = "Reconhecimento de Saberes" });

            modelBuilder.Entity<TipoFormulario>().HasData(new TipoFormulario { Id = 1, Nome = "Formulário Validação Unidade Curricular" });
        }

        

    }
}