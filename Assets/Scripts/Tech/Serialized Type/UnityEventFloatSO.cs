using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace KatLib.Serialized_Type
{
    [CreateAssetMenu(menuName = "SO/Serialized Type/Event/Event Float")]
    public class UnityEventFloatSO : SerializedTypeSO<UnityEvent<float>>
    {
#if UNITY_EDITOR
        void OnDisable()
        {
            string path = AssetDatabase.GetAssetPath(this);
            string evtName = "Evt_F_";
            var replace = this.name.Replace(evtName,"");
            AssetDatabase.RenameAsset(path, evtName + replace + ".asset");
            AssetDatabase.SaveAssets();
        }
#endif
    }
}
