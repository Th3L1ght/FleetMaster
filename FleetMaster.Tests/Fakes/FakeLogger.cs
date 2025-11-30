using System;
using System.Collections.Generic;
using FleetMaster.Core.Interfaces;

namespace FleetMaster.Tests.Fakes
{
    public class FakeLogger : ILogger
    {
        public List<string> Logs { get; } = new List<string>();

        public void LogError(string message, Exception ex = null)
        {
            Logs.Add($"ERROR: {message}");
        }

        public void LogInfo(string message)
        {
            Logs.Add($"INFO: {message}");
        }

        public void LogWarning(string message)
        {
            Logs.Add($"WARNING: {message}");
        }
    }
}