using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCIR.Utils
{
    public class ConsisteUtils
    {
        public IList<string> Advertencias { get; }
        public IList<string> Inconsistencias { get; }
        public IList<string> Sucesso { get; }
        public string AdvertenciasToString
        {
            get => ToString(Advertencias);
        }

        public string InconsistenciasToString
        {
            get => ToString(Inconsistencias);
        }

        public string SucessoToString
        {
            get => ToString(Sucesso);
        }


        public enum Tipo
        {
            Advertecia,
            Inconsistencia,
            Sucesso
        }

        public ConsisteUtils()
        {
            Advertencias = new List<string>();
            Inconsistencias = new List<string>();
            Sucesso = new List<string>();
        }

        public void Add(string msg, Tipo tipo)
        {
            if (tipo == Tipo.Advertecia)
                Advertencias.Add(msg);
            else if (tipo == Tipo.Inconsistencia)
                Inconsistencias.Add(msg);
            else if (tipo == Tipo.Sucesso)
                Sucesso.Add(msg);
        }

        private string ToString(IList<string> lista)
        {
            var retorno = "";
            foreach (var item in lista)
            {
                retorno += item + "|";
            }

            return retorno;
        }

    }
}