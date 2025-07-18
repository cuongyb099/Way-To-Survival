using System.Threading.Tasks;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Core.AnimationEventSystem;
using Core.AnimationEventSystem.EventData;
using UnityEngine;

namespace BTreeNode
{
    [TaskCategory("Animation Event")]
    public class RegisterAnimEventNoParam : Action
    {
        public AnimEventNoParamSO Evt;
        protected AnimationEventReceiver receiver;
        public SharedBool IsEventRaised;
        
        public override void OnAwake()
        {
            receiver = this.transform.GetComponentInChildren<AnimationEventReceiver>();
            receiver.RegisterEvent(Evt.Key, () =>
            {
                IsEventRaised.Value = true;
            });
        }
    }
}