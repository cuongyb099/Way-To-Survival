using BehaviorDesigner.Runtime;
using UnityEngine;

[RequireComponent(typeof(BehaviorTree))]
public abstract class TurretBase : StructureBase
{
    protected BehaviorTree behavior;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        behavior = GetComponent<BehaviorTree>();
    }
}