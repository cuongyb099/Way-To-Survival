using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using UnityEngine;

[TaskCategory("Utilities")]
public class PlaySound : Action
{
    public SharedAudioClipArray AudioClips;
    public SharedSoundSetting Setting = SoundSetting.GetDefault();
    public SharedBool UseDelayTime = false;
    public SharedFloat DelayTime = 0.25f;
    private bool _isDelay;
    private TweenCallback _tweenCallback;

    public override void OnAwake()
    {
        base.OnAwake();
        _tweenCallback += ResetDelay;
    }

    private void ResetDelay()
    {
        _isDelay = false;
    }

    public override void OnPause(bool paused)
    {
        _tweenCallback -= ResetDelay;
    }

    public override void OnStart()
    {
        if(!UseDelayTime.Value || _isDelay) return;
        _isDelay = true;
        DOVirtual.DelayedCall(Random.Range(DelayTime.Value/2f,DelayTime.Value), _tweenCallback);
        
        if (Setting.Value.SpatialBlend > 0)
        {
            AudioManager.Instance.PlaySound(AudioClips.Value, Setting.Value, transform);
            return;
        }
        
        AudioManager.Instance.PlaySound(AudioClips.Value, Setting.Value.Loop);
    }

    public override TaskStatus OnUpdate()
    {
        return TaskStatus.Success;
    }
}