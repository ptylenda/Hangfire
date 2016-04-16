using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hangfire.Common;
using Hangfire.Server;
using Hangfire.Storage;

namespace Hangfire.Filters
{
    public class JobContext : IServerFilter
    {
        [ThreadStatic]
        private static string _jobId;
        
        [ThreadStatic]
        private static JobStatus _currentStatus;

        [ThreadStatic]
        private static IStorageConnection _connection;

        public static string JobId { get { return _jobId; } private set { _jobId = value; } }

        public static void UpdateStatus(object statusData)
        {
            _currentStatus.Data = statusData;
            SetCurrentStatus();
        }

        public void OnPerforming(PerformingContext context)
        {
            _connection = context.Connection;
            _currentStatus = new JobStatus
                {
                    State = "a",
                    Success = false,
                    Data = null,
                    Result = null
                };
            JobId = context.BackgroundJob.Id;
        }

        public void OnPerformed(PerformedContext filterContext)
        {
            var stateData = _connection.GetStateData(JobId);

            if (stateData != null)
            {
                _currentStatus.State = stateData.Name;
            }
            else
            {
                _currentStatus.State = "COMPLETED";
            }

            _currentStatus.Success = (filterContext.Canceled == false) && filterContext.Exception == null;
            _currentStatus.Result = filterContext.Result;
            
            SetCurrentStatus();

            _connection = null;
            JobId = null;
        }

        private static void SetCurrentStatus()
        {
            _connection.SetJobStatus(_jobId, JobHelper.ToJson(_currentStatus));
        }
    }

    public class JobStatus
    {
        public string State { get; set; }

        public bool Success { get; set; }
        
        public object Data { get; set; }

        public object Result { get; set; }
    }
}
