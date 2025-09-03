using Unity.Collections;
using UnityEngine;

namespace Core.Job_System
{
    public abstract class JobLogicHandleSO : ScriptableObject
    {
        public abstract void HandleJobs();
    }
    
    public abstract class JobLogicHandleSO<T> : JobLogicHandleSO where T : unmanaged
    {
        protected NativeList<T> jobDatas;

        public virtual void AddJobData(T jobData)
        {
            if (!jobDatas.IsCreated)
            {
                jobDatas = new NativeList<T>(Allocator.Persistent);
            }
            
            jobDatas.Add(jobData);
        }

        protected virtual void OnDisable()
        {
            if (jobDatas.IsCreated)
            {
                jobDatas.Dispose();
            }
        }
    }
}