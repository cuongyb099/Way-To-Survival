using System;
using System.Linq;
using KatInventory;
using Tech.Logger;
using UnityEngine;
using UnityEngine.Rendering;

public abstract class StructureBase : ItemBase
{
    private MeshRenderer[] _renderer;
    private Collider _collider;
    [SerializeField]
    private Transform _model;
    
    public Material DefaultMat { get; private set; }

    protected virtual void Reset()
    {
        LoadComponents();
    }

    protected virtual void Awake()
    {
        LoadComponents();
    }

    protected virtual void LoadComponents()
    {
        if (!_model)
        {
            _model = transform.Find(GOConstant.Model);
        }
        
        _renderer = _model.GetComponentsInChildren<MeshRenderer>();
        
        _collider = GetComponent<Collider>();
    }
    
    public virtual void SetIsIndicator(Material matIndicator)
    {
        _collider.enabled = false;
    }

    public virtual void SetIsStructure()
    {
        _collider.enabled = true;
    }
    
    public virtual void SetIsStructure(Material Mat)
    {
        _collider.enabled = true;
    }
}
