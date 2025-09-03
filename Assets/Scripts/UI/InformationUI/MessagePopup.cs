using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Tech.Singleton;
using TMPro;
using UnityEngine;

public class MessagePopup : SingletonPersistent<MessagePopup>
{
    [field:SerializeField] public float TransitionDuration { get; set; }
    [field:SerializeField] public float MessageDuration { get; set; }
    [SerializeField] private TextMeshProUGUI messageText;
    private CanvasGroup canvasGroup;
    private Sequence sequence;
    protected override void Awake()
    {
        base.Awake();
        canvasGroup = GetComponentInChildren<CanvasGroup>();
    }

    public void ShowMessage(string message)
    {
        messageText.text = message;
        if(sequence != null) sequence.Kill();
        sequence = DOTween.Sequence();
        sequence.Append( DOVirtual.Float(0, 1, TransitionDuration, x => canvasGroup.alpha = x));
        sequence.AppendInterval(MessageDuration);
        sequence.Append( DOVirtual.Float(1, 0, TransitionDuration, x => canvasGroup.alpha = x));
    }

}
