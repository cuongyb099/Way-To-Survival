using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("AI")]
public class IsTargetInRangeJob : Conditional
{
    public OverlapSphereData Receiver;
    public SharedTransform ResultTarget;
    public SharedFloat Range;
    public SharedVector3 Offset;

    public override void OnAwake()
    {
        Receiver.Point = transform;
        Receiver.Radius = Range.Value;
        Receiver.Offset = Offset.Value;
        EntitiesJobManager.Instance.Add(Receiver);
    }

    public override TaskStatus OnUpdate()
    {
        ResultTarget.Value = Receiver.Target;
        return Receiver.Target ? TaskStatus.Success : TaskStatus.Failure;
    }

#if UNITY_EDITOR
    public override void OnDrawGizmos()
    {
        if(Receiver.Point == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + Offset.Value, Range.Value);
    }
#endif
}