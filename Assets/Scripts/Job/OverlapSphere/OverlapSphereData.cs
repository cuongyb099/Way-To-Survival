using UnityEngine;

public class OverlapSphereData : JobData
{
    [HideInInspector] public float Radius;
    [HideInInspector] public Transform Point;
    [HideInInspector] public Vector3 Offset;
    [HideInInspector] public Transform Target;
}