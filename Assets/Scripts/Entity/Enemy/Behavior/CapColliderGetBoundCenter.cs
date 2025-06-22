using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("AI")]
public class CapColliderGetBoundCenter : Action
{
    private CapsuleCollider _capsuleCollider;
    public SharedVector3 Offset;
    public SharedVector3 StoredValue;
    
    public override void OnAwake()
    {
        base.OnAwake();
        _capsuleCollider = GetComponent<CapsuleCollider>();
    }

    public override TaskStatus OnUpdate()
    {
        if (!_capsuleCollider)
        {
            return TaskStatus.Failure;
        }
        
        StoredValue.Value = _capsuleCollider.bounds.center + Offset.Value;
        return TaskStatus.Success;
    }
}