using System;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Core.Animation_Event_System;
using UnityEngine;
using Action = BehaviorDesigner.Runtime.Tasks.Action;

public abstract class WaitAnimEventBase<T> : Action where T : unmanaged, Enum
{
    public SharedGameObject Target;
    public T Event;
    protected AnimationEventReceiver receiver;
    protected bool isEventRaised;
    
    public override void OnAwake()
    {
        GameObject target = !this.Target.Value ? this.gameObject : this.Target.Value;
        receiver = target.GetComponentInChildren<AnimationEventReceiver>();
        receiver.Subscribe(Event, () => { isEventRaised = true; });
    }

    public override TaskStatus OnUpdate()
    {
        return isEventRaised ? TaskStatus.Success : TaskStatus.Running;
    }

    public override void OnEnd()
    {
        isEventRaised = false;
    }
}