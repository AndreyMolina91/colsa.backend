using COLSA.DataAccess.Context;
using COLSA.Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace COLSA.Infraestructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        /*constructor herramienta para inicializar los objs de la interfaz*/
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        // Repositories injection to can use
        public IUser Users { get; private set; }
        public UnitOfWork(ApplicationDbContext context, IConfiguration configuration)
        {
            // Inicializar herramientas necesarias para inyectarlo a los repositorios
            _context = context;
            _configuration = configuration;
            // Initialize interface and Repo with context params.
            Users = new UserRepo(_context, _configuration);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public void SaveData()
        {
            _context.SaveChanges();
        }
    }
}