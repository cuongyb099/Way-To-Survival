using Newtonsoft.Json;
using UnityEngine;

namespace KatInventory
{
    [System.Serializable]
    public class ItemGOData : ItemData
    {
        public ItemGOData(ItemBaseSO staticData, int quantity, GameObject go) : base(staticData, quantity)
        {
            GoReference = go;
        }
        [JsonIgnore]
        public GameObject GoReference { get; private set; }

        public void SetReference(GameObject go)
        {
            GoReference = go;
        }
    }
}