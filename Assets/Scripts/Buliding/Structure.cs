using KatInventory;
using UnityEngine;

[RequireComponent(typeof(IndicatorBuilding))]
public abstract class Structure : BasicController, IItemInstance
{
    protected IndicatorBuilding indicator;
    protected Collider collision;
    
    [field: SerializeField] public PlaceHitBox PlacmentHitBox { get; private set; }

    protected virtual void Reset()
    {
        PlacmentHitBox = GetComponentInChildren<PlaceHitBox>();
    }

    protected override void Awake()
    {
        base.Awake();
        collision = GetComponent<Collider>();
        indicator = GetComponent<IndicatorBuilding>();
    }

    public virtual void SetIndicator(Color color)
    {
        collision.enabled = false;
        indicator.SetIndicator(color);
    }

    public virtual void ReturnDefault()
    {
        collision.enabled = true;
        indicator.ReturnDefault();   
    }

    public abstract void SetData(ItemData itemData);
}