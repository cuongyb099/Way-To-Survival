using BehaviorDesigner.Runtime;
using UnityEngine;

public class RangeTurret : Structure
{
    protected BehaviorTree bTree;
    protected override void Awake()
    {
        base.Awake();
        bTree = GetComponent<BehaviorTree>();
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
}
