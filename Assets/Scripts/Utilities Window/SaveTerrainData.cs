using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
public class SaveTerrainData : EditorWindow
{
    [MenuItem("Tools/Delete Animator")]
    static void CreateReplaceWithPrefab()
    {
        EditorWindow.GetWindow<SaveTerrainData>();
    }
    
    private void OnGUI()
    {
        if (GUILayout.Button("Save"))
        {
            var selections = Selection.gameObjects;
            foreach (var selection in selections)
            {
                foreach (Animator animator in selection.transform.GetComponentsInChildren<Animator>())
                {
                    DestroyImmediate(animator);
                }
            }
        }
    }
}
#endif