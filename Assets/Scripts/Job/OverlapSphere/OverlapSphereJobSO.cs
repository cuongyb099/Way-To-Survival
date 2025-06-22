using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Job/OverlapSphere")]
public class OverlapSphereJobSO : EntitiesJobSO
{
    [field: SerializeField] public int MaxHits { get; private set; }
    [field: SerializeField] public LayerMask Mask { get; private set; }
    [field: SerializeField] public string TagCheck { get; private set; }
    
    
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
                    layerMask = Mask
                },
                radius = data[i].Radius,
                point = data[i].Point.position + data[i].Offset,
            };
        }
        
        OverlapSphereCommand.ScheduleBatch(commands, hits, 1, MaxHits).Complete();
        
        for (int i = 0; i < jobData.Count; i++)
        {
            for (int j = 0; j < MaxHits; j++)
            {
                var index = i * MaxHits + j;

                if (!hits[index].collider || !hits[index].collider.CompareTag(TagCheck))
                {
                    data[i].Target = null;
                    continue;
                }
                
                data[i].Target = hits[index].collider.transform;
                break;
            }
        }

        commands.Dispose();
        hits.Dispose();
    }
}