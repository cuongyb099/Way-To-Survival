using System;
using KatLib.Serialized_Type;
using UnityEngine;

public class ColliderDetection : MonoBehaviour
{
    public StringSO DetectionName;
    public Action<Collider> CallbackTriggerEnter;
    protected Collider m_collider;
    
    protected virtual void Awake()
    {
        m_collider = GetComponent<Collider>();
        m_collider.isTrigger = true;
        m_collider.enabled = false;
    }

    public virtual void SetActiveDetect(bool active)
    {
        m_collider.enabled = active;
    }
    
    protected virtual void OnTriggerEnter(Collider other)
    {
        CallbackTriggerEnter?.Invoke(other);
    }

    public virtual void SetIncludeLayer(LayerMask layer)
    {
        m_collider.includeLayers = layer;
    }

    public virtual void SetExcludeLayer(LayerMask layer)
    {
        m_collider.excludeLayers = layer;
    }
}