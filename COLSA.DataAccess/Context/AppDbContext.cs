
using COLSA.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace COLSA.DataAccess.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<TournamentModel> Tournament { get; set; }
    }
}