using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHandler : MonoBehaviour
{
	public bool IsTriggered {
		get { return colliders.Count > 0; }
	}
	private List<GameObject> colliders = new List<GameObject>();

	private void OnEnable()
	{
		colliders.Clear();
	}

	private void OnTriggerStay(Collider other)
	{
		if(colliders.Contains(other.gameObject)) return;
		colliders.Add(other.gameObject);
	}

	private void LateUpdate()
	{
		for (int i = colliders.Count-1;i >= 0; --i )
		{
			if(colliders[i]) continue;
			colliders.RemoveAt(i);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		colliders.Remove(other.gameObject);
	}

	private void OnDisable()
	{
		colliders.Clear();
	}
}
