using UnityEngine;

namespace KatInventory.Sample
{
    [CreateAssetMenu(fileName = "Sword SO", menuName = "Sword SO")]
    public class SwordSO : ItemGOBaseSO
    {
        public override ItemType GetItemType() => ItemType.Weapon;
        
        //It Is Sample Please Don't Declare Variable like this with readonly variable
        public float DamageSample = 10f;
        public override ItemData CreateItemData(int quantity, GameObject prefab)
        {
            return new SwordData(this, Mathf.Clamp(quantity, 0, MaxStack), prefab);
        }
    }
}