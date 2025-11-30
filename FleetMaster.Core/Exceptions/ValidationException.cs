using System;

namespace FleetMaster.Core.Exceptions
{
    public class ValidationException : FleetMasterException
    {
        public ValidationException(string message) : base(message)
        {
        }

        public ValidationException(string field, string error)
            : base($"Помилка валідації поля '{field}': {error}")
        {
        }
    }
}