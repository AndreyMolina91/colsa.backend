namespace COLSA.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        //Acá crearemos los objs de tipo interface de los modelos
        IUser Users { get; }
        ITournament Tournament { get; }
        void SaveData();
    }
}