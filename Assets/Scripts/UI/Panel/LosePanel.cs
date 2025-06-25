using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LosePanel : FadeBlurPanel
{
    [Header("Text")] 
    [SerializeField] private TextMeshProUGUI _waveText;
    [SerializeField] private TextMeshProUGUI _rewardText;
    [Header("Button")] 
    [SerializeField] private Button _quitBtn;
    
    private float reward = 0;

    protected override void OnAwake()
    {
        base.OnAwake();
        LoadButton();
        reward = 0;
    }

    private void LoadButton()
    {

        _quitBtn.onClick.AddListener(() =>
        {
            //Unload
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
            Resources.UnloadUnusedAssets();
            LoadingAsyncManager.Instance.SwitchToMainMenu();
        });
    }

    private void Reset()
    {
        ResetButton();
    }

    private void ResetButton()
    {
        var middle = transform.Find(UIConstant.Middle);

        if (!_quitBtn)
        {
            _quitBtn = middle.Find("Quit BTN").GetComponentInChildren<Button>();
        }
    }
    
    public override void Show()
    {
        base.Show();
        PlayerInput.Instance.InputActions.BasicAction.Disable();
        int wave = GameManager.Instance.WaveManager.CurrentWave;
        reward = (wave * 1000f + wave / 6 * 10000);
        _rewardText.text = reward.ToString();
        _waveText.text = wave.ToString();
    }

    public override void Hide()
    {
        base.Hide();
        PlayerInput.Instance.InputActions.BasicAction.Enable();
    }
}