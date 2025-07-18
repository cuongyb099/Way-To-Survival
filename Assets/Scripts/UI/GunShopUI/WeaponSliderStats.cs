
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSliderStats: MonoBehaviour
{
    private static float animationTime = 0.2f;
    [field:SerializeField] public TextMeshProUGUI Text { get; private set; }
    [field:SerializeField] public Slider Slider { get; private set; }

    public void ChangeStat(float value, float min, float max, string textFormat)
    {
        Text.text = value.ToString(textFormat);
        Slider.minValue = min;
        Slider.maxValue = max;
        
        DOVirtual.Float(Slider.value, value, animationTime, (x) => { Slider.value = x;}).SetUpdate(true);
    }
}
