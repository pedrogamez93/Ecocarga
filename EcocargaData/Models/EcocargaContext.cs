using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Cl.Gob.Energia.Ecocarga.Data.Models
{
    public partial class EcocargaContext : DbContext
    {
        public EcocargaContext()
        {
        }

        public EcocargaContext(DbContextOptions<EcocargaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DataAuto> DataAuto { get; set; }
        public virtual DbSet<DataCargador> DataCargador { get; set; }
        public virtual DbSet<DataCompania> DataCompania { get; set; }
        public virtual DbSet<DataElectrolinera> DataElectrolinera { get; set; }
        public virtual DbSet<DataMarcaauto> DataMarcaauto { get; set; }
        public virtual DbSet<DataObservacion> DataObservacion { get; set; }
        public virtual DbSet<DataTipoconector> DataTipoconector { get; set; }
        public virtual DbSet<DataVersion> DataVersion { get; set; }
        public virtual DbSet<DataTipocobro> DataTipocobro { get; set; }
        public virtual DbSet<DataDiccionarioTipoConector> DataDiccionarioTipoConector { get; set; }
        public virtual DbSet<DataUsuario> DataUsuario { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
              optionsBuilder.UseSqlServer("fake");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
