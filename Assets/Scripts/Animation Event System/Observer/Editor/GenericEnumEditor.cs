#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace KatLib.Observer.Editor
{
    [CustomPropertyDrawer(typeof(GenericEnum))]
    public class GenericEnumEditor : PropertyDrawer
    {
        private string[] includeAssembly;
        private string[] includeNamespace;
        private Type[] includeEnumType;
        private string[] excludeNamespace;
        private Type[] excludeEnumType;
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            SerializedProperty typeNameProp = property.FindPropertyRelative("_enumTypeName");
            SerializedProperty enumValueProp = property.FindPropertyRelative("EnumValue");

            includeAssembly = new[]{"Assembly-CSharp"};
            includeNamespace = null;
            includeEnumType = null;
            
            excludeNamespace = new[] 
            {
                "System",
                "UnityEditor",
                "UnityEngine.Experimental",
                "UnityEngine.Rendering",
                "UnityEngine.UIElements",
                "Newtonsoft.Json",
                "DG.Tweening",
                "TMPro"
            };
            excludeEnumType = null;

            var filterField = this.fieldInfo.GetCustomAttributes()
                .FirstOrDefault(x => x.GetType() == typeof(GenericEnumFilterAttribute));

            if (filterField != null)
            {
                var att = filterField as GenericEnumFilterAttribute;
                
                if (att.IncludeAssembly != null && att.IncludeAssembly.Length > 0)
                {
                    includeAssembly = att.IncludeAssembly;
                }
                
                if (att.IncludeNamespace != null && att.IncludeNamespace.Length > 0)
                {
                    includeNamespace = att.IncludeNamespace;
                }
                
                if (att.IncludeEnumType != null && att.IncludeEnumType.Length > 0)
                {
                    includeEnumType = att.IncludeEnumType;
                }
                
                if (att.ExcludeNamespace != null && att.ExcludeNamespace.Length > 0)
                {
                    excludeNamespace = excludeNamespace.Union(att.ExcludeNamespace).ToArray();
                }
                
                if (att.ExcludeEnumType != null && att.ExcludeEnumType.Length > 0)
                {
                    excludeEnumType = excludeEnumType.Union(att.ExcludeEnumType).ToArray();
                }
            }
            
            string buttonText = "None"; 
            Type currentEnumType = null;
            
            if (!string.IsNullOrEmpty(typeNameProp.stringValue))
            {
                currentEnumType = Type.GetType(typeNameProp.stringValue);
            }

            if (currentEnumType != null && currentEnumType.IsEnum)
            {
                object enumObject = Enum.ToObject(currentEnumType, enumValueProp.intValue);
                
                if (Enum.IsDefined(currentEnumType, enumObject))
                {
                    buttonText = ObjectNames.NicifyVariableName(enumObject.ToString()) + $" ({currentEnumType.Name})" ;
                }
            }
            
            VisualElement container = new VisualElement();
            container.style.flexDirection = FlexDirection.Row; 
            container.style.alignItems = Align.Center; 

            Label label = new Label();
            if (property.propertyPath.Contains(".Array.data["))
            {
                label.text = "Event";
            }
            else
            {
                label.text = property.displayName; 
            }
            label.style.minWidth = EditorGUIUtility.labelWidth; 
            label.style.flexShrink = 0;
            label.style.paddingLeft = 4f;

            Button btn = new Button();
            btn.text = buttonText; 
            btn.style.flexGrow = 1;
            btn.style.minWidth = 180f;

            btn.clicked += () => OpenSearchWindow(typeNameProp, enumValueProp, btn);

            container.Add(label);
            container.Add(btn);
            
            return container; 
        }

        private void OpenSearchWindow(SerializedProperty typeNameProp, SerializedProperty enumValueProp, Button btn)
        {
            var searchWindow = ScriptableObject.CreateInstance<EnumSearchWindow>();
            searchWindow.includeAssembly = includeAssembly;
            searchWindow.includeNamespace = includeNamespace;
            searchWindow.includeType = includeEnumType;
            searchWindow.excludeNamespace = excludeNamespace;
            searchWindow.excludeType = excludeEnumType;
            
            searchWindow.OnSelect += (type, value, displayName) =>
            {
                typeNameProp.stringValue = type.AssemblyQualifiedName;
                enumValueProp.intValue = value;
                btn.text = displayName;
                
                typeNameProp.serializedObject.ApplyModifiedProperties();
                enumValueProp.serializedObject.Update();
            };
            SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)), searchWindow);
        }
    }

    internal class EnumSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        public Action<Type, int, string> OnSelect;
        public string[] includeAssembly;
        public string[] includeNamespace;
        public Type[] includeType;
        public string[] excludeNamespace;
        public Type[] excludeType;
        
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var list = new List<SearchTreeEntry> { new SearchTreeGroupEntry(new GUIContent("All Enum"), 0) };
            var allEnumType = GetAllEnumsInAllAssemblies(includeAssembly, includeNamespace, includeType, excludeNamespace, excludeType);

            foreach (var enumType in allEnumType)
            {
                try
                {
                    list.Add(new SearchTreeGroupEntry(new GUIContent(ObjectNames.NicifyVariableName(enumType.Name)), 1));

                    var names = Enum.GetNames(enumType);
                    var values = Enum.GetValues(enumType); 
                    
                    for (var i = 0; i < names.Length; i++)
                    {
                        var enumName = names[i];
                        int actualEnumValue = (int)values.GetValue(i); 
                        string displayName = ObjectNames.NicifyVariableName(enumName) + $" ({enumType.Name})"; 
            
                        list.Add(new SearchTreeEntry(new GUIContent(ObjectNames.NicifyVariableName(enumName))) 
                        {
                            level = 2,
                            userData = new Tuple<Type, int, string>(enumType, actualEnumValue, displayName), 
                        });
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }
            
            
            return list;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            var tuple = SearchTreeEntry.userData as Tuple<Type, int, string>;
            OnSelect?.Invoke(tuple.Item1, tuple.Item2, tuple.Item3);
            return true;
        }
        
        public static List<Type> GetAllEnumsInAllAssemblies(string[] includeAssemblyNames = null, 
            string[] includeNamespaces = null, Type[] includeTypes = null,
            string[] excludeNamespaces = null, Type[] excludeTypes = null)
        {
            List<Type> enumTypes = new List<Type>();

            bool filterByAssemblies = includeAssemblyNames != null && includeAssemblyNames.Length > 0;
            bool filterByNamespaces = includeNamespaces != null && includeNamespaces.Length > 0;
            bool filterByTypes = includeTypes != null && includeTypes.Length > 0;
            bool filterByExcludeNamespaces = excludeNamespaces != null && excludeNamespaces.Length > 0;
            bool filterByExcludeTypes = excludeTypes != null && excludeTypes.Length > 0;

            Assembly[] allLoadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly assembly in allLoadedAssemblies)
            {
                string assemblyName = assembly.GetName().Name;

                if (filterByAssemblies && !includeAssemblyNames.Contains(assemblyName))
                {
                    continue;
                }

                Type[] allTypesInAssembly;
                try
                {
                    allTypesInAssembly = assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    allTypesInAssembly = ex.Types.Where(t => t != null).ToArray();
                }
                catch (Exception)
                {
                    continue;
                }

                foreach (Type type in allTypesInAssembly)
                {
                    if (type.IsEnum)
                    {
                        if (filterByExcludeNamespaces)
                        {
                            string typeNamespace = type.Namespace;
                            if (!string.IsNullOrEmpty(typeNamespace) && excludeNamespaces.Any(ns => typeNamespace.StartsWith(ns)))
                            {
                                continue;
                            }
                        }

                        if (filterByExcludeTypes)
                        {
                            if (excludeTypes.Contains(type))
                            {
                                continue;
                            }
                        }
                        
                        if (filterByNamespaces)
                        {
                            string typeNamespace = type.Namespace;
                            if (string.IsNullOrEmpty(typeNamespace) || !includeNamespaces.Any(ns => typeNamespace.StartsWith(ns)))
                            {
                                continue;
                            }
                        }

                        if (filterByTypes)
                        {
                            if (!includeTypes.Contains(type))
                            {
                                continue;
                            }
                        }
                        
                        enumTypes.Add(type);
                    }
                }
            }
            return enumTypes;
        }
    }
}
#endif