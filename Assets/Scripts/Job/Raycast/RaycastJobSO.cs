using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

public abstract class RaycastJobSO : EntitiesJobSO
{
    [field: SerializeField] public int MaxHits { get; private set; }
    [field: SerializeField] public LayerMask Mask { get; private set; }
    
    public override void DoJob(List<JobData> jobData)
    {
        if(jobData.Count <= 0 ) return;
        var data = jobData.Cast<RaycastData>().ToArray();
        
        var commands = new NativeArray<RaycastCommand>(jobData.Count , Allocator.TempJob);
        var hits = new NativeArray<RaycastHit>(jobData.Count  * MaxHits, Allocator.TempJob);
        
        for (var i = 0; i < jobData.Count ; i++)
        {
            commands[i] = new RaycastCommand()
            {
                queryParameters = new QueryParameters()
                {
                    layerMask = Mask
                },
                from = data[i].OriginPoint,
                direction = data[i].Direction,
                distance = data[i].Distance
            };
        }
        
        RaycastCommand.ScheduleBatch(commands, hits, 1, MaxHits).Complete();

        OnBatchCompleted(jobData, ref hits);

        commands.Dispose();
        hits.Dispose();
    }

    protected abstract void OnBatchCompleted(List<JobData> jobData, ref NativeArray<RaycastHit> hits);
}