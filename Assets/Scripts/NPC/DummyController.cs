using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyController : BasicController
{
    public int CashDrop = 2;
    public override void Death(GameObject dealer)
    {
        base.Death(dealer);
        if (dealer.TryGetComponent(out PlayerController player))
        {
            player.Resin += CashDrop;
        }
        Destroy(gameObject);
    }
}
