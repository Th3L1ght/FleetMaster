using System;

namespace FleetMaster.Core.Entities
{
    public abstract class BaseEntity
    {
        private int _id;
        private DateTime _createdAt;

        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("ID не може бути від'ємним", nameof(Id));
                }
                _id = value;
            }
        }

        public DateTime CreatedAt
        {
            get
            {
                return _createdAt;
            }
            set
            {
                if (value > DateTime.Now.AddDays(1))
                {
                    throw new ArgumentException("Дата створення не може бути в майбутньому");
                }
                _createdAt = value;
            }
        }

        public BaseEntity()
        {
            _createdAt = DateTime.Now;
        }
    }
}