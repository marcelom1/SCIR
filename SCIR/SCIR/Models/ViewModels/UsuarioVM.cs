using SCIR.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCIR.Models.ViewModels
{
    public class UsuarioVM
    {
        public Usuario Usuario { get; set; }
        public ConsisteUtils Consistencia { get; set; }

        public UsuarioVM()
        {
            Usuario = new Usuario();
            Consistencia = new ConsisteUtils();
        }
    }
}