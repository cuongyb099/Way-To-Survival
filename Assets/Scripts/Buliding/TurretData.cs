using KatInventory;
using Tech.Pooling;

public class TurretData : ItemData
{
    public TurretDataSO TurretDataSO => (TurretDataSO)StaticData;
    
    public TurretData(ItemBaseSO staticData, int quantity) : base(staticData, quantity)
    {
    }

    
    public Structure GetStructureRef()
    {
        return ObjectPool.Instance.SpawnObject(TurretDataSO.Prefab.gameObject, default, default)
            .GetComponent<Structure>();
    } 
}