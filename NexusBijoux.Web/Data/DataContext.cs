
using NexusBijoux.web.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace NexusBijoux.web.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }//Agregar Tabla Usuarios
        public DbSet<Permission> Permissions { get; set; } //Agregar Tabla Permisos
        public DbSet<Product> Products { get; set; }//Agregar Tabla Productos
    }
}
