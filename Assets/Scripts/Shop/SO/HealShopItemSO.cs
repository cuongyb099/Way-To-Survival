
using UnityEngine;

[CreateAssetMenu(fileName = "HealShopItemSO",menuName ="Item/Shop/new HealShopItemSO")]
public class HealShopItemSO : ShopBuyItemSO
{
    [field: Header("Heal Data")]
    [field:Range(0f,1f),SerializeField] public float HealPercentageAmount { get; private set; }
    [field: SerializeField] public float GiveHealthAmount { get; private set; }

    public override void OnBuyItem()
    {
        if (GiveHealthAmount > 0)
        {
            Stat stat = GameManager.Instance.Player.Stats.GetStat(StatType.MaxHP);
            stat.AddModifier(new StatModifier(GiveHealthAmount,StatModType.Percentage));
        }
        Attribute attribute = GameManager.Instance.Player.Stats.GetAttribute(AttributeType.Hp);
        attribute.Value += attribute.MaxValue * HealPercentageAmount;
    }
}
