using BehaviorDesigner.Runtime;

[System.Serializable]
public class SharedSoundSetting : SharedVariable<SoundSetting>
{
    public static implicit operator SharedSoundSetting(SoundSetting value) { return new SharedSoundSetting { mValue = value }; }
}