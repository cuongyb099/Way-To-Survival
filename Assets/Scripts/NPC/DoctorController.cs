using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoctorController : InteractableController
{
    [SerializeField] private int buyPrice;
    
    public override void Interact(PlayerController source)
    {
        if (source.Resin < buyPrice)
        {
            DamagePopUpGenerator.Instance.CreateDamagePopUp(source.transform.position, $"Không đủ tiền!!!");
            return;
        }
        source.Resin -= buyPrice;
        
        UIManager.Instance.HidePanel(UIConstant.MainGameplayPanel);
        UIManager.Instance.ShowPanel(UIConstant.BuffPanel);
        base.Interact(source);
    }
}
