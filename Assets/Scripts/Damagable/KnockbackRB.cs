using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class KnockbackRB : MonoBehaviour,  IKnockbackable
{
    private Rigidbody _rb;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void ApplyKnockback(Vector3 direction, float force)
    {
        _rb.velocity = Vector3.zero;
        _rb.AddForce(direction * force, ForceMode.Impulse);
    }
}