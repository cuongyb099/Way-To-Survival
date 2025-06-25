using Newtonsoft.Json;
using UnityEngine;

namespace KatInventory
{
    [System.Serializable]
    public class ItemGOData : ItemData
    {
        public ItemGOData(ItemBaseSO staticData, int quantity) : base(staticData, quantity)
        {
            
        }
        
        [JsonIgnore]
        protected GameObject goReference;

        [JsonIgnore]
        public virtual GameObject GoReference
        {
            get
            {
                if (goReference) return goReference;
                var data = this.StaticData as ItemGOBaseSO;
                goReference = Object.Instantiate(data.Prefab).gameObject;
                goReference.GetComponent<ItemBase>().SetData(this);
                return goReference;
            }
        }
    }
}