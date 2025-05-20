using Microsoft.EntityFrameworkCore;

namespace ClienteAPI.Data
{
    public class BdClientesContext : DbContext
    {
        public BdClientesContext(DbContextOptions<BdClientesContext> options)
            : base(options)
        {
        }

        // Agrega tus DbSet aquí cuando crees tus entidades
        // public DbSet<Cliente> Clientes { get; set; }
    }
}
