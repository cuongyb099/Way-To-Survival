using UnityEngine;

namespace KatInventory
{
    public abstract class ItemBase : MonoBehaviour
    {
        public ItemGOData Data { get; protected set; }

        public void SetData(ItemGOData itemData)
        {
            Data = itemData;
        }

        //Not Always Item Use This Method So Not Abstract
        public virtual void Use()
        {
            
        }
    }
}
