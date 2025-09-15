using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tech.Singleton;
using UnityEngine;
using UnityEngine.Purchasing;

public static class ProductIDs
{
    public const string Diamond120 = "120DiamondPack";
    public const string Diamond250 = "250DiamondPack";
    public const string Diamond620 = "620DiamondPack";
    public const string Diamond1260 = "1260DiamondPack";
    public const string BeginnerPack = "BeginnerPack";
}

public enum ProductKeys
{
    diamond120,
    diamond250,
    diamond620,
    diamond1260,
    beginnerPack,
}
public class IAPManager : SingletonPersistent<IAPManager>
{
    private StoreController storeController;
    public Action OnPurchasedSuccess;
    public Action OnPurchasedFailed;
    
    //Maybe there is another way to do this
    private bool onPurchasing = false;
    private async void Start()
    {
        storeController = UnityIAPServices.StoreController();

        // Listen to store events
        storeController.OnPurchasePending += OnPurchasePending;
        storeController.OnPurchaseConfirmed += OnPurchaseConfirmed;
        storeController.OnPurchaseFailed += OnPurchaseFailed;
        storeController.OnProductsFetchFailed += OnProductFetchFailed;
        await storeController.Connect();

        // Fetch your products
        FetchProducts();
    }


    private void FetchProducts()
    {
        var products = new List<ProductDefinition>
        {
            new(ProductIDs.Diamond120, ProductType.Consumable),
            new(ProductIDs.Diamond250, ProductType.Consumable),
            new(ProductIDs.Diamond620, ProductType.Consumable),
            new(ProductIDs.Diamond1260, ProductType.Consumable),
            new(ProductIDs.BeginnerPack, ProductType.NonConsumable),
        };

        storeController.FetchProducts(products);
    }
    
    private void OnPurchasePending(PendingOrder order)
    {
        var product = order.CartOrdered.Items().First()?.Product;
        Debug.Log($"Pending purchase: {product.definition.id}");

        // Grant reward now if you want immediate effect
        // But for consumables, best practice is to wait until confirmed

        // Confirm purchase so the transaction is completed
        storeController.ConfirmPurchase(order);
    }

    private void OnPurchaseConfirmed(Order order)
    {
        var product = order.CartOrdered.Items().First()?.Product;
        MessagePopup.Instance.ShowMessage($"Confirmed purchase: {product.definition.id}");
        onPurchasing = false;

        OnPurchasedSuccess?.Invoke();
        GrantReward(product.definition.id);
    }

    private void OnPurchaseFailed(FailedOrder order)
    {
        var product = order.CartOrdered.Items().First()?.Product;
        MessagePopup.Instance.ShowMessage($"Purchase failed for {product?.definition.id}, reason: {order.FailureReason}");
        onPurchasing = false;
    }

    private void OnProductFetchFailed(ProductFetchFailed product)
    {
        MessagePopup.Instance.ShowMessage($"Failed to fetch reason: {product?.FailureReason}");
    }
    private void GrantReward(string productId)
    {
        switch (productId)
        {
            case ProductIDs.Diamond120:
                PlayerDataPersistent.Instance.PlayerData.Diamonds += 120;
                break;
            case ProductIDs.Diamond250:
                PlayerDataPersistent.Instance.PlayerData.Diamonds += 250;
                break;
            case ProductIDs.Diamond620:
                PlayerDataPersistent.Instance.PlayerData.Diamonds += 620;
                break;
            case ProductIDs.Diamond1260:
                PlayerDataPersistent.Instance.PlayerData.Diamonds += 1260;
                break;
            case ProductIDs.BeginnerPack:
                PlayerDataPersistent.Instance.PlayerData.Diamonds += 200;
                PlayerDataPersistent.Instance.PlayerData.Coins += 1000;
                
                break;
            default:
                Debug.LogWarning("Unknown product: " + productId);
                break;
        }
    }
    public void BuyProduct(ProductKeys key)
    {
        if (onPurchasing) return;
        onPurchasing = true;
        
        string productId = GetID(key);

        if (!string.IsNullOrEmpty(productId))
        {
            storeController.PurchaseProduct(productId);
        }
        else
        {
            Debug.LogWarning("Invalid product key: " + key);
        }
    }

    public Product GetProduct(ProductKeys key)
    {
        string productId = GetID(key);
        return storeController.GetProductById(productId);
    }

    private string GetID(ProductKeys key)
    {
        return key switch
        {
            ProductKeys.diamond120 => ProductIDs.Diamond120,
            ProductKeys.diamond250 => ProductIDs.Diamond250,
            ProductKeys.diamond620 => ProductIDs.Diamond620,
            ProductKeys.diamond1260 => ProductIDs.Diamond1260,
            ProductKeys.beginnerPack => ProductIDs.BeginnerPack,
            _ => null
        };
    }
    
}
