using System;
using BehaviorDesigner.Runtime;
using UnityEngine;

public class MilitaryBase : BasicController
{
    protected override void Awake()
    {
        base.Awake();
        GlobalVariables.Instance.SetVariable("Military Base", new SharedTransform()
        {
            Value = transform
        });
    }
}