using System.Text.Json;
using FleetMaster.Core.Entities;
using FleetMaster.Core.Interfaces;

namespace FleetMaster.Infrastructure.Repositories
{
    public class FileRepository<T> : IRepository<T> where T : BaseEntity
    {
        private const string DataFolder = "data";
        private readonly string _filePath;
        private List<T> _items;

        public FileRepository(string fileName)
        {
            if (!Directory.Exists(DataFolder))
            {
                Directory.CreateDirectory(DataFolder);
            }

            _filePath = Path.Combine(DataFolder, fileName);
            _items = LoadData();
        }

        public void Add(T entity)
        {
            entity.Id = _items.Any() ? _items.Max(i => i.Id) + 1 : 1;
            entity.CreatedAt = DateTime.Now;

            _items.Add(entity);
            SaveChanges();
        }

        public void Delete(int id)
        {
            var item = GetById(id);
            if (item != null)
            {
                _items.Remove(item);
                SaveChanges();
            }
        }

        public IEnumerable<T> Find(Func<T, bool> predicate)
        {
            return _items.Where(predicate);
        }

        public IEnumerable<T> GetAll()
        {
            return _items;
        }

        public T GetById(int id)
        {
            return _items.FirstOrDefault(x => x.Id == id);
        }

        public void Update(T entity)
        {
            var existingItem = GetById(entity.Id);
            if (existingItem != null)
            {
                var index = _items.IndexOf(existingItem);
                _items[index] = entity;
                SaveChanges();
            }
        }

        public void SaveChanges()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(_items, options);
            File.WriteAllText(_filePath, json);
        }

        private List<T> LoadData()
        {
            if (!File.Exists(_filePath))
            {
                return new List<T>();
            }

            string json = File.ReadAllText(_filePath);
            if (string.IsNullOrWhiteSpace(json))
            {
                return new List<T>();
            }

            try
            {
                return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
            }
            catch
            {
                return new List<T>();
            }
        }
    }
}