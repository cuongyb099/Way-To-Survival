using System;
using System.Collections.Generic;
using System.Linq;

using Cysharp.Threading.Tasks;
using Tech.Singleton;
using UnityEngine;

namespace KatInventory
{
    public class ItemDataBase : Singleton<ItemDataBase>
    {
        private Dictionary<string, ItemBaseSO> _itemDictionary;
    #if UNITY_EDITOR
        //I Don't Want this variable Can Get When Game Build It Not Compile If Access It Game Build Failure
        //Only Use For Debug
        public Dictionary<string,ItemBaseSO> ItemDictionary => _itemDictionary;
        
    #endif
        
        private static string _itemAddressKey = "Inventory";
        public static Action OnLoadDone;
        
        protected override void Awake()
        {
            base.Awake();
            _ = LoadInventoryAsync();
        }

        private async UniTaskVoid LoadInventoryAsync()
        {
            while (!AddressablesManager.Instance)
            {
                await UniTask.Yield();
            }
            
            var items = await AddressablesManager.Instance.LoadAssetsAsync<ItemBaseSO>(_itemAddressKey);
            _itemDictionary = items.ToDictionary(item => item.ID);
            OnLoadDone?.Invoke();
        }
        
        public ItemBaseSO SearchItem(string id)
        {
            return _itemDictionary.GetValueOrDefault(id);
        }

        public List<ItemBaseSO> GetItemsWithType(ItemType itemType)
        {
            List<ItemBaseSO> items = new List<ItemBaseSO>();
            
            foreach (var value in _itemDictionary.Values)
            {
                if(value.GetItemType() != itemType) continue;
                    
                items.Add(value);
            }
            
            return items;
        }
    }
}