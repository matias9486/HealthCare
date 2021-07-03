using HealthCare.Web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCare.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<Usuario> //se agrego generic <Usuario> para que podamos usar nuestra clase usuario
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Producto> Productos { get; set; }

        //Se agrego para los usuarios
        public DbSet<Usuario> Usuarios { get; set; }

        //Se agrego para los tratamientos
        public DbSet<Tratamiento> Tratamientos { get; set; }

        //Se agrego para tipo de tipo de patologias
        public DbSet<TipoPatologia> TipoPatologias { get; set; }

        //Se agrego para tipo de patologias
        public DbSet<Patologia> Patologias { get; set; }


    }
}
