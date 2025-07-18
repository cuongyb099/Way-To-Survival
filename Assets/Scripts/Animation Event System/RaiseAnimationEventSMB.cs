using System.Collections.Generic;
using Core.AnimationEventSystem.EventData;
using UnityEngine;

namespace Core.AnimationEventSystem
{
    public class RaiseAnimationEventSMB : StateMachineBehaviour
    {
        [Header("Events Auto Sort By Normalize Time")]
        [SerializeField] protected List<AnimEventSO> events;
        [SerializeField] protected bool recycleOnLoop;
        
        protected AnimationEventReceiver eventReceiver;
        [SerializeField] protected int currentEvtIndex;
        
#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            events.Sort((a, b) => a.NormalizeTimeCall.CompareTo(b.NormalizeTimeCall));
        }
#endif
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Init(animator);
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            bool isPlayingBackwards = stateInfo.speed < 0f;
            float currentNormalizedTime = isPlayingBackwards ? 1 - stateInfo.normalizedTime : stateInfo.normalizedTime;
            
            if (currentNormalizedTime >= 1f && recycleOnLoop)
            {
                currentEvtIndex = 0;
            }
            
            if(currentEvtIndex >= events.Count) return;
            
            var evt = events[currentEvtIndex];

            if (currentNormalizedTime < evt.NormalizeTimeCall) return;
            
            evt.Raise(eventReceiver);

            currentEvtIndex++;
        }

        protected virtual void Init(Animator animator)
        {
            currentEvtIndex = 0;
            
            if (eventReceiver) return;

            if (animator.TryGetComponent(out eventReceiver)) return;
            events.Sort((a, b) => a.NormalizeTimeCall.CompareTo(b.NormalizeTimeCall));
#if UNITY_EDITOR
            Debug.LogError("Event Receiver Not Found");
#endif
        }
    }
}
