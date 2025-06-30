using KatInventory;
using UnityEngine;

public class TestInventory : MonoBehaviour
{
    //Note: All This Is Demo Please Don't Put String In Method Like This. Instead Of Use static readonly or const string
    [ContextMenu("Test Add Gold")]
    public void AddGold()
    {
        PlayerDataPersistent.Instance.PlayerData.Inventory.AddItem("Gold", 100);
    }
    
    [ContextMenu("Test Add Sword Prefab")]
    public void AddSwordPrefab()
    {
        if(PlayerDataPersistent.Instance.PlayerData.Inventory.AddItem("Sword") is not SwordData data) return;
        
        Debug.Log("I Do Something With " + data.GoReference.name);
    }

    [ContextMenu("Test Remove Gold")]
    public void RemoveGold()
    {
        PlayerDataPersistent.Instance.PlayerData.Inventory.RemoveItem("Gold", 100);
    }
    
    [ContextMenu("Test Remove Sword")]
    public void RemoveSword()
    {
        if (PlayerDataPersistent.Instance.PlayerData.Inventory.AddItem("Sword") is SwordData data) 
            data.GoReference.gameObject.SetActive(true);
    }

    [ContextMenu("Test Save")]
    public void Save()
    {
        PlayerDataPersistent.Instance.Save();
    }

    [ContextMenu("Load")]
    public void Load()
    {
        PlayerDataPersistent.Instance.Load();
    }

    [ContextMenu("Encrypt")]
    public void test()
    {
        
    }
}
