using System.Collections;
using DG.Tweening;
using Tech.Singleton;
using UnityEngine;

public class BlindPlayer : Singleton<BlindPlayer>
{
    [SerializeField]private float blindInOutDuration;
    private CanvasGroup canvasGroup;
    private Coroutine holder;
    private bool blinded = false;
    protected override void Awake()
    {
        base.Awake();
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = false;
    }
    [ContextMenu("blind")]
    public void Blind(float duration = 5f)
    {
        if (!blinded)
        {
            DOVirtual.Float(0, 1, blindInOutDuration, x =>
            {
                canvasGroup.alpha = x;
            });
            blinded = true;
        }
        
        if (holder != null)
        {
            StopCoroutine(holder);
        }
        holder = StartCoroutine(Countdown(duration));
    }
    [ContextMenu("unblind")]
    public void UnBlind()
    {
        if (holder != null)
        {
            StopCoroutine(holder);
        }
        DOVirtual.Float(1, 0, blindInOutDuration, x =>
        {
            canvasGroup.alpha = x;
        });
        blinded = false;
    }

    IEnumerator Countdown(float duration)
    {
        yield return new WaitForSeconds(duration);
        UnBlind();
    }
}
