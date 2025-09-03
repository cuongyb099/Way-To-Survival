using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Core.Animation_Event_System;
using KatLib.Observer;
using UnityEngine;

namespace BTreeNode
{
    [TaskCategory("Animation Event")]
    public class WaitAnimEventRaise : Action
    {
        public GameObject Target;
        public GenericEnum Event;
        protected AnimationEventReceiver receiver;
        protected bool isEventRaised;
        
        public override void OnAwake()
        {
            receiver = this.Target.GetComponentInChildren<AnimationEventReceiver>();
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
}