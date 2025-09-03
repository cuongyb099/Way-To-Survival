using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public class BuildingItemUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] protected LocalizeStringEvent name_Language;
    [SerializeField] protected TextMeshProUGUI quantity_TMP;
    public TurretData Data { get; private set; }
    public UnityEvent<BuildingItemUI> OnClick;
    
    public BuildingItemUI SetData(TurretData data)
    {
        this.Data = data;
        return this;
    }
    
    public BuildingItemUI SetQuantity(int quantity)
    {
        quantity_TMP.text = quantity.ToString();
        return this;
    }
    
    public BuildingItemUI SetNameLanguage(LocalizedString langKey)
    {
        name_Language.StringReference = langKey;
        return this;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke(this);
    }
}