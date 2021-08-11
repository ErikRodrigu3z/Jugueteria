using Jugueteria.Models;
using Jugueteria.Models.Segurity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Jugueteria.Persistence
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        //Fluent API - conventions
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Toys
            modelBuilder.Entity<Toys>().HasKey(x => x.Id);
            modelBuilder.Entity<Toys>().Property(x => x.Nombre)
                .HasMaxLength(50)
                .IsRequired();
            modelBuilder.Entity<Toys>().Property(x => x.Compañía)
                .HasMaxLength(50)
                .IsRequired();
            modelBuilder.Entity<Toys>().Property(x => x.Descripcion)
                .HasMaxLength(100);                
            modelBuilder.Entity<Toys>().Property(x => x.Precio)
                .HasMaxLength(1000)                
                .IsRequired();
            modelBuilder.Entity<Toys>().Property(x => x.RestriccionEdad)
                .HasMaxLength(100);
            

        }

        //DbSet
        public DbSet<Users> User { get; set; }
        public DbSet<Toys> Toys { get; set; } 


    }
}
