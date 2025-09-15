using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BuyItem : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI productName;
    [SerializeField] private TMPro.TextMeshProUGUI productDescription;
    [SerializeField] private TMPro.TextMeshProUGUI price;
    [SerializeField] private Image productImage;
    [SerializeField] private Button buyButton;

    public ProductKeys Product;
    
    private void Awake()
    {
        buyButton.onClick.AddListener(OnBuyClicked);
    }
    private void OnEnable()
    {
        IAPManager.Instance.OnPurchasedSuccess += OnInstancePurchased;
    }

    private void OnDisable()
    {
        IAPManager.Instance.OnPurchasedSuccess -= OnInstancePurchased;
    }

    private void OnInstancePurchased()
    {
        buyButton.interactable = true;
    }
    
    private void Start()
    {
        if (IAPManager.Instance.GetProduct(Product).hasReceipt)
        {
            
        }
        UpdateUI();
    }

    private void OnBuyClicked()
    {
        IAPManager.Instance.BuyProduct(Product);
    }

    public void SetProduct(ProductKeys key, Sprite icon)
    {
        Product = key;
        productImage.sprite = icon;
        UpdateUI();
    }

    private void UpdateUI()
    {
        
        var meta = IAPManager.Instance.GetProduct(Product).metadata;
        productName.text = meta.localizedTitle;
        //productDescription.text = meta.localizedDescription;
        price.text = meta.localizedPriceString;
    }

}