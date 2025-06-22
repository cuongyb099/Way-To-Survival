using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.PlayerLoop;

[TaskCategory("Utilities")]
public class SetActiveCollider : Action
{
    public SharedTransform TargetObject;
    public SharedBool Active;
    private Collider _collider;
    
    public override void OnAwake()
    {
        if (!TargetObject.Value)
        {
            _collider = GetComponent<Collider>();
            return;
        }
        
        _collider = TargetObject.Value.GetComponent<Collider>();
    }

    public override TaskStatus OnUpdate()
    {
        if (_collider == null) return TaskStatus.Failure;
        _collider.enabled = Active.Value;
        return TaskStatus.Success;
    }
}