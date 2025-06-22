using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("AI")]
public class IsTargetInRange : Conditional
{
    public SharedFloat RadiusCheck;
    public SharedLayerMask LayerMask;
    public SharedString TargetTag;
    public SharedVector3 Offset;
    protected Collider[] colliders = new Collider[5];
    
    public override TaskStatus OnUpdate()
    {
        var hitCount = Physics.OverlapSphereNonAlloc(transform.position + Offset.Value, RadiusCheck.Value, colliders, LayerMask.Value);

        if (hitCount == 0) return TaskStatus.Failure;
        
        for (var i = 0; i < hitCount; i++)
        {
            Collider collider = colliders[i];
            if (collider.CompareTag(TargetTag.Value))
                return TaskStatus.Success;
        }

        return TaskStatus.Failure;
    }

#if UNITY_EDITOR
    public bool DrawGizmos;
    public override void OnDrawGizmos()
    {
        if(!DrawGizmos) return;
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + Offset.Value, RadiusCheck.Value);
    }
#endif
}