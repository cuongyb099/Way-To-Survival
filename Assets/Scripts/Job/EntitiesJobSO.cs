using System.Collections.Generic;
using UnityEngine;

public abstract class EntitiesJobSO : ScriptableObject
{
    public abstract void DoJob(List<JobData> jobData);
}
