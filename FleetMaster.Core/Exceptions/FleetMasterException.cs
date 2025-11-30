using System;

namespace FleetMaster.Core.Exceptions
{
    public class FleetMasterException : Exception
    {
        public FleetMasterException()
        {
        }

        public FleetMasterException(string message)
            : base(message)
        {
        }

        public FleetMasterException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}