using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusBarUI : MonoBehaviour
{
    public Image HeathBarProgress;
    public Image HeathFollowBar;
    
    public TextMeshProUGUI TextHeath;
    
    private float _offsetHeathText;
    private float _offsetManaText;
    
    private Tween[] _tweens = new Tween[2];
    
    [SerializeField] private string _colorHex = ColorUtility.ToHtmlStringRGBA(new Color(1, 1, 1, 0.3f));
    [SerializeField] private float offsetPercent = 0.135f;
    [SerializeField] private float _delayTimeBeforeFollow = 0.25f;
    [SerializeField] private float _lerpTime = 0.25f;
    
    private void Awake()
    {                
        CalculateOffset();
        
        PlayerEvent.OnHeathChange += ChangeHpBar;
        PlayerEvent.OnMaxHeathChange += ChangeHpWithoutAnimation;
        PlayerEvent.OnInitStatusBar += InitBar;
        
        Attribute attribute = GameManager.Instance.Player.Stats.GetAttribute(AttributeType.Hp);
        ChangeHpWithoutAnimation(attribute.Value,attribute.MaxValue);
    }

    private void ChangeHpWithoutAnimation(float arg1, float arg2)
    {
        TextHeath.text =$"{(int)arg1} <size=80%><voffset={_offsetHeathText}><color=#{_colorHex}>/ {(int)arg2}</color></voffset></size>";
        var ratio = arg1 / arg2;
        HeathBarProgress.fillAmount = ratio;
        HeathFollowBar.fillAmount = ratio;
    }

    private void InitBar(AttributeType attribute,float curValue, float maxValue)
    {
        switch (attribute)
        {
            case AttributeType.Hp:
                ChangeHpWithoutAnimation(curValue, maxValue);
                return;
            default:
                return;
        }
    }
    
    private void CalculateOffset()
    {
        var textHeight = TextHeath.preferredHeight;
        _offsetHeathText = textHeight * offsetPercent;
    }
    
    private void OnDestroy()
    {
        PlayerEvent.OnHeathChange -= ChangeHpBar;
        PlayerEvent.OnInitStatusBar -= InitBar;
        PlayerEvent.OnMaxHeathChange -= ChangeHpWithoutAnimation;
    }

    private void ChangeHpBar(float curHp, float maxHp)
    {
        DoAnimationBar(curHp, maxHp, _offsetHeathText, HeathBarProgress, HeathFollowBar, TextHeath, 0);
    }

    private void DoAnimationBar(float curValue, float maxValue, float offset,Image progressBar, Image followBar, TextMeshProUGUI textChange, int tweenIndex)
    {
        _tweens[tweenIndex].Kill();
        
        textChange.text =
            $"{(int)curValue} <size=70%><voffset={offset}><color=#{_colorHex}>/ {(int)maxValue}</color></voffset></size>";
        
        var targetFillAmount = curValue / maxValue;
        var curFillAmount = progressBar.fillAmount;

        if (curFillAmount > targetFillAmount)
        {
            _tweens[tweenIndex] = DOVirtual.DelayedCall(_delayTimeBeforeFollow, () =>
            {
                _tweens[tweenIndex] = DOVirtual.Float(followBar.fillAmount, targetFillAmount, _lerpTime, (value) =>
                {
                    followBar.fillAmount = value;
                });
            });
            
            progressBar.fillAmount = targetFillAmount;
            return;
        }
        
        followBar.fillAmount = targetFillAmount;
        _tweens[tweenIndex] = DOVirtual.DelayedCall(_delayTimeBeforeFollow, () =>
        {
            _tweens[tweenIndex] = DOVirtual.Float(progressBar.fillAmount, targetFillAmount, _lerpTime, (value) =>
            {
                progressBar.fillAmount = value;
            });
        });
    }
}
