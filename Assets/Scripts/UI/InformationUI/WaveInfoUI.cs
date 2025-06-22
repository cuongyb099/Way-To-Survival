using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class WaveInfoUI : MonoBehaviour
{
    [SerializeField] private float showWaveUIDuration = 3f;
    [SerializeField] private AnimationCurve showAnimationCurve;
    [Header("Wave Info")]
    [SerializeField] private TextMeshProUGUI textWave;
    [SerializeField] private TextMeshProUGUI textTime;
    [SerializeField] private CanvasGroup waveCanvasGroup;
    [Header("Special Event Info")]
    [SerializeField] private TextMeshProUGUI textBuff;
    [SerializeField] private CanvasGroup eventCanvasGroup;
    
    [SerializeField] private TimeSliderFade timerSlider;
    
    [SerializeField] private AudioClip waveClip;
    [SerializeField] private AudioClip[] buffClips;
    private void Awake()
    {
        GameEvent.OnInitializedUI += ShowWaveInfo;
        GameEvent.OnStartCombatState += PlaySoundSupense;
        GameEvent.OnStartShoppingState += ShowWaveInfo;
    }
    private void OnDestroy()
    {
        GameEvent.OnInitializedUI -= ShowWaveInfo;
        GameEvent.OnStartCombatState -= PlaySoundSupense;
        GameEvent.OnStartShoppingState -= ShowWaveInfo;
    }
    private void PlaySoundSupense()
    {
        AudioManager.Instance.PlaySound(waveClip,volumeType: SoundVolumeType.SOUNDFX_VOLUME);
    }
    private void PlaySoundBadBuff()
    {
        AudioManager.Instance.PlaySound(buffClips,volumeType: SoundVolumeType.SOUNDFX_VOLUME);
    }

    private void ShowEventInfo()
    {
        if (BuffEventManager.Instance.PositiveBuffListSO.Count > 0)
        {
            textBuff.text = BuffEventManager.Instance.PositiveBuffListSO[0].Description.GetLocalizedString();
            PlaySoundSupense();
        }
        if (BuffEventManager.Instance.NegativeBuffListSO.Count > 0)
        {
            textBuff.text = BuffEventManager.Instance.NegativeBuffListSO[0].Description.GetLocalizedString();
            PlaySoundBadBuff();
        }
        
        
        eventCanvasGroup.blocksRaycasts = true;
        DOVirtual.Float(1, 0, showWaveUIDuration+2, x =>
        {
            eventCanvasGroup.alpha = x;
        }).OnComplete(() =>
        {
            eventCanvasGroup.blocksRaycasts = false;
            timerSlider.FadeIn();
        }).SetEase(showAnimationCurve);
    }
    private void ShowWaveInfo()
    {
        textTime.text = GetTimeString();
        textWave.text = $"Wave {EnemyManager.Instance.GetCurrentWave()}";
        PlaySoundSupense();
        
        waveCanvasGroup.blocksRaycasts = true;
        DOVirtual.Float(1, 0, showWaveUIDuration, x =>
        {
            waveCanvasGroup.alpha = x;
        }).OnComplete(() =>
        {
            waveCanvasGroup.blocksRaycasts = false;
            if (BuffEventManager.Instance.PositiveBuffListSO.Count > 0|| BuffEventManager.Instance.NegativeBuffListSO.Count > 0)
            {
                ShowEventInfo();
            }
            else
                timerSlider.FadeIn();
        }).SetEase(showAnimationCurve);
    }
    private void HideWaveInfo()
    {
        waveCanvasGroup.alpha = 1;
        waveCanvasGroup.blocksRaycasts = false;
    }
    private string GetTimeString()
    {
        float time = TimeManager.Instance.TimeOfDay;
        if (time > 12f)
        {
            time -= 12f;
            return $"{time}:00 PM";
        }
        return $"{time}:00 AM";
    }
}
