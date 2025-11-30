using System;

namespace FleetMaster.Core.Exceptions
{
    public class BusinessLogicException : FleetMasterException
    {
        public BusinessLogicException(string message) : base(message)
        {
        }
    }
}