using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BTreeNode.SharedType;
using UnityEngine;

public class OverlapSphereData : JobData
{
    public SharedFloat Radius;
    [HideInInspector] public Transform Point;
    public SharedVector3 Offset;
    public SharedHashsetTransform HitTargets;
    public SharedLayerMask Layer;
}