using KatInventory;
using Newtonsoft.Json;
using UnityEngine;

public class SwordData : ItemGOData
{
    public SwordData(ItemBaseSO staticData, int quantity) : base(staticData, quantity)
    {
    }

    //This Value Need Save So I Put Here
    //If Value Don't Need Save You Can Put It In MonoBehavior
    [JsonProperty(Order = 2)]
    public float Durability = 100f;
}