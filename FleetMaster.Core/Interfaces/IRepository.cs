using FleetMaster.Core.Entities;

namespace FleetMaster.Core.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        void Add(T entity);
        T GetById(int id);
        IEnumerable<T> GetAll();
        void Update(T entity);
        void Delete(int id);

        IEnumerable<T> Find(Func<T, bool> predicate);
    }
}