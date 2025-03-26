using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Modelos;

namespace info
{
    public partial class APPDbContext : DbContext
    {
        public IConfiguration Configuration { get; }

        public DbSet<CentroReceita> CentroReceitas { get; set; }
        public DbSet<Carteira> Carteiras { get; set; }
        public DbSet<Cedente> Cedentes { get; set; }
        public DbSet<UF> UFs { get; set; }
        public DbSet<Municipio> Municipios { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Revenda> Revendas { get; set; }
        public DbSet<GeradorID> Generators { get; set; }
        public DbSet<Atualizacao> Atualizacoes { get; set; }
        public DbSet<TipoAPP> TiposAPP { get; set; }
        public DbSet<Retorno> Retornos { get; set; }
        public DbSet<Boleto> Boletos { get; set; }
        public DbSet<Sacado> Sacados { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<StatusTitulo> StatusTitulo { get; set; }
        public DbSet<RelacaoBoletos> RelacaoBoletos { get; set; }
        public DbSet<BoletoEvento> BoletoEventos { get; set; }
        public DbSet<ListaEventos> ListaEventos { get; set; }


        public APPDbContext(DbContextOptions<APPDbContext> options, IConfiguration configuration)
            : base(options)
        {
            Configuration = configuration;
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                optionsBuilder.UseSqlServer(Configuration.GetConnectionString("Default")!,
                    builder => builder.EnableRetryOnFailure());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.NoAction;

            OnModelCreatingPartial(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
