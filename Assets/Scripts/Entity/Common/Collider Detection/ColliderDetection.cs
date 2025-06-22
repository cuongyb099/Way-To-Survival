using System;
using UnityEngine;

public class ColliderDetection : MonoBehaviour
{
    public string DetectionName;
    public Action<Collider> CallbackTriggerEnter;
    private Collider _collider;
    
    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _collider.isTrigger = true;
        _collider.enabled = false;
    }

    public void SetActiveDetect(bool active)
    {
        _collider.enabled = active;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        CallbackTriggerEnter?.Invoke(other);
    }
}