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
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("Server=localhost;User Id=root;Database=dbscir"); ;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { }

    }
}