using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShopController : InteractableController
{
    public override void Interact(PlayerController source)
    {
        base.Interact(source);
        UIManager.Instance.HidePanel(UIConstant.MainGameplayPanel);
        UIManager.Instance.ShowPanel(UIConstant.ShopPanel);
    }
}
