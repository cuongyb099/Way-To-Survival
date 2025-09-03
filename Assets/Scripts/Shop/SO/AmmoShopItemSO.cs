
using UnityEngine;

[CreateAssetMenu(fileName = "AmmoShopItemSO",menuName ="Item/Shop/new AmmoShopItemSO")]
public class AmmoShopItemSO : ShopBuyItemSO
{
    [field: Header("Ammo Data")]
    [field:Range(0f,1f),SerializeField] public float AmmoPercentageAmount { get; private set; }
    [field: SerializeField] public float GiveAmmoAmount { get; private set; }

    public override void OnBuyItem()
    {
        if (GiveAmmoAmount > 0)
        {
            Stat stat = GameManager.Instance.Player.Stats.GetStat(StatType.MaxBulletPoints);
            stat.AddModifier(new StatModifier(GiveAmmoAmount,StatModType.Percentage));
        }
        Attribute attribute = GameManager.Instance.Player.Stats.GetAttribute(AttributeType.HoldingBullets);
        attribute.Value += attribute.MaxValue * AmmoPercentageAmount;
    }
}
