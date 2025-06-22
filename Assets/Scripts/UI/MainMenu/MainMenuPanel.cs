using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MainMenuPanel : FadeBlurPanel
{
    [FormerlySerializedAs("_startBtn")]
    [Header("Button")] 
    [SerializeField] private Button _beginBtn;
    [SerializeField] private Button _tutorialBtn;
    [SerializeField] private Button _settingsBtn;
    [SerializeField] private Button _inventoryBtn;
    [SerializeField] private Button _shopBtn;

    protected override void OnAwake()
    {
        base.OnAwake();
        LoadButton();
    }

    private void LoadButton()
    {
        _beginBtn.onClick.AddListener(() =>
        {
            Hide();
            UIManager.Instance.ShowPanel(UIConstant.SetupBeforePlayPanel);
        });
        _tutorialBtn.onClick.AddListener(() =>
        {
            //Hide();
        });
        _settingsBtn.onClick.AddListener(() =>
        {
            Hide();
            UIManager.Instance.ShowPanel(UIConstant.SettingsMenuPanel);
        });
        _inventoryBtn.onClick.AddListener(() =>
        {
            Hide();
            UIManager.Instance.ShowPanel(UIConstant.WeaponUpgradePanel);
        });
        _shopBtn.onClick.AddListener(() =>
        {
            Hide();
            UIManager.Instance.ShowPanel(UIConstant.SetupBeforePlayPanel);
        });
    }
}
