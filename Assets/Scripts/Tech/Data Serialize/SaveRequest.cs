using UnityEngine;

namespace KatLib.Data_Serialize
{
    public class SaveRequest : MonoBehaviour
    {
        protected ISaveable[] saveableObjects;
        
        private void Awake()
        {
            saveableObjects = GetComponentsInChildren<ISaveable>();
            DataSerialize.OnSave += HandleOnSave;
        }

        private void OnDestroy()
        {
            DataSerialize.OnSave += HandleOnSave;
        }

        private void HandleOnSave()
        {
            foreach (var saveableObject in saveableObjects)
            {
                saveableObject.Save();           
            }            
        }
    }
}
