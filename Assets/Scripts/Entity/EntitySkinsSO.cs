using System;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Skin", order = 0)]
public class EntitySkinsSO : ScriptableObject
{
    [SerializeField] private Skin[] _entitySkins;
    [SerializeField, Range(1, 20)] private int _bodyPartCount = 1;
    public Skin[] EntitySkins => _entitySkins;
    
    [Serializable]
    public class Skin
    {
        public GameObject[] SkinParts;

        public Skin(int amount)
        {
            SkinParts = new GameObject[amount];
        }
    }
    
#if UNITY_EDITOR
    public string Path;
    public string[] PartName;
    
    [ContextMenu("Auto Find Zombie Skin")]
    public void AutoFindEnemySkin()
    {
        _entitySkins = new Skin[_bodyPartCount];
        for (int i = 0; i < _bodyPartCount; i++)
        {
            var guids = AssetDatabase.FindAssets("t:GameObject", new[] { Path +  PartName[i]});
            _entitySkins[i] = new Skin(guids.Length);
            for (int j = 0; j < guids.Length; j++)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guids[j]);
                GameObject data = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                
                if (data != null)
                {
                    _entitySkins[i].SkinParts[j] = data;
                }
            }
        }
    }
#endif
}