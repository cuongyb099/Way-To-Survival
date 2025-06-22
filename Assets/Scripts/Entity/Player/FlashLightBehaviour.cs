using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLightBehaviour : MonoBehaviour
{
    [SerializeField] private Light flashLight; 
    [SerializeField] private AudioClip flashLightOn; 
    [SerializeField] private AudioClip flashLightOff; 
    private void Awake()
    {
        GameEvent.OnChangeTimeOfDay += FlashLightEvent;
    }

    private void OnDestroy()
    {
        GameEvent.OnChangeTimeOfDay -= FlashLightEvent;
    }

    private void FlashLightEvent(TimeOfTheDay timeOfTheDay)
    {
        switch (timeOfTheDay)
        {
            case TimeOfTheDay.Morning:
                TurnOff();
                break;
            case TimeOfTheDay.Evening:
                TurnOn();
                break;
        }
    }
    [ContextMenu("Turn on")]
    public void TurnOn()
    {
        flashLight.enabled = true;
        AudioManager.Instance.PlaySound(flashLightOn,volumeType:SoundVolumeType.SOUNDFX_VOLUME);
    }
    [ContextMenu("Turn off")]
    public void TurnOff()
    {
        flashLight.enabled = false;
        AudioManager.Instance.PlaySound(flashLightOff,volumeType:SoundVolumeType.SOUNDFX_VOLUME);
    }
}
