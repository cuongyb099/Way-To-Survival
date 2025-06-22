/*
using System;

#if UNITY_EDITOR
namespace KatInventory.Editor
{
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;
    
    [CustomEditor(typeof(Inventory))]
    public class InventoryEditor : Editor
    {
        List<IItemData> _inventory => ((Inventory)target).DataRuntime;
        private SerializedProperty _capacity;
        private string _searchID = string.Empty;
        private int _pageCount = 10;
        private int _curPage;
        private void OnEnable()
        {
            _capacity = serializedObject.FindAutoProperty("Capacity");
        }

        public override void OnInspectorGUI()
        {
            try
            {

                EditorGUILayout.PropertyField(_capacity);
                _capacity.serializedObject.ApplyModifiedProperties();
                
                
                if (_inventory == null || _inventory.Count == 0)
                {
                    EditorGUILayout.LabelField("Inventory Is Empty");
                    return;
                }
                
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Search");
                _searchID = EditorGUILayout.TextField(_searchID, GUILayout.MaxWidth(400));
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.LabelField("Inventory");

                var value = _inventory;
                if (_searchID != string.Empty)
                {
                    value = value.FindAll(x => x.StaticData.ID.ToLower().StartsWith(_searchID.ToLower()));
                }
                
                var maxPageCount = Mathf.CeilToInt(value.Count / _pageCount);
                if (_curPage > maxPageCount)
                {
                    _curPage = maxPageCount;
                }
                GUI.enabled = false;
                for (int i = 0 + _curPage * _pageCount; i < _pageCount + _curPage * _pageCount; i++)
                {
                    if(i >= value.Count) break;
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.ObjectField(value[i].StaticData, typeof(ItemBaseSO), false);
                    GUILayout.Space(10); 
                    EditorGUILayout.LabelField("Quantity : " + value[i].Quantity);
                    EditorGUILayout.EndHorizontal();
                }
                
                GUI.enabled = true;
                
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Previous"))
                {
                    if (_curPage > 0)
                    {
                        _curPage--;
                    }
                };
                if (GUILayout.Button("Next"))
                {
                    if (_curPage < maxPageCount)
                    {
                        _curPage++;
                    }
                };
                EditorGUILayout.EndHorizontal();
            }
            catch (Exception e)
            {
            }
        }
    }
}
#endif
*/
