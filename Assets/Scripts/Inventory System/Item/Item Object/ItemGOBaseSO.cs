using Tech.Logger;
using UnityEngine;

namespace KatInventory
{
    public abstract class ItemGOBaseSO : ItemBaseSO
    {
        [field: SerializeField]
        public ItemBase Prefab  { get; private set; }

        public ItemData CreateNewItemInWorld(int quantity = 1,Transform parent = null)
        {
            if (!Prefab)
            {
                LogCommon.LogError("Prefab Is Null");
                return null;
            }
            
            var item = Instantiate(Prefab, parent? parent:Inventory.Instance.transform); 
            var data = CreateItemData(quantity, item.gameObject);
            item.SetData((ItemGOData)data);
            item.gameObject.SetActive(false);
            return data;
        }
        public ItemData CreateExistingItemInWorld(int quantity = 1,Transform parent = null, ItemData data = null)
        {
            if (!Prefab)
            {
                LogCommon.LogError("Prefab Is Null");
                return null;
            }
            
            var item = Instantiate(Prefab, parent? parent:Inventory.Instance.transform); 
            ItemGOData itemGOData = (ItemGOData)data;
            item.SetData(itemGOData);
            itemGOData.SetReference(item.gameObject);
            item.gameObject.SetActive(false);
            return data;
        }
    }
}