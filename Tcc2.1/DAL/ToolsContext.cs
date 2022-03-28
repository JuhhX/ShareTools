using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using Tcc2._1.Models;

namespace Tcc2._1.DAL
{
    public class ToolsContext:DbContext
    {
        public ToolsContext() : base("ToolsDB") 
        { 

        }
        public DbSet<Ferramenta> Ferramenta { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<MeuCarrinho> MeuCarrinho { get; set; }

        public DbSet<Compras> Compras { get; set; }

        public DbSet<Pedido> Pedido { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}