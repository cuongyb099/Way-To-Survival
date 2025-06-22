using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingInventory", menuName = "Building/Inventory")]
public class BuildingInventorySO : ScriptableObject
{
    public List<ItemBuiding> listItem = new List<ItemBuiding>();

#if UNITY_EDITOR
    //Utilities Method
    [ContextMenu("Auto Find All Data")]
    public void AutoFindAll()
    {
        listItem = new List<ItemBuiding>();
        
        string[] guids = AssetDatabase.FindAssets("t:ScriptableObject", new[] { "Assets/Data/Building/Structure Data" });

        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            StructureDataSO data = AssetDatabase.LoadAssetAtPath<StructureDataSO>(assetPath);

            if (data != null)
            {
                listItem.Add(new ItemBuiding(data, 0));
            }
        }
    }
#endif
}

[System.Serializable]
public struct ItemBuiding
{
    public StructureDataSO Data;
    public int Quantity;

    public ItemBuiding(StructureDataSO data, int quantity)
    {
        Data = data;
        Quantity = quantity;
    }
}