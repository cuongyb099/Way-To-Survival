using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable 
{
    public Transform ObjTransform { get; }
    public Action OnKill { get; set; }
    public void Interact(PlayerController source);
    public void OnSelect();
    public void OnDeselect();
}
