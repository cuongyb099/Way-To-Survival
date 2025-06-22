using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class FadeUI : MonoBehaviour
{
    [SerializeField] protected CanvasGroup canvasGroup;
    protected Tween fadeTween;
        
    private void Reset()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Awake()
    {
        if(!canvasGroup)
            canvasGroup = GetComponent<CanvasGroup>();
    }
    
    public void FadeIn(float duration, TweenCallback onComplete)
    {
        canvasGroup.blocksRaycasts = false;
        onComplete += () => canvasGroup.blocksRaycasts = true;
        if(fadeTween != null && fadeTween.IsActive()) fadeTween.Kill();
        fadeTween = canvasGroup.DOFade(1f, duration)
            .SetEase(Ease.Linear)
            .OnComplete(onComplete);
    }

    public void FadeOut(float duration, TweenCallback onComplete)
    {
        canvasGroup.blocksRaycasts = false;
        if(fadeTween != null && fadeTween.IsActive()) fadeTween.Kill();
        fadeTween = canvasGroup.DOFade(0f, duration)
            .SetEase(Ease.Linear)
            .OnComplete(onComplete);
    }

    public void FadeIn(float duration)
    {
        canvasGroup.blocksRaycasts = false;
        if(fadeTween != null && fadeTween.IsActive()) fadeTween.Kill();
        fadeTween = canvasGroup.DOFade(1f, duration)
            .SetEase(Ease.Linear)
            .OnComplete(() => canvasGroup.blocksRaycasts = true);
    }
    
    public void FadeOut(float duration)
    {
        canvasGroup.blocksRaycasts = false;
        if(fadeTween != null && fadeTween.IsActive()) fadeTween.Kill();
        fadeTween = canvasGroup.DOFade(0f, duration)
            .SetEase(Ease.Linear);
    }
    
    public void Hide()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
    }

    public void Show()
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }
}