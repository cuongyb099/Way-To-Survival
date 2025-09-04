using System.Collections.Generic;
using Core.Animation_Event_System.Event_Data;
using UnityEngine;

namespace Core.Animation_Event_System
{
    public class RaiseEvent : StateMachineBehaviour
    {
        [Header("Events Auto Sort By Normalize Time")]
        [SerializeReference] protected List<AnimEvent> events;
        [SerializeField] protected bool recycleOnLoop;
        
        protected AnimationEventReceiver eventReceiver;
        protected int currentEvtIndex;
        protected int lastCycle;
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Init(animator);
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            OnUpdate(stateInfo);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //Need Update Here If Time = 1f When Animation None Looping
            OnUpdate(stateInfo);
        }

        private void OnUpdate(AnimatorStateInfo stateInfo)
        {
            float currentNormalizedTime = stateInfo.normalizedTime;

            if (currentEvtIndex >= events.Count)
            {
                if (!recycleOnLoop) return;
                currentEvtIndex = 0;
                lastCycle++;
            }
            
            float timeInCurrentCycle = currentNormalizedTime - lastCycle;
            while (currentEvtIndex < events.Count)
            {
                var evt = events[currentEvtIndex];
                
                if (evt.Time <= timeInCurrentCycle)
                {
                    evt.Raise(eventReceiver);
                    currentEvtIndex++;
                }
                else
                {
                    break; 
                }
            }
        }

        protected virtual void Init(Animator animator)
        {
            currentEvtIndex = 0;
            lastCycle = 0;
            if (eventReceiver) return;

            if (animator.TryGetComponent(out eventReceiver))
            {
                events.Sort((a,b) => a.Time.CompareTo(b.Time));
                return;
            }
            
            Debug.LogError("Event Receiver Not Found");
        }
    }
}