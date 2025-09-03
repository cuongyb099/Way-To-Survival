using System.Collections.Generic;
using Tech.Singleton;
using UnityEngine;

namespace Core.Job_System
{
    public class JobManager : Singleton<JobManager>
    {
        [SerializeField]
        private List<JobLogicHandleSO> _jobHandles = new ();

        private void Update()
        {
            foreach (var job in _jobHandles)
            {
                job.HandleJobs();
            }
        }
    }
}
