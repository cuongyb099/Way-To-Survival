using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MainMenuPanel : FadeBlurPanel
{
    [FormerlySerializedAs("_startBtn")]
    [Header("PlayerInfo")] 
    [SerializeField] private TextMeshProUGUI _playerNameText;
    [SerializeField] private Image _playerImage;
    [Header("Button")] 
    [SerializeField] private Button _logOutBtn;
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

    public override void Show()
    {
        base.Show();
        _playerNameText.text = PlayerDataPersistent.Instance.PlayerData.Username;
    }

    private void LoadButton()
    {
        _logOutBtn.onClick.AddListener(() =>
        {
            PlayerDataPersistent.Instance.Save();
            AuthHandle.Instance.LogOutGoogle();
            Hide();
            MessagePopup.Instance.ShowMessage("Signed out");
            UIManager.Instance.ShowPanel(UIConstant.LoginMenuPanel);
        });
        _beginBtn.onClick.AddListener(() =>
        {
            Hide();
            UIManager.Instance.ShowPanel(UIConstant.SetupBeforePlayPanel);
        });
        _tutorialBtn.onClick.AddListener(() =>
        {
            Hide();
            UIManager.Instance.ShowPanel(UIConstant.ShopMenuPanel);
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
            UIManager.Instance.ShowPanel(UIConstant.ShopMenuPanel);
        });
    }
}
