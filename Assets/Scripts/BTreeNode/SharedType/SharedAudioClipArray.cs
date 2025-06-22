using BehaviorDesigner.Runtime;
using UnityEngine;

[System.Serializable]
public class SharedAudioClipArray : SharedVariable<AudioClip[]>
{
    public static implicit operator SharedAudioClipArray(AudioClip[] value) { return new SharedAudioClipArray { mValue = value }; }
}
