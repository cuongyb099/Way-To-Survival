using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Job System")]
public class OverlapSphereJob : Conditional
{
    public OverlapSphereData Receiver;
    
    public override void OnAwake()
    {
        Receiver.Point = transform;
        EntitiesJobManager.Instance.Add(Receiver);
    }

    public override TaskStatus OnUpdate()
    {
        if (Receiver.HitTargets.Value.Count > 0)
        {
            return TaskStatus.Success;
        }
        
        return TaskStatus.Failure;
    }

#if UNITY_EDITOR
    public override void OnDrawGizmos()
    {
        if(Receiver.Point == null) return;
        Gizmos.color = Color.green;
        Vector3 offset = Receiver.Offset.Value;
        float range = Receiver.Radius.Value;
        Gizmos.DrawWireSphere(transform.position + offset, range);
    }
#endif
}