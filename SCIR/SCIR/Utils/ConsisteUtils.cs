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
        public string AdvertenciasToString
        {
            get => ToString(Advertencias);
        }
        public string InconsistenciasToString
        {
            get => ToString(Inconsistencias);
        }


        public enum Tipo
        {
            Advertecia,
            Inconsistencia
        }

        public ConsisteUtils()
        {
            Advertencias = new List<string>();
            Inconsistencias = new List<string>();
        }

        public void Add(string msg, Tipo tipo)
        {
            if (tipo == Tipo.Advertecia)
                Advertencias.Add(msg);
            else
                Inconsistencias.Add(msg);
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