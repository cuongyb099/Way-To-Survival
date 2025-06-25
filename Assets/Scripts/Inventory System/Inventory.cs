using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KatInventory
{
    [RequireComponent(typeof(ItemDataBase))]
    public class Inventory : Tech.Singleton.Singleton<Inventory>
    {
        [field: SerializeField, Range(1, 1000)]
        public int Capacity { get; private set;} = 99;

        [SerializeField]private List<ItemData> _inventory = new ();
        public List<ItemData> DataRuntime => _inventory;

        public static Action OnInventoryChange;
        public static Action OnAddItem;

        private ItemData FindFirstItemNotFullStack(ItemBaseSO itemBase)
        {
            for (int i = 0; i < _inventory.Count; i++)
            {
                var data = _inventory[i];
                var staticData = data.StaticData;
                if (staticData != itemBase || data.Quantity == staticData.MaxStack) continue;
                    
                return data;
            }   
            
            return null;
        }
        public ItemData AddItem(ItemBaseSO itemBase, int quantity = 1)
        {
            if (!itemBase || quantity < 1) return null;
            
            var existItemData = FindFirstItemNotFullStack(itemBase);
            
            if (itemBase.Unique && existItemData != null)
            {
                return null;
            }

            if ((itemBase.Unique || existItemData == null) && _inventory.Count < Capacity)
            {
                return AddNewItem(itemBase, quantity);
            }

            if (existItemData == null)
            {
                return null;
            }
            
            var resultQuantity = quantity + existItemData.Quantity;
            if (resultQuantity > itemBase.MaxStack)
            {
                existItemData.Quantity = itemBase.MaxStack;
                OnInventoryChange?.Invoke();
                return existItemData;
            }
            
            existItemData.Quantity += quantity;
            OnInventoryChange?.Invoke();
            return existItemData;
        }
        private ItemData AddNewItem(ItemBaseSO itemBase, int quantity = 1)
        {
            var newItemData = itemBase.CreateItemData(quantity);
            _inventory.Add(newItemData);
            OnInventoryChange?.Invoke();
            return newItemData;
        }
        public ItemData AddItem(string id, int quantity = 1)
        {
            if (id == string.Empty) return null;

            return AddItem(ItemDataBase.Instance.SearchItem(id), quantity);
        }
        public ItemData RemoveItem(ItemBaseSO itemBase, int quantity = 1)
        {
            if(!itemBase || quantity < 1) return null;

            
            for (int i = _inventory.Count - 1; i >= 0; i--)
            {
                var itemData = _inventory[i];
                
                if (itemData.StaticData != itemBase) continue;
                
                if (itemData.Quantity - quantity <= 0)
                {
                    _inventory.RemoveAt(i);
                    OnInventoryChange?.Invoke();
                    return itemData;
                }
                 
                itemData.Quantity -= quantity;
                OnInventoryChange?.Invoke();
                return itemData;
            }

            return null;
        }
        public ItemData RemoveItem(string id, int quantity = 1)
        {
            if (id == string.Empty) return null;
            
            return RemoveItem(ItemDataBase.Instance.SearchItem(id), quantity);
        }

        public void RemoveAllItemOfType<T>()
        {
            _inventory.RemoveAll(x => x is T);
            OnInventoryChange?.Invoke();
        }

        public void RemoveItems(ItemType type)
        {
            _inventory.RemoveAll(x => x.StaticData.GetItemType() == type);
            OnInventoryChange?.Invoke();
        }
        
        public bool Has(ItemBaseSO itemBaseSo)
        {
            for (int i = 0; i < _inventory.Count; i++)
            {
                var item = _inventory[i];
                if (item.StaticData == itemBaseSo)
                    return true;
            }

            return false;
        }
        
        public bool Has(string id)
        {
            for (int i = 0; i < _inventory.Count; i++)
            {
                var item = _inventory[i];
                if (item.StaticData.ID == id)
                    return true;
            }

            return false;
        }

        public List<ItemData> GetItems(ItemType itemType)
        {
            List<ItemData> result = new();
            for (int i = 0; i < _inventory.Count; i++)
            {
                var item = _inventory[i];
                
                if (item.StaticData.GetItemType() != itemType) continue;
                
                result.Add(item);
            }
            return result;
        }

        public List<T> GetItemsOfType<T>() where T : ItemData
        {
            Debug.Log(_inventory.OfType<T>().ToList().Count);
            return _inventory.OfType<T>().ToList();
        }

        public void Clear()
        {
            _inventory.Clear();
            OnInventoryChange?.Invoke();
        }
        public void Save(PlayerSaveData data)
        {
            
        }
        public void Load(PlayerSaveData data)
        {
            foreach (var item in data.Inventory)
            {
                AddItem(item.Item, item.Quantity);
            }
        }
    }
}