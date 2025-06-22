using System;
using Tech.Pooling;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

//Simple Sound Manager 
public class AudioManager : MonoBehaviour
{
    [SerializeField] private GameObject _audioPrefab;
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private AudioMixerGroup _masterGroup;
    [SerializeField] private AudioMixerGroup _bgGroup;
    [SerializeField] private AudioMixerGroup _fxGroup;
    [SerializeField] private AudioMixerGroup _uiGroup;

    private static readonly string _masterVolume = "Master Volume"; 
    private static readonly string _bgVolume = "BG Volume"; 
    private static readonly string _fxVolume = "FX Volume"; 
    private static readonly string _uiVolume = "UI Volume"; 
    
    public static AudioManager Instance { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        if (Instance) return;
        GameObject go = new GameObject("Audio Manager");
        DontDestroyOnLoad(go);
        Instance = go.AddComponent<AudioManager>();
        
    }

    private void Awake()
    {
        _audioPrefab = Resources.Load<GameObject>("Audio Child");
        _mixer = Resources.Load<AudioMixer>("Global Mixer");
        _bgGroup = _mixer.FindMatchingGroups("BG Music")[0];
        _fxGroup = _mixer.FindMatchingGroups("Sound FX")[0];
        _uiGroup = _mixer.FindMatchingGroups("UI Sound")[0];
        _masterGroup = _mixer.FindMatchingGroups("Master")[0]; 
    }

    private void Start()
    {
        LoadSave();
    }

    private void LoadSave()
    {
        SetVolume(_masterVolume, PlayerPrefs.GetFloat(_masterVolume, 1f));
        SetVolume(_bgVolume, PlayerPrefs.GetFloat(_bgVolume, 1f));
        SetVolume(_fxVolume, PlayerPrefs.GetFloat(_fxVolume, 1f));
        SetVolume(_uiVolume, PlayerPrefs.GetFloat(_uiVolume, 1f));
    }

    public void PlaySound(AudioClip audioClip, bool isLoop = false, float volume = 1f, SoundVolumeType volumeType = SoundVolumeType.SOUNDFX_VOLUME)
    {
        AudioChild audioChild = ObjectPool.Instance.SpawnObject(_audioPrefab,
            default, default, PoolType.Audio).GetComponent<AudioChild>();

        AudioSource source = audioChild.Source;
        source.outputAudioMixerGroup = GetMixerGroup(volumeType);
        source.clip = audioClip;
        
        SoundSetting soundSetting = SoundSetting.GetDefault();
        soundSetting.Loop = isLoop;
        soundSetting.Volume = volume;
        soundSetting.Play(source);
    }

    public void PlaySound(AudioClip[] audioClips,  bool isLoop = false, float volume = 1f, SoundVolumeType volumeType = SoundVolumeType.SOUNDFX_VOLUME)
    {
        AudioClip targetClip = audioClips[Random.Range(0, audioClips.Length)];
        PlaySound(targetClip, isLoop, volume, volumeType);
    }
    
    public void PlaySound(AudioClip audioClip, SoundSetting setting, Transform parent = null)
    {
        AudioChild audioChild = ObjectPool.Instance.SpawnObject(_audioPrefab,
            default, default, PoolType.Audio).GetComponent<AudioChild>();
        audioChild.transform.localPosition = Vector3.zero;

        if (parent)
        {
            audioChild.transform.SetParent(parent, false);
        }
        
        AudioSource source = audioChild.Source;
        source.clip = audioClip;
        source.outputAudioMixerGroup = GetMixerGroup(setting.VolumeType);
        setting.Play(source);
        //audioChild.WaitToReturnPool();
    }

    public void PlaySound(AudioClip[] audioClips, SoundSetting setting, Transform parent = null)
    {
        AudioClip targetClip = audioClips[Random.Range(0, audioClips.Length)];
        PlaySound(targetClip, setting, parent);
    }

    private AudioMixerGroup GetMixerGroup(SoundVolumeType volumeType)
    {
        switch (volumeType)
        {
            case SoundVolumeType.MASTER_VOLUME:
                return _masterGroup;
            case SoundVolumeType.BGM_VOLUME:
                return _bgGroup;
            case SoundVolumeType.SOUNDFX_VOLUME:
                return _fxGroup;
            case SoundVolumeType.UI_VOLUME:
                return _uiGroup;
            default:
                return null;
        }
    }
    
    private void SetVolume(string parameter, float value)
    {
        value = (float)Math.Clamp(value, 0.0001, 1);
        _mixer.SetFloat(parameter, (float)Math.Log10(value) * 20f);
    }
    public void SetMasterVolume(float volume)
    {
        SetVolume(_masterVolume, volume);
    }
    public void SetGameVolume(float volume)
    {
        SetVolume(_fxVolume, volume);
    }
    public void SetMusicVolume(float volume)
    {
        SetVolume(_bgVolume, volume);
    }
    public void SetUIVolume(float volume)
    {
        SetVolume(_uiVolume, volume);
    }
}

public enum SoundVolumeType
{
    MASTER_VOLUME,
    BGM_VOLUME,
    SOUNDFX_VOLUME,
    UI_VOLUME
}

public struct SoundSetting
{
    public float Volume;
    public float Pitch;
    public float StereoPan;
    public float SpatialBlend;
    public float MinDistance;
    public float MaxDistance;
    public bool Loop;
    public AudioRolloffMode RolloffMode;
    public SoundVolumeType VolumeType;
    
    public void Play(AudioSource source)
    {
        source.volume = Volume;
        source.pitch = Pitch;
        source.panStereo = StereoPan;
        source.spatialBlend = SpatialBlend;
        source.loop = Loop;
        source.minDistance = MinDistance;
        source.maxDistance = MaxDistance;
        source.rolloffMode = RolloffMode;
        source.Play();
    }

    public static SoundSetting GetDefault()
    {
        return new SoundSetting()
        {
            Volume = 1,
            Pitch = 1,
            StereoPan = 0,
            SpatialBlend = 0,
            Loop = false,
            VolumeType = SoundVolumeType.SOUNDFX_VOLUME
        };
    }
}
