using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CanvasFadeHelper : MonoBehaviour
{
    public float TransitionDuration = 0.5f;
    private CanvasGroup canvasGroup;
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private Tweener a, b;
    [ContextMenu("Fade In")]
    public void FadeIn(TweenCallback onComplete = null)
    {
        canvasGroup.blocksRaycasts = true;
        DOVirtual.Float(0, 1, TransitionDuration, x => canvasGroup.alpha = x).SetUpdate(true).OnComplete(onComplete);
    }
    [ContextMenu("Fade Out")]
    public void FadeOut(TweenCallback onComplete = null)
    {
        canvasGroup.blocksRaycasts = false;
        DOVirtual.Float(1, 0, TransitionDuration, x => canvasGroup.alpha = x).SetUpdate(true).OnComplete(onComplete);
    }
    
}