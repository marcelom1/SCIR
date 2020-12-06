using SCIR.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCIR.Models.ViewModels
{
    public class CursoVM
    {
        public Cursos Curso { get; set; }
        public ConsisteUtils Consistencia { get; set; }
        
        public CursoVM()
        {
            Curso = new Cursos();
            Consistencia = new ConsisteUtils();
        }
    }
}