using SCIR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCIR.DAO.Cadastros
{
    public class CursosDao
    {
        public void Add(Cursos curso)
        {
            using (var context = new ScirContext())
            {
                context.Cursos.Add(curso);
                context.SaveChanges();
            }
        }

        public IList<Cursos> List()
        {
            using (var contexto = new ScirContext())
            {
                return contexto.Cursos.ToList();
            }
        }

        public Cursos BuscarPorId(int id)
        {
            using (var contexto = new ScirContext())
            {
                return contexto.Cursos.Where(e => e.Id == id).FirstOrDefault();
            }
        }

        public void Update(Cursos curso)
        {
            using (var contexto = new ScirContext())
            {
                contexto.Cursos.Update(curso);
                contexto.SaveChanges();
            }
        }

        public void Delete(Cursos curso)
        {
            using (var contexto = new ScirContext())
            {
                contexto.Cursos.Remove(curso);
                contexto.SaveChanges();
            }
        }

    }
}