using System;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Serialization;

namespace KatInventory
{
    
    [System.Serializable]
    public class ItemData
    {
        [JsonIgnore]
        public Action<int> OnChangeQuantity;
        public ItemData()
        {
            
        }
        public ItemData(ItemBaseSO staticData, int quantity)
        {
            _quantity = quantity;
            StaticData = staticData;
        }

        [field: SerializeField]
        [JsonProperty("ID",Order = 0)]
        public ItemBaseSO StaticData { get; private set; }
        [JsonIgnore]
        public string ID => StaticData.ID;
        [JsonIgnore]
        public int Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnChangeQuantity?.Invoke(_quantity);
            }
        }
        [JsonProperty("ItemQuantity", Order = 1)] 
        [SerializeField]
        private int _quantity;
    }
}
