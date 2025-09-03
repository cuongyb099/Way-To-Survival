using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Tech.Singleton;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum ConfirmPopUpType
{
    YesNo,
    Confirm,
}
public class ConfirmPopUp : SingletonPersistent<ConfirmPopUp>
{
    [SerializeField] private CanvasGroup canvasGroup; 
    [SerializeField] private TextMeshProUGUI Header; 
    [SerializeField] private TextMeshProUGUI Message; 
    
    [SerializeField] private GameObject YesNoScreen;
    [SerializeField] private GameObject ConfirmScreen;
    
    [SerializeField] private Button YesButton;
    [SerializeField] private Button NoButton; 
    [SerializeField] private Button ConfirmButton;
    
    [SerializeField] private float FadeDuration = 0.2f;
    private Tweener tweener;
    
    public void Show(ConfirmPopUpType type = ConfirmPopUpType.YesNo, string header = "Header", string message = "Message", UnityAction confirmAction = null, UnityAction cancelAction= null)
    {
        Header.text = header;
        Message.text = message;
        switch (type)
        {
            case ConfirmPopUpType.YesNo:
                YesNoScreen.SetActive(true);
                ConfirmScreen.SetActive(false);
                
                YesButton.onClick.RemoveAllListeners();
                NoButton.onClick.RemoveAllListeners();
                if (confirmAction != null)YesButton.onClick.AddListener(confirmAction);
                if (cancelAction != null) NoButton.onClick.AddListener(cancelAction);
                YesButton.onClick.AddListener(FadeOut);
                NoButton.onClick.AddListener(FadeOut);
                break;
            case ConfirmPopUpType.Confirm:
                ConfirmScreen.SetActive(true);
                YesNoScreen.SetActive(false);
                ConfirmButton.onClick.RemoveAllListeners();
                if (confirmAction != null) ConfirmButton.onClick.AddListener(confirmAction);
                
                ConfirmButton.onClick.AddListener(FadeOut);
                break;
        }
        FadeIn();
    }

    public void FadeIn()
    {
        if(tweener != null) tweener.Kill();
        tweener =  DOVirtual.Float(0, 1, FadeDuration, x => canvasGroup.alpha = x).OnComplete(()=>canvasGroup.blocksRaycasts = true);
    }
    public void FadeOut()
    {
        if(tweener != null) tweener.Kill();
        canvasGroup.blocksRaycasts = false;
        tweener =  DOVirtual.Float(1, 0, FadeDuration, x => canvasGroup.alpha = x);
    }
}
