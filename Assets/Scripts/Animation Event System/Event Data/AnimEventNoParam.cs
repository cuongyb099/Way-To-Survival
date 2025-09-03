using System;

namespace Core.Animation_Event_System.Event_Data
{
    [Serializable]
    public class AnimEventNoParam : AnimEvent<AnimEventID>
    {
        public override void Raise(AnimationEventReceiver receiver)
        {
            receiver.Publish(this.id);
        }
    }
    
    public enum AnimEventID
    {
        None = 0,
        OnCurrentAnimationEnd,
    }
}
