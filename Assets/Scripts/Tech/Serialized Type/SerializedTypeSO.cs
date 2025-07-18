using UnityEngine;

namespace KatLib.Serialized_Type
{
    public abstract class SerializedTypeSO<T> : ScriptableObject
    {
        [SerializeField] protected T value;
        public virtual T Value => value;
    }
}
