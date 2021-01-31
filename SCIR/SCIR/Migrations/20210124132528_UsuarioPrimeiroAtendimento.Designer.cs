﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SCIR.DAO;

namespace SCIR.Migrations
{
    [DbContext(typeof(ScirContext))]
    [Migration("20210124132528_UsuarioPrimeiroAtendimento")]
    partial class UsuarioPrimeiroAtendimento
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("SCIR.Models.ArquivoRequerimento", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Caminho")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Nome")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("RequerimentoId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RequerimentoId");

                    b.ToTable("ArquivoRequerimento");
                });

            modelBuilder.Entity("SCIR.Models.Cursos", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("Ativo")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Cursos");
                });

            modelBuilder.Entity("SCIR.Models.FluxoStatus", b =>
                {
                    b.Property<int>("StatusAtualId")
                        .HasColumnType("int");

                    b.Property<int>("StatusProximoId")
                        .HasColumnType("int");

                    b.Property<int>("TipoRequerimentoId")
                        .HasColumnType("int");

                    b.HasKey("StatusAtualId", "StatusProximoId", "TipoRequerimentoId");

                    b.HasIndex("StatusProximoId");

                    b.HasIndex("TipoRequerimentoId");

                    b.ToTable("FluxoStatus");
                });

            modelBuilder.Entity("SCIR.Models.HistoricoRequerimento", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Antes")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Depois")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("Modificado")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("RequerimentoId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RequerimentoId");

                    b.ToTable("HistoricoRequerimento");
                });

            modelBuilder.Entity("SCIR.Models.Papel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("Ativo")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Papel");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Ativo = true,
                            Nome = "Administrador"
                        },
                        new
                        {
                            Id = 2,
                            Ativo = true,
                            Nome = "Servidor"
                        },
                        new
                        {
                            Id = 3,
                            Ativo = true,
                            Nome = "Discente"
                        });
                });

            modelBuilder.Entity("SCIR.Models.Requerimento", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("Abertura")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("Encerramento")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Mensagem")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Protocolo")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("StatusRequerimentoId")
                        .HasColumnType("int");

                    b.Property<int>("TipoFormularioId")
                        .HasColumnType("int");

                    b.Property<int>("TipoRequerimentoId")
                        .HasColumnType("int");

                    b.Property<int>("UsuarioAtendenteId")
                        .HasColumnType("int");

                    b.Property<int>("UsuarioRequerenteId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StatusRequerimentoId");

                    b.HasIndex("TipoFormularioId");

                    b.HasIndex("TipoRequerimentoId");

                    b.HasIndex("UsuarioAtendenteId");

                    b.HasIndex("UsuarioRequerenteId");

                    b.ToTable("Requerimento");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Requerimento");
                });

            modelBuilder.Entity("SCIR.Models.StatusRequerimento", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("Ativo")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("Cancelamento")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("CodigoInterno")
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("StatusRequerimento");

                    b.HasData(
                        new
                        {
                            Id = 96,
                            Ativo = true,
                            Cancelamento = false,
                            CodigoInterno = 1,
                            Nome = "Deferido"
                        },
                        new
                        {
                            Id = 97,
                            Ativo = true,
                            Cancelamento = false,
                            CodigoInterno = 2,
                            Nome = "Indeferido"
                        },
                        new
                        {
                            Id = 98,
                            Ativo = true,
                            Cancelamento = true,
                            CodigoInterno = 3,
                            Nome = "Protocolado"
                        },
                        new
                        {
                            Id = 99,
                            Ativo = true,
                            Cancelamento = true,
                            CodigoInterno = 4,
                            Nome = "Aguardando Esclarecimento"
                        });
                });

            modelBuilder.Entity("SCIR.Models.TipoFormulario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("Codigo")
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("TipoFormulario");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Codigo = 1,
                            Nome = "Formulário Validação Unidade Curricular"
                        });
                });

            modelBuilder.Entity("SCIR.Models.TipoRequerimento", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("Ativo")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("PrimeiroAtendimentoId")
                        .HasColumnType("int");

                    b.Property<string>("SequenciaProtocolo")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Sigla")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("TipoFormularioId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PrimeiroAtendimentoId");

                    b.HasIndex("TipoFormularioId");

                    b.ToTable("TipoRequerimento");
                });

            modelBuilder.Entity("SCIR.Models.TipoValidacaoCurricular", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("Ativo")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("TipoValidacaoCurricular");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Ativo = true,
                            Nome = "Reconhecimento de Estudos"
                        },
                        new
                        {
                            Id = 2,
                            Ativo = true,
                            Nome = "Reconhecimento de Saberes"
                        });
                });

            modelBuilder.Entity("SCIR.Models.UnidadeCurricular", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("Ativo")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("CursoId")
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("CursoId");

                    b.ToTable("UnidadeCurricular");
                });

            modelBuilder.Entity("SCIR.Models.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("Ativo")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Email")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("PapelId")
                        .HasColumnType("int");

                    b.Property<string>("Senha")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("PapelId");

                    b.ToTable("Usuario");
                });

            modelBuilder.Entity("SCIR.Models.FormularioValidacaoUC", b =>
                {
                    b.HasBaseType("SCIR.Models.Requerimento");

                    b.Property<string>("Motivo")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Nome")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("TipoValidacaoCurricularId")
                        .HasColumnType("int");

                    b.Property<int>("UnidadeCurricularId")
                        .HasColumnType("int");

                    b.HasIndex("TipoValidacaoCurricularId");

                    b.HasIndex("UnidadeCurricularId");

                    b.HasDiscriminator().HasValue("FormularioValidacaoUC");
                });

            modelBuilder.Entity("SCIR.Models.ArquivoRequerimento", b =>
                {
                    b.HasOne("SCIR.Models.Requerimento", "Requerimento")
                        .WithMany()
                        .HasForeignKey("RequerimentoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SCIR.Models.FluxoStatus", b =>
                {
                    b.HasOne("SCIR.Models.StatusRequerimento", "StatusAtual")
                        .WithMany()
                        .HasForeignKey("StatusAtualId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SCIR.Models.StatusRequerimento", "StatusProximo")
                        .WithMany()
                        .HasForeignKey("StatusProximoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SCIR.Models.TipoRequerimento", "TipoRequerimento")
                        .WithMany()
                        .HasForeignKey("TipoRequerimentoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SCIR.Models.HistoricoRequerimento", b =>
                {
                    b.HasOne("SCIR.Models.Requerimento", "Requerimento")
                        .WithMany()
                        .HasForeignKey("RequerimentoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SCIR.Models.Requerimento", b =>
                {
                    b.HasOne("SCIR.Models.StatusRequerimento", "StatusRequerimento")
                        .WithMany()
                        .HasForeignKey("StatusRequerimentoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SCIR.Models.TipoFormulario", "TipoFormulario")
                        .WithMany()
                        .HasForeignKey("TipoFormularioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SCIR.Models.TipoRequerimento", "TipoRequerimento")
                        .WithMany()
                        .HasForeignKey("TipoRequerimentoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SCIR.Models.Usuario", "UsuarioAtendente")
                        .WithMany()
                        .HasForeignKey("UsuarioAtendenteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SCIR.Models.Usuario", "UsuarioRequerente")
                        .WithMany()
                        .HasForeignKey("UsuarioRequerenteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SCIR.Models.TipoRequerimento", b =>
                {
                    b.HasOne("SCIR.Models.Usuario", "PrimeiroAtendimento")
                        .WithMany()
                        .HasForeignKey("PrimeiroAtendimentoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SCIR.Models.TipoFormulario", "TipoFormulario")
                        .WithMany()
                        .HasForeignKey("TipoFormularioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SCIR.Models.UnidadeCurricular", b =>
                {
                    b.HasOne("SCIR.Models.Cursos", "Curso")
                        .WithMany()
                        .HasForeignKey("CursoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SCIR.Models.Usuario", b =>
                {
                    b.HasOne("SCIR.Models.Papel", "Papel")
                        .WithMany()
                        .HasForeignKey("PapelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SCIR.Models.FormularioValidacaoUC", b =>
                {
                    b.HasOne("SCIR.Models.TipoValidacaoCurricular", "TipoValidacaoCurricular")
                        .WithMany()
                        .HasForeignKey("TipoValidacaoCurricularId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SCIR.Models.UnidadeCurricular", "UnidadeCurricular")
                        .WithMany()
                        .HasForeignKey("UnidadeCurricularId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
