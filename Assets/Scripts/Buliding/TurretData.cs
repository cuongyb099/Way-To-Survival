using KatInventory;
using Newtonsoft.Json;
using Tech.Pooling;

public class TurretData : ItemData
{
    public TurretDataSO TurretDataSO => (TurretDataSO)StaticData;
    
    [JsonConstructor]
    public TurretData(string ID, int quantity)  : base(ID, quantity)
    {
            
    }
    
    public TurretData(ItemBaseSO staticData, int quantity) : base(staticData, quantity)
    {
    }

    
    public Structure GetStructureRef()
    {
        return ObjectPool.Instance.SpawnObject(TurretDataSO.Prefab.gameObject, default, default)
            .GetComponent<Structure>();
    } 
}