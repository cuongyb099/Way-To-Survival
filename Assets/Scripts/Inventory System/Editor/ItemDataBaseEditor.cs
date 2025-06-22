#if UNITY_EDITOR
namespace KatInventory.Editor
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(ItemDataBase))]
    [CanEditMultipleObjects]
    public class ItemDataBaseEditor : Editor
    {
        private Dictionary<string, ItemBaseSO> _dictionary => ((ItemDataBase)target).ItemDictionary;
        private string _searchID = string.Empty;
        private int _pageCount = 10;
        private int _curPage;
        public override void OnInspectorGUI()
        {
            if (_dictionary == null)
            {
                return;
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Search");
            _searchID = EditorGUILayout.TextField(_searchID, GUILayout.MaxWidth(400));
            EditorGUILayout.EndHorizontal();
            
            List<ItemBaseSO> items = new();
            GUI.enabled = false;

            var value = _dictionary.Values.ToList();

            if (_searchID != string.Empty)
            {
                value = value.FindAll(x => x.ID.ToLower().StartsWith(_searchID.ToLower()));
            }
            
            var maxPageCount = Mathf.CeilToInt(value.Count / _pageCount);
            if (_curPage > maxPageCount)
            {
                _curPage = maxPageCount;
            }
            for (int i = 0 + _curPage * _pageCount; i < _pageCount + _curPage * _pageCount; i++)
            {
                if(i >= value.Count) break;
                EditorGUILayout.ObjectField("Item " + i, value[i], typeof(ItemBaseSO), false);
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
        
        
    }
}
#endif
