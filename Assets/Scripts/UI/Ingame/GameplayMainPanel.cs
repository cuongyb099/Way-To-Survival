using System;
using UnityEngine;
using UnityEngine.UI;

public class GameplayMainPanel : PanelToggleByCanvas
{
    [Header("Button")]
    [SerializeField] private Button _skipShoppingBtn;
    [SerializeField] private Button _openInventoryBtn;
    [SerializeField] private Button _switchBuildingModeBtn;
    [SerializeField] private Button _rotateBuildingBtn;
    [SerializeField] private Button _buildBtn;
    [SerializeField] private Button _pauseBtn;
    [SerializeField] private Button _interactBtn;
    
    [field: Header("Slider")]
    [field: SerializeField] public Slider CountDownSlider { get; private set; }

    protected override void OnAwake()
    {
        LoadComponents();
        
    }

    private void Start()
    {
        Show();
    }

    private void Reset()
    {
        ResetButton();
        ResetSlider();
    }


    private void LoadComponents()
    {
        LoadButton();
    }

    private void LoadButton()
    {
        _skipShoppingBtn.onClick.AddListener(() => GameManager.Instance.SkipShopping = true);
        _openInventoryBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.ShowPanel(UIConstant.WeaponWheelPanel);
            Hide();
        });
        _pauseBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.ShowPanel(UIConstant.PausePanel);
            Hide();
        });
        
        PlayerEvent.OnInteractEnter += ShowInteractBtn;
        PlayerEvent.OnInteractExit += HideInteractBtn;
    }

    private void OnDestroy()
    {
        PlayerEvent.OnInteractEnter -= ShowInteractBtn;
        PlayerEvent.OnInteractExit -= HideInteractBtn;
    }

    public override void Hide()
    {
        base.Hide();
        PlayerInput.Instance.InputActions.BasicAction.Disable();
    }

    public override void Show()
    {
        base.Show();
        PlayerInput.Instance.InputActions.BasicAction.Enable();
    }

    private void ShowInteractBtn()
    {
        _interactBtn.gameObject.SetActive(true);
    }

    private void HideInteractBtn()
    {
        _interactBtn.gameObject.SetActive(false);
    }
    

    private void ResetSlider()
    {
        var top = transform.Find(UIConstant.Top);
        var topMid = top.Find(UIConstant.Mid);

        if (!CountDownSlider)
        {
            CountDownSlider = topMid.GetComponentInChildren<Slider>();
        }
    }
    private void ResetButton()
    {
        var top = transform.Find(UIConstant.Top);
        var bottom = transform.Find(UIConstant.Bottom);
        var topMid = top.Find(UIConstant.Mid);
        var topRight = top.Find(UIConstant.Right);
        var topLeft = top.Find(UIConstant.Left);
        
        if (!_skipShoppingBtn)
        {
            _skipShoppingBtn = topMid.Find("Skip Shopping BTN").GetComponentInChildren<Button>(true);
        }

        if (!_openInventoryBtn)
        {
            _openInventoryBtn = topRight.Find("Open Inventory BTN").GetComponentInChildren<Button>(true);
        }

        if (!_switchBuildingModeBtn)
        {
            _switchBuildingModeBtn = topRight.Find("Building Mode BTN").GetComponentInChildren<Button>(true);
        }
        
        if (!_rotateBuildingBtn)
        {
            _rotateBuildingBtn = topRight.Find("Rotate BTN").GetComponentInChildren<Button>(true);
        }

        if (!_buildBtn)
        {
            _buildBtn = topRight.Find("Build BTN").GetComponentInChildren<Button>(true);
        }

        if (!_pauseBtn)
        {
            _pauseBtn = topLeft.Find("Pause BTN").GetComponentInChildren<Button>(true);
        }

        if (!_interactBtn)
        {
            _interactBtn = topRight.Find("Interact BTN").GetComponentInChildren<Button>(true);
        }
    }
}