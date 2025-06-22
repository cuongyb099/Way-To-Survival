using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TimeSliderFade : MonoBehaviour
{
    public float TransitionDuration = 0.5f;
    private CanvasGroup canvasGroup;
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        GameEvent.OnStopShoppingState += FadeOut;
    }

    private void OnDestroy()
    {
        GameEvent.OnStopShoppingState -= FadeOut;
    }

    private Tweener a, b;
    [ContextMenu("Fade In")]
    public void FadeIn()
    {
        canvasGroup.blocksRaycasts = true;
        DOVirtual.Float(0, 1, TransitionDuration, x => canvasGroup.alpha = x);
    }
    [ContextMenu("Fade Out")]
    public void FadeOut()
    {
        canvasGroup.blocksRaycasts = false;
        DOVirtual.Float(1, 0, TransitionDuration, x => canvasGroup.alpha = x);
    }
    
}
