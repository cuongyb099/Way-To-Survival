using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Core.Animation_Event_System.Event_Data
{
    [Serializable]
    public abstract class AnimEvent
    {
        [SerializeField, Range(0, 1f)] 
        protected float time = 1;
        public float Time => time;
        public abstract void Raise(AnimationEventReceiver receiver);
    }
    
    [Serializable]
    public abstract class AnimEvent<TEnum> : AnimEvent where TEnum : unmanaged, Enum
    {
        [SerializeField] protected TEnum id;
        public TEnum ID => id;

        public int GetHashFromEnum(Type enumType, int enumToInterger)
        {
            return enumToInterger ^ RuntimeHelpers.GetHashCode(enumType);
        }

        public override void Raise(AnimationEventReceiver receiver)
        {
            receiver.Publish(this.id);
        }
    }

    [Serializable]
    public abstract class AnimEvent<TValue, TEnum> : AnimEvent<TEnum> where TEnum : unmanaged, Enum
    {
        [SerializeField] protected TValue param;
        
        public override void Raise(AnimationEventReceiver receiver)
        {
            receiver.Publish(this.id, param);
        }
    }
}
