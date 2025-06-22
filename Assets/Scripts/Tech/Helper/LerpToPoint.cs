
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpToPoint : MonoBehaviour
{
    [field: SerializeField] public Transform TargetPoint { get; private set; }
	[field: SerializeField] public float SwitchBlendValue { get; private set; } = 100f;
       
    void Update()
    {
		transform.position = Vector3.Lerp(transform.position, TargetPoint.transform.position, SwitchBlendValue*Time.deltaTime);
	}
}
