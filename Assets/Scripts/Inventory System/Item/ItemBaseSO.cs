using System;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Localization;

namespace KatInventory
{
    [JsonConverter(typeof(ItemSoConverter))]
    public abstract class ItemBaseSO : ScriptableObject, IEquatable<ItemBaseSO>
    {
        public string ID => name;
        [field:Header("Item Data")]
        [field: SerializeField] public LocalizedString Name { get; private set; }
        [field: SerializeField] public LocalizedString Description { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        //Unique Is Item Only Exist 1 In Inventory difference 1 Maxstack
        [field: SerializeField] public virtual bool Unique { get; protected set; }
        [field: SerializeField] public virtual int BuyPrice { get; private set; }
        [field: SerializeField] public virtual int SellPrice { get; private set; }
        [field: SerializeField, Range(1, 1000000)] 
        public virtual int MaxStack { get; protected  set; } = 1;
        [field: SerializeField] public virtual Manipulator[] Modifiers { get; private set; }
        [field: SerializeField] public Rarity Rarity { get; protected set; }
        public virtual ItemData CreateItemData(int quantity = 1,GameObject prefab = null)
        {
            return new ItemData(this, Mathf.Clamp(quantity,0, MaxStack));
        }

        public virtual void Use(GameObject user)
        {
            Inventory.Instance.RemoveItem(this);
        }
        public abstract ItemType GetItemType();
        public bool Equals(ItemBaseSO other)
        {
            if (!other) return false;
            return ID == other.ID;
        }
    }

    public enum ItemType
    {
        Weapon,
        Building,
        Buffs,
        UpgradeMaterials,
    }
    public enum Rarity
    {
        Common,
        Uncommon,
        Rare,
        ExtremelyRare,
        Myth,
    }
    public enum Manipulator
    {
        Sell,
        Buy,
        Upgrade,
    }
}