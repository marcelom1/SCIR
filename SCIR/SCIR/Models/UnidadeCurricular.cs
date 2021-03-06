﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SCIR.Models
{
    public class UnidadeCurricular
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Nome é um campo obrigatório.")]
        public string Nome { get; set; }
        public bool Ativo { get; set; }
        public int CursoId { get; set; }
        public Cursos Curso { get; set; }

        public UnidadeCurricular()
        {
            Curso = new Cursos();
        }
    }
}