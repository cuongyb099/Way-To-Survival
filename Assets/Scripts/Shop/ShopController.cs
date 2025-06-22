using System;
using BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : BasicController, IInteractable
{
    public GameObject ShopCanvas;
    public GameObject SelectedUI;
    public Transform ObjTransform => transform;
    public Action OnKill { get; set; }
    
    private void OnDestroy()
    {
        OnKill?.Invoke();
    }

    public void Interact(PlayerController source)
    {
        ShopCanvas.SetActive(true);
    }

    public void OnSelect()
    {
        SelectedUI.gameObject.SetActive(true);
    }

    public void OnDeselect()
    {
        SelectedUI.gameObject.SetActive(false);
    }
    
}
