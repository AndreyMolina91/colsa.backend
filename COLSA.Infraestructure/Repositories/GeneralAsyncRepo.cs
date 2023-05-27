using System.Linq.Expressions;
using COLSA.DataAccess.Context;
using COLSA.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace COLSA.Infraestructure.Repositories
{
    public class GeneralAsyncRepo<T> : IGeneralAsyncRepo<T> where T : class
    {
        /* 
         * Herramientas necesarias: 
         * Application Db Context.
         * DbSet de T (Genérica)
        */

        protected ApplicationDbContext _context;
        protected IConfiguration _configuration;
        internal DbSet<T> _dbSet;

        public GeneralAsyncRepo(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _dbSet = _context.Set<T>();

        }

        public async Task AddModel(T model)
        {
            await _dbSet.AddAsync(model);
        }

        public async Task<IEnumerable<T>> GetAllModel(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null,
            string includeproperties = null)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                //Select * from where filter = al parametro
                query = query.Where(filter).Take(5);
            }

            if (includeproperties != null)
            {
                //Include properties = al string
                foreach (var item in includeproperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }

            if (orderby != null)
            {
                //Orderby consulta de linq
                return await orderby(query).ToListAsync();
            }

            return await query.ToListAsync();
        }

        public async Task<T> GetFirstModel(Expression<Func<T, bool>>? filter = null, string? includeproperties = null)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeproperties != null)
            {
                foreach (var item in includeproperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }
            //Objeto 1 de la lista
            return await query.FirstOrDefaultAsync();
        }

        public async Task<T> GetModelById(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task RemoveModel(T model)
        {
            _dbSet.Remove(model);
            _context.SaveChanges();
        }

        public async Task RemoveModelById(int id)
        {
            T model = await _dbSet.FindAsync(id);
            await RemoveModel(model);
        }

        public async Task RemoveModelsRange(IEnumerable<T> modelList)
        {
            _dbSet.RemoveRange(modelList);
        }

    }
}