using System;
using System.Runtime.Serialization;
using JsonSubTypes;
using Newtonsoft.Json;
using UnityEngine;

namespace KatInventory
{
    // [JsonConverter(typeof(JsonSubtypes))]
    // [JsonSubtypes.KnownSubTypeWithProperty(typeof(WeaponData), "WeaponLevel")]
    [Serializable]
    public class ItemData
    {
        [JsonIgnore]
        [field: SerializeField] public ItemBaseSO StaticData { get; private set; }
        
        [JsonIgnore]
        public Action<int> OnChangeQuantity;
        
        [JsonProperty("ID",Order = 0)]
        public string ID => StaticData.ID;

        [JsonIgnore]
        public int Quantity 
        { 
            get => quantity;
            set
            {
                if (value > StaticData.MaxStack)
                {
                    quantity = StaticData.MaxStack;
                    OnChangeQuantity?.Invoke(quantity);
                    return;
                }

                if (value < 0)
                {
                    quantity = 0;
                    OnChangeQuantity?.Invoke(quantity);
                    return;
                }
                
                quantity = value;
                OnChangeQuantity?.Invoke(quantity);
            }
        }
        
        [JsonProperty("ItemQuantity", Order = 1)]
        [SerializeField] 
        protected int quantity;
        
        [JsonConstructor]
        public ItemData(string ID, int quantity)
        {
            StaticData = ItemDataBase.Instance.SearchItem(ID);
            Quantity = quantity;
        }
        public ItemData(ItemBaseSO staticData, int quantity)
        {
            StaticData = staticData;
            Quantity = quantity;
        }
    }
    public enum SerializeItemType
    {
        DefaultItem = 1,
        Weapon = 2,
        Gun = 3,
        Turret = 4,
    }
}
