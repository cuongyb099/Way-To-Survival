using System;
using TMPro;
using UnityEngine;

public class InteractableController : BasicController, IInteractable
{
    public GameObject SelectedUI;
    public TextMeshProUGUI SpeakingText;
    public Transform ObjTransform => transform;
    public Action OnKill { get; set; }
    
    private void OnDestroy()
    {
        OnKill?.Invoke();
    }

    public virtual void Interact(PlayerController source)
    {
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
