using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TLS.Nautilus.Api.Shared.DataStructures
{
    public class JobResult
    {
        public SiteCalculationAction Action { get; set; }
        public DateTime QueueTime { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public string RunBy { get; set; }

        public string Output { get; set; }

        public bool Success { get; set; }

        public List<JobResultEntry> Results { get; set; }

        public JobResult()
        {
            Results = new List<JobResultEntry>();
        }
    }

    public enum SiteCalculationAction
    {
        [Description("Calculation")]
        Calculate,
        [Description("Export")]
        Export,
        [Description("Generation")]
        Generate
    }

    public class JobResultEntry
    {
        public JobResultEntryStatus Status { get; set; }
        public string Message { get; set; }
    }

    public enum JobResultEntryStatus
    {
        Warning,
        Error
    }
}
