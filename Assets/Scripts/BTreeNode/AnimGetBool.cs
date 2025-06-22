using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Utilities")]
public class AnimGetBool : Action
{
    public SharedGameObject TargetGameObject;
    public SharedString ParamaterName;
    public SharedBool StoreValue;
    private Animator _animator;
    private int _hashValue;

    public override void OnAwake()
    {
        _hashValue = Animator.StringToHash(ParamaterName.Value);
        
        if (!TargetGameObject.Value)
        {
            _animator = GetComponent<Animator>();
            return;
        }

        _animator = TargetGameObject.Value.GetComponent<Animator>();
    }

    public override TaskStatus OnUpdate()
    {
        StoreValue.Value = _animator.GetBool(_hashValue);
        return TaskStatus.Success;
    }
}
