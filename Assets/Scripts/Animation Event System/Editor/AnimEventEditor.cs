using System;
using System.Collections.Generic;
using System.Linq;
using Core.Animation_Event_System.Event_Data;
using KatLib.Editor;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Core.Animation_Event_System.Editor
{
    [CustomPropertyDrawer(typeof(AnimEvent), true)]
    public class AnimEventEditor : PropertyDrawer
    {
        public float DefaultLineHeight = 18f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            string title = "None";
            Rect currentRect = new Rect(position.x, position.y, position.width, DefaultLineHeight);

            if (property.managedReferenceValue != null)
            {
                title = property.managedReferenceValue.GetType().Name;
            }

            if (EditorGUI.DropdownButton(currentRect, new GUIContent(title), FocusType.Passive))
            {
                OpenSearchWindow(property);
            }

            currentRect.y += DefaultLineHeight;

            if (property.managedReferenceValue != null)
            {
                var idProp = property.FindPropertyRelative("id");
                var timeProp = property.FindPropertyRelative("time");
                var valueProp = property.FindPropertyRelative("param");

                EditorGUI.PropertyField(currentRect, idProp);
                currentRect.y += DefaultLineHeight;

                EditorGUI.PropertyField(currentRect, timeProp);
                currentRect.y += DefaultLineHeight;

                if (valueProp != null)
                {
                    EditorGUI.PropertyField(currentRect, valueProp, true);
                }
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float totalHeight = 0;

            totalHeight += DefaultLineHeight;

            if (property.managedReferenceValue != null)
            {
                totalHeight += DefaultLineHeight;

                totalHeight += DefaultLineHeight;

                var valueProp = property.FindPropertyRelative("param");
                if (valueProp != null)
                {
                    totalHeight += EditorGUI.GetPropertyHeight(valueProp, true);
                }
            }

            return totalHeight;
        }

        private void OpenSearchWindow(SerializedProperty property)
        {
            var source = GetSubtypesOf(typeof(AnimEvent)).Select(x => x.AssemblyQualifiedName).ToList();
            var searchWindow = ScriptableObject.CreateInstance<SimpleSearchWindow>();
            searchWindow.Source = source.Select(RemoveNamespace).ToList();
            searchWindow.Title = "Anim Event";
            searchWindow.OnSelect += (index) =>
            {
                var type = Type.GetType(source[index]);
                if (type == null) return;

                property.managedReferenceValue = Activator.CreateInstance(type);
                property.serializedObject.ApplyModifiedProperties();
                AssetDatabase.SaveAssets();
            };
            SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)), searchWindow);
        }

        private static string RemoveNamespace(string name)
        {
            string removeAssembly = name.Remove(name.IndexOf(','));
            string removeNamespace = removeAssembly.Substring(removeAssembly.LastIndexOf('.') + 1);
            return removeNamespace;
        }

        private static List<Type> GetSubtypesOf(Type baseType)
        {
            var subtypes = new List<Type>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type != baseType && baseType.IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface)
                    {
                        subtypes.Add(type);
                    }
                }
            }

            return subtypes;
        }
    }
}