
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class ZFightingFix : EditorWindow
{
    [MenuItem("Tools/ZFightingFix")]
    static void ZFightingFixMenu()
    {
        EditorWindow.GetWindow<ZFightingFix>();
    }


    private void OnGUI()
    {
        var selection = Selection.gameObjects;
        if (GUILayout.Button("Replace"))
        {

            for (var i = selection.Length - 1; i >= 0; --i)
            {
                var selected = selection[i];

                var tmp = selected.transform.position;
                var begin = tmp;
                tmp.x += Random.Range(-0.001f, 0.001f);
                tmp.y += Random.Range(-0.001f, 0.001f);
                tmp.z += Random.Range(-0.001f, 0.001f);
                selected.transform.position = tmp;
            }
        }
        
        GUI.enabled = false;
        EditorGUILayout.LabelField("Selection count: " + Selection.objects.Length);
    }
}
#endif