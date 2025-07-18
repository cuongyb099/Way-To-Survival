using UnityEngine;

namespace KatInventory
{
    public abstract class ItemGOBaseSO : ItemBaseSO
    {
        [field: SerializeField]
        public GameObject Prefab  { get; private set; }
    }
}