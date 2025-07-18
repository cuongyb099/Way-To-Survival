using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Job/OverlapSphere")]
public class OverlapSphereJobSO : EntitiesJobSO
{
    [field: SerializeField] public int MaxHits { get; private set; }
    
    public override void DoJob(List<JobData> jobData)
    {
        if(jobData.Count <= 0 ) return;
        var data = jobData.Cast<OverlapSphereData>().ToArray();
        var commands = new NativeArray<OverlapSphereCommand>(jobData.Count , Allocator.TempJob);
        var hits = new NativeArray<ColliderHit>(jobData.Count  * MaxHits, Allocator.TempJob);


        for (var i = 0; i < jobData.Count ; i++)
        {
            commands[i] = new OverlapSphereCommand()
            {
                queryParameters = new QueryParameters()
                {
                    layerMask = data[i].Layer.Value,
                },
                radius = data[i].Radius.Value,
                point = data[i].Point.position + data[i].Offset.Value,
            };
        }
        
        OverlapSphereCommand.ScheduleBatch(commands, hits, 1, MaxHits).Complete();
        
        for (int i = 0; i < jobData.Count; i++)
        {
            var hitTargets = data[i].HitTargets.Value;
            
            if (hitTargets == null)
            {
                hitTargets = new HashSet<Transform>();
                data[i].HitTargets.Value = hitTargets;
            }
            
            hitTargets.Clear();
            for (int j = 0; j < MaxHits; j++)
            {
                var index = i * MaxHits + j;

                var hitCollider = hits[index].collider;
                if (!hits[index].collider) continue;
                
                hitTargets.Add(hitCollider.transform);
                break;
            }
        }

        commands.Dispose();
        hits.Dispose();
    }
}