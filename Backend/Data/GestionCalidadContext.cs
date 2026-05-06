using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GestionCalidad.Backend.Dominio;

namespace GestionCalidad.Backend.Data
{
    public class GestionCalidadContext : DbContext
    {
        public GestionCalidadContext (DbContextOptions<GestionCalidadContext> options)
            : base(options)
        {
        }

        public DbSet<Departamento> Departamento { get; set; } = default!;
        public DbSet<InformeCalidad> InformeCalidad { get; set; } = default!;
        public DbSet<InformeQueja> InformeQueja { get; set; } = default!;
        public DbSet<InformeCalidad_Departamento> InformeCalidad_Departamento { get; set; } = default!;
        public DbSet<InformeQueja_Departamento> InformeQueja_Departamento { get; set; } = default!;
    }
}
