using UnityEngine;
using UnityEngine.UI;

public class MainSelectorPanel : FadeBlurPanel
{
    [Header("Button")] 
    [SerializeField] private Button _startBtn;
    [SerializeField] private Button _quitBtn;

    protected override void OnAwake()
    {
        base.OnAwake();
        LoadButton();
    }

    public override void Hide()
    {
        base.Hide();
        MainMenuManager.Instance.GameTitle.SetActive(false);
    }

    public override void Show()
    {
        base.Show();
        MainMenuManager.Instance.GameTitle.SetActive(true);
    }

    private void LoadButton()
    {
        _startBtn.onClick.AddListener(() =>
        {
            Hide();
            UIManager.Instance.ShowPanel(UIConstant.MainMenuPanel);
        });
        
        _quitBtn.onClick.AddListener(Application.Quit);
    }
}
