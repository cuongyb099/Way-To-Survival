using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using BTreeNode.SharedType;
using Core.Job_System.JobLogicHandle;
using UnityEngine;

[TaskCategory("Job System")]
public class OverlapSphereJob : Conditional
{
    public SharedFloat Radius = 1f;
    public SharedVector3 Offset;
    public SharedLayerMask Layer;
    public OverlapSphereJobHandleSO JobHandle;
    public SharedListCollider HitResults;
    
    public override void OnStart()
    {
        JobHandle.AddJobData(new OverlapSphereData()
        {
            Point = transform.position + Offset.Value,
            Radius = Radius.Value,
            Layer = Layer.Value,
            InstanceID = gameObject.GetInstanceID()
        });
    }

    public override TaskStatus OnUpdate()
    {
        var hits = JobHandle.GetResult(gameObject.GetInstanceID());
        
        if (hits == null) return TaskStatus.Running;
        if (hits.Count == 0) return TaskStatus.Failure;
        
        HitResults.Value = hits;
        return TaskStatus.Success;
    }

#if UNITY_EDITOR
    public override void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + Offset.Value, Radius.Value);
    }
#endif
}