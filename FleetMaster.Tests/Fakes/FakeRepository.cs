using FleetMaster.Core.Entities;
using FleetMaster.Core.Interfaces;

namespace FleetMaster.Tests.Fakes
{
    public class FakeRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly List<T> _data = new List<T>();

        public void Add(T entity)
        {
            if (entity.Id == 0)
            {
                entity.Id = _data.Any() ? _data.Max(x => x.Id) + 1 : 1;
            }

            entity.CreatedAt = DateTime.Now;
            _data.Add(entity);
        }

        public void Delete(int id)
        {
            var item = GetById(id);
            if (item != null) _data.Remove(item);
        }

        public IEnumerable<T> Find(Func<T, bool> predicate)
        {
            return _data.Where(predicate);
        }

        public IEnumerable<T> GetAll()
        {
            return _data;
        }

        public T GetById(int id)
        {
            return _data.FirstOrDefault(x => x.Id == id);
        }

        public void SaveChanges()
        {
        }

        public void Update(T entity)
        {
            var existing = GetById(entity.Id);
            if (existing != null)
            {
                _data.Remove(existing);
                _data.Add(entity);
            }
        }
    }
}