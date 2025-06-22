using UnityEngine;

namespace Tech.Pooling
{
    public class ReturnToPool : MonoBehaviour
    {
        public PooledObject PoolsObjects;

        public void OnDisable()
        {
            PoolsObjects.AddToPool(gameObject);
        }
    }
}