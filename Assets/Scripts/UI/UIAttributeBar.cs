using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class UIAttributeBar : MonoBehaviour
{
    public Image TargetImage;
    public StatsController Stats;
    public AttributeType Attribute;
    private void Awake()
    {
        _ = InitAsync();
    }

    private async UniTaskVoid InitAsync()
    {
        await UniTask.Yield();
        var att = Stats.GetAttribute(Attribute);
        TargetImage.fillAmount = att.Value / att.MaxValue;
        att.OnValueChange += () =>
        {
            TargetImage.fillAmount = att.Value / att.MaxValue;
        };
    }
}
