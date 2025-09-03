#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace KatLib.Observer.Editor
{
    [CustomEditor(typeof(Observer), true)]
    public class ObserverEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            Observer observer = (Observer)target;
            
            serializedObject.Update();
            EditorGUILayout.LabelField("Debug", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("removeNamespace"));
            
            if (observer.Observers == null || observer.Observers.Count == 0)
            {
                EditorGUILayout.LabelField("No Event Register", EditorStyles.boldLabel);
                serializedObject.ApplyModifiedProperties();
                return;
            }

            
            foreach (var debug in observer.EditorDebug)
            {
                int id = Convert.ToInt32(debug.EnumValue) ^ debug.EnumValue.GetType().GetHashCode();
                int count = 0;
                
                if (observer.Observers.TryGetValue(id, out var function))
                {
                    if(function == null) return;
                    count = function.GetInvocationList().Length;
                }
                
                string typeText = debug.EnumValue.GetType().ToString();
                string typeTitle = debug.Param != null ? $"{debug.Param}" : string.Empty;

                if (observer.removeNamespace)
                {
                    typeTitle = RemoveNameSpace(typeTitle);
                    typeText = RemoveNameSpace(typeText);
                }

                if (!string.IsNullOrEmpty(typeTitle))
                {
                    typeTitle = '<' + typeTitle + '>';
                }
                
                EditorGUILayout.LabelField($"Type: {typeText} " + 
                                           $"| Name: {debug.EnumValue}" + typeTitle +
                                           $" | Count : {count}", EditorStyles.boldLabel);
            }
            serializedObject.ApplyModifiedProperties();
        }

        public string RemoveNameSpace(string title)
        {
            return title.Substring(title.LastIndexOf('.') + 1);
        }
    }
}
#endif