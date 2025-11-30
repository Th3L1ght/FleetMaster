using System;

namespace FleetMaster.Core.Exceptions
{
    public class EntityNotFoundException : FleetMasterException
    {
        public EntityNotFoundException(string entityName, int id)
            : base($"Сутність '{entityName}' з ID {id} не знайдена в базі даних.")
        {
        }

        public EntityNotFoundException(string message) : base(message)
        {
        }
    }
}