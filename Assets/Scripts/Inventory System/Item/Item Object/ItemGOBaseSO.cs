using UnityEngine;

namespace KatInventory
{
    public abstract class ItemGOBaseSO : ItemBaseSO
    {
        [field: SerializeField]
        public ItemBase Prefab  { get; private set; }
    }
}