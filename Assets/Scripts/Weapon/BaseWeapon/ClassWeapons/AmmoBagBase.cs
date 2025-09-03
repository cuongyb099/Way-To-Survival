
using DG.Tweening;
using UnityEngine;

public class AmmoBagBase : MeleeBase
{
    [SerializeField]private int bulletsPerRound = 300;
    private bool giveBullets = false;
    private Attribute holdingBullets;
    public override void Initialize()
    {
        base.Initialize();
        playerController.Stats.TryGetAttribute(AttributeType.HoldingBullets, out holdingBullets);
        GameEvent.OnStartShoppingState += CanGiveBullets;
    }

    private void OnDestroy()
    {
        GameEvent.OnStartShoppingState -= CanGiveBullets;
    }

    private void CanGiveBullets()
    {
        giveBullets = true;
    }
    public override void MeleeLogic()
    {
        if (holdingBullets != null && giveBullets)
        {
            holdingBullets.Value += bulletsPerRound;
            giveBullets = false;
            return;
        }
        Debug.Log("Cannot Give anymore bullets");
    }
}
