using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class BaseStructure : MonoBehaviour
{
    private new MeshRenderer renderer;
    private new Collider collider;
    public Material DefaultMat { get; private set; }
    protected virtual void Awake()
    {
        renderer = GetComponentInChildren<MeshRenderer>();
        collider = GetComponentInChildren<Collider>();

        DefaultMat = renderer.materials.FirstOrDefault();
    }

    //Unity only allow change array of materials meshRenderer 
    public virtual void SetIsIndicator(Material matIndicator)
    {
        renderer.materials = new[] { matIndicator };
        renderer.shadowCastingMode = ShadowCastingMode.Off;
        collider.isTrigger = true;
    }

    public virtual void SetIsStructure()
    {
        renderer.materials = new[] { DefaultMat };
        renderer.shadowCastingMode = ShadowCastingMode.On;
        collider.isTrigger = false;
        Destroy(GetComponent<Rigidbody>());
    }
    
    public virtual void SetIsStructure(Material Mat)
    {
        renderer.materials = new[] { Mat };
        renderer.shadowCastingMode = ShadowCastingMode.On;
        collider.isTrigger = false;
        Destroy(GetComponent<Rigidbody>());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Structure")) return;
        Debug.Log(other.gameObject.name + "Enter");
        //BuildingSystem.Instance.ObstaclesOccupy++;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Structure")) return;
        Debug.Log(other.gameObject.name + "Exit");
        //BuildingSystem.Instance.ObstaclesOccupy--;
    }
}
