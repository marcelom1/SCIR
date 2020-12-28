﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SCIR.DAO;

namespace SCIR.Migrations
{
    [DbContext(typeof(ScirContext))]
    [Migration("20201223205816_Papeis")]
    partial class Papeis
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

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

            modelBuilder.Entity("SCIR.Models.StatusRequerimento", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("Ativo")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("Cancelamento")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("StatusRequerimento");
                });

            modelBuilder.Entity("SCIR.Models.TipoFormulario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
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

                    b.Property<int>("TipoFormularioId")
                        .HasColumnType("int");

                    b.HasKey("Id");

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

            modelBuilder.Entity("SCIR.Models.TipoRequerimento", b =>
                {
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
#pragma warning restore 612, 618
        }
    }
}