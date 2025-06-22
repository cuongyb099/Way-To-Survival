using System;
using BehaviorDesigner.Runtime;
using Tech.Singleton;
using UnityEngine;

public class BTreeInit : Singleton<BTreeInit>
{
    public Transform Player { get; protected set; }

    protected override void Awake()
    {
        base.Awake();
        Player = FindObjectOfType<PlayerController>().transform;
        
        SharedTransform player = new ()
        {
            Value = Player
        };
        
        GlobalVariables.Instance.SetVariable("Player", player);
    }
}