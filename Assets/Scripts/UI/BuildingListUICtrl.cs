/*
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingListUICtrl : MonoBehaviour
{
    private Transform slotPrefab;
    private Transform slotHolder;

    [SerializeField] private Button buildBtn;
    [SerializeField] private Button rotateBtn;
    
    private Transform scrollView;
    private Dictionary<BuidingSlotUI, int> listSlotUI;
    
    private void Awake()
    {
        scrollView = transform.Find("Structure Panel");
        slotHolder = transform.GetChild(0).GetChild(0).GetChild(0);

        AddressablesManager.Instance.CreateAsset<GameObject>("UI/Structure Slot", (prefab) =>
        {
            slotPrefab = prefab.transform;
		});

        InputEvent.OnBuildingMode += SetInActiveBtn;
    }
	private void OnDestroy()
	{
		InputEvent.OnBuildingMode -= SetInActiveBtn;
	}

	private void Start()
	{
        InitAllItemView();
	}


	private void InitAllItemView()
    {
        listSlotUI = new();
        
        BuildingSystem.Instance.Inventory.listItem.ForEach(x =>
        {
            var function = Instantiate(slotPrefab).GetComponent<BuidingSlotUI>();
            function.Init(x.Data.IconUI, x.Quantity, x.Data.name);
            listSlotUI.Add(function, x.Data.Id);
            
            function.OnSlotBtnClick += () =>
            {
                SetActiveBtn();
                ToggleBuildingList();
                BuildingSystem.Instance.ActiveIndicator(x.Data.Id);
            };
            function.transform.SetParent(slotHolder);
        });
    }

    public void SetActiveBtn()
    {
        buildBtn.gameObject.SetActive(true);
        rotateBtn.gameObject.SetActive(true);
    }
    
    public void SetInActiveBtn()
    {
        if(PlayerInput.Instance.IsInBuildingMode) return;
        
        buildBtn.gameObject.SetActive(false);
        rotateBtn.gameObject.SetActive(false);
    }
    
    public void ToggleBuildingList()
    {
        if(!PlayerInput.Instance.IsInBuildingMode) return;
        scrollView.gameObject.SetActive(!scrollView.gameObject.activeSelf);
    }
}
*/
