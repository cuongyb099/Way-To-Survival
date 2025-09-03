using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : FadeBlurPanel
{
    [Header("Button")] 
    [SerializeField] private Button _startBtn;
    [SerializeField] private Button _quitBtn;
    
    [SerializeField] private Button _guestBtn;
    [SerializeField] private Button _googleBtn;
    [SerializeField] private Button _facebookBtn;
    protected override void OnAwake()
    {
        base.OnAwake();
        LoadButton();

        PlayerDataPersistent.Instance.OnLoadPlayerData += MoveToMainMenu;
    }

    public override void Hide()
    {
        base.Hide();
        MainMenuManager.Instance.GameTitle.SetActive(false);
    }

    public override void Show()
    {
        if (AuthHandle.Instance.User != null)
        {
            MoveToMainMenu();
            return;
        }
        base.Show();
        MainMenuManager.Instance.GameTitle.SetActive(true);
    }
    
    private void LoadButton()
    {
        _startBtn.onClick.AddListener(MoveToMainMenu);
        _guestBtn.onClick.AddListener(() =>
        {
            AuthHandle.Instance.LoginAnonymous();
        });
        _googleBtn.onClick.AddListener(() =>
        {
            AuthHandle.Instance.LoginGoogle();
        });
        _facebookBtn.onClick.AddListener(() =>
        {
            AuthHandle.Instance.LoginFace();
        });
        
        _quitBtn.onClick.AddListener(Application.Quit);
    }

    private void MoveToMainMenu()
    {
        Hide();
        UIManager.Instance.ShowPanel(UIConstant.MainMenuPanel);
    }
}
