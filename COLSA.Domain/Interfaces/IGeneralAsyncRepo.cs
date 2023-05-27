
using System.Linq.Expressions;

namespace COLSA.Domain.Interfaces
{
    public interface IGeneralAsyncRepo<T> where T : class
    {
        //Metodos a implementar

        //Obtener Promo o Rating por id
        /*1*/
        Task<T> GetModelById(int id);

        //Obtener Lista de T IENumerable con consultas Linq
        /*2*/
        Task<IEnumerable<T>> GetAllModel(Expression<Func<T, bool>>? filter = null,
          Func<IQueryable<T>, IOrderedQueryable<T>>? orderby = null,
          string? includeproperties = null
          );
        //consultaremos por el primero que tenga X propiedad
        /*3*/
        Task<T> GetFirstModel(
          Expression<Func<T, bool>>? filter = null,
          string? includeproperties = null
          );
        //Agregar
        /*4*/
        Task AddModel(T entity);

        //Remove por id
        /*5*/
        Task RemoveModelById(int id);

        //Remove T = Clase
        /*6*/
        Task RemoveModel(T entity);

        //Remove por rango
        /*7*/
        Task RemoveModelsRange(IEnumerable<T> entitylist);

    }
}