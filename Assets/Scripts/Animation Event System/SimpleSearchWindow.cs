#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace KatLib.Editor
{
    public class SimpleSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        public List<string> Source;
        public string Title;
        public Action<int> OnSelect;
        private string space = "      ";
        
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var list = new List<SearchTreeEntry> { new SearchTreeGroupEntry(new GUIContent(Title), 0) };

            for (var i = 0; i < Source.Count; i++)
            {
                SearchTreeEntry entry = new SearchTreeEntry(new GUIContent(space + Source[i]))
                {
                    level = 1,
                    userData = i,
                };
                list.Add(entry);
            }

            return list;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            OnSelect?.Invoke((int)SearchTreeEntry.userData);
            return true;
        }
    }
}
#endif