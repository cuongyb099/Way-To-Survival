/*
using System.Collections.Generic;
using UnityEngine;

public class BuildingListCtrl : MonoBehaviour
{
    [SerializeField] private BuildingInventorySO buildingInventory;
    [SerializeField] private Transform slotPrefab;
    [SerializeField] private Transform slotHolder;
    
    private Transform scrollView;
    private Dictionary<BuidingSlotUI, int> listSlotUI;
    
    private void Awake()
    {
        scrollView = transform.GetChild(0);
    }

    private void Start()
    {
        InitAllItemView();
    }

    private void InitAllItemView()
    {
        listSlotUI = new();
        
        buildingInventory.listItem.ForEach(x =>
        {
            var function = Instantiate(slotPrefab).GetComponent<BuidingSlotUI>();
            function.Init(x.Data.IconUI, x.Quantity, x.Data.name);
            listSlotUI.Add(function, x.Data.Id);
            function.OnSlotBtnClick += () =>
            {
                ToggleBuildingList();
                //Subject.Notify(EventID.OnBuidingSlotBtnClick, listSlotUI[function]);
            };
            function.transform.SetParent(slotHolder);
        });
        
        
    }

    public void ToggleBuildingList()
    {
        if(!PlayerInput.Instance.IsInBuildingMode) return;
        scrollView.gameObject.SetActive(!scrollView.gameObject.activeSelf);
    }
    
    
}
*/
