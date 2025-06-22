using System;
using System.Collections.Generic;
using UnityEngine;

public class EntitySkinCtrl : MonoBehaviour
{
    [field: SerializeField] public Skin[] BodySkinParts { get; private set; }
    
    private void Awake()
    {
        Transform model = transform.GetChild(0);
        for (int i = 1; i < model.childCount; i++)
        {
            model.GetChild(i).gameObject.SetActive(false);       
        }
    }
#if UNITY_EDITOR
    [Range(1, 30)]public int BodyPartCount;
    public string[] BodyPartsName;
    public GameObject ObjectToFindSkin;

    [ContextMenu("Auto Find Skins")]
    public void AutoFindSkins()
    {
        if(!ObjectToFindSkin || BodyPartsName.Length <= 0) return;
        
        var childCount = ObjectToFindSkin.transform.childCount;
        
        BodySkinParts = new Skin[BodyPartsName.Length];
        var tmpSkins = new List<List<SkinnedMeshRenderer>>();
        
        for (int i = 0; i < BodyPartsName.Length; i++)
        {
            tmpSkins.Add(new List<SkinnedMeshRenderer>());
        }
        
        for (int i = 0; i < childCount; i++)
        {
            Transform child = ObjectToFindSkin.transform.GetChild(i);
            for (int j = 0; j < BodyPartsName.Length; j++)
            {
                if (child.name.Contains(BodyPartsName[j]))
                {
                    tmpSkins[j].Add(child.gameObject.GetComponent<SkinnedMeshRenderer>());
                    break;
                }
            }
        }

        for (int i = 0; i < BodyPartsName.Length; i++)
        {
            BodySkinParts[i] = new Skin(tmpSkins[i].Count)
            {
                SkinParts = tmpSkins[i].ToArray()
            };
        }
    }
#endif
}

[Serializable]
public class Skin
{
    public SkinnedMeshRenderer[] SkinParts;

    public Skin(int amount)
    {
        SkinParts = new SkinnedMeshRenderer[amount];
    }
}
