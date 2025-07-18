using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace KatLib.Serialized_Type
{
    [CreateAssetMenu(menuName = "SO/Serialized Type/Event/Event Int")]
    public class UnityEventIntSO : SerializedTypeSO<UnityEvent<int>> 
    {
#if UNITY_EDITOR
        void OnDisable()
        {
            string path = AssetDatabase.GetAssetPath(this);
            string evtName = "Evt_Int_";
            var replace = this.name.Replace(evtName,"");
            AssetDatabase.RenameAsset(path, evtName + replace + ".asset");
            AssetDatabase.SaveAssets();
        }
#endif
    }
}
