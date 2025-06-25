using KatInventory;
using UnityEngine;

[RequireComponent(typeof(IndicatorBuilding))]
public class Structure : ItemBase
{
    protected IndicatorBuilding indicator;
    protected Collider collision;
    
    [field: SerializeField] public PlaceHitBox PlacmentHitBox { get; private set; }

    protected virtual void Reset()
    {
        PlacmentHitBox = GetComponentInChildren<PlaceHitBox>();
    }

    protected virtual void Awake()
    {
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
}