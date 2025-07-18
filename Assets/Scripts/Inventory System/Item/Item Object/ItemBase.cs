using UnityEngine;

namespace KatInventory
{
    public abstract class ItemBase : MonoBehaviour, IItemInstance
    {
        protected ItemGOData Data;

        public virtual void SetData(ItemData itemData)
        {
            Data = itemData as ItemGOData;
        }

        public virtual void Use()
        {
            
        }
    }
}
