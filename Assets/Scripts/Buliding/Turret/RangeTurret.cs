using BehaviorDesigner.Runtime;
using KatInventory;
using UnityEngine;

public class RangeTurret : Structure
{
    protected BehaviorTree bTree;
    protected TurretData turretData;
    protected override void Awake()
    {
        base.Awake();
        bTree = GetComponent<BehaviorTree>();
        OnDeath += () =>
        {
            this.gameObject.SetActive(false);
        };
    }

    public override void SetIndicator(Color color)
    {
        base.SetIndicator(color);
        bTree.enabled = false;
    }

    public override void ReturnDefault()
    {
        base.ReturnDefault();
        bTree.enabled = true;
    }

    public override void SetData(ItemData itemData)
    {
        turretData = itemData as TurretData;
    }
}
