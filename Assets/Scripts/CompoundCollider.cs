using System;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class CompoundCollider : MonoBehaviour
{
    public List<string> TagsCheck = new ();
    private Dictionary<Collider, int> collisions = new Dictionary<Collider, int>();
    public Action<Collider> OnEnter;
    public Action<Collider> OnExit;
    public Action<Collider> OnStay;

    private void OnTriggerEnter(Collider other)
    {
        foreach(string tag in TagsCheck)
        {
            if(!other.CompareTag(tag)) continue;

            if (collisions.ContainsKey(other))
            {
                collisions[other]++;
            }
            else
            {
                collisions[other] = 1;
                OnEnter?.Invoke(other);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        foreach(string tag in TagsCheck)
        {
            if(!other.CompareTag(tag)) continue;

            OnStay?.Invoke(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        foreach(string tag in TagsCheck)
        {
            if(!other.CompareTag(tag)) continue;

            if (collisions.ContainsKey(other))
            {
                collisions[other]--;
                if (collisions[other] <= 0)
                {
                    OnExit?.Invoke(other);
                    collisions.Remove(other);
                }
            }
        }
    }
}