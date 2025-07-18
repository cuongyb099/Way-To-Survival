#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

namespace KatLib.Serialized_Type
{
    [CreateAssetMenu(menuName = "SO/Serialized Type/String")]
    public class StringSO : SerializedTypeSO<string>
    {

    }
}
