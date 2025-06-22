using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using UnityEngine;

[TaskCategory("AI")]
public class RotateToTarget : Action
{
    public SharedTransform Target;
    private Tween _rotateTween;
    private TweenCallback _callback;
    private Quaternion rotation;

    public override void OnAwake()
    {
        _callback = () =>
        {
            transform.rotation = rotation;
        };
    }

    public override TaskStatus OnUpdate()
    {
        Vector3 direction = Target.Value.position - transform.position;
        direction = Vector3.ProjectOnPlane(direction, Vector3.up).normalized;
        rotation = Quaternion.LookRotation(direction, Vector3.up);
        if(_rotateTween.IsActive()) _rotateTween.Kill();
        _rotateTween = transform.DORotateQuaternion(rotation, 0.25f).OnKill(_callback);
    
        return TaskStatus.Success;
    }
}