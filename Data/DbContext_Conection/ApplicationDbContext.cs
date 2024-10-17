using Microsoft.EntityFrameworkCore;
using Model.Entidades;

namespace Data.DbContext_Conection;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
}
