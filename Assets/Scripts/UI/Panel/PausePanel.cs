using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PausePanel : FadeBlurPanel
{
    [Header("Button")] 
    [SerializeField] private Button _resumeBtn;
    [SerializeField] private Button _settingBtn;
    [SerializeField] private Button _restartBtn;
    [SerializeField] private Button _quitBtn;

    protected override void OnAwake()
    {
        base.OnAwake();
        LoadButton();
    }

    private void LoadButton()
    {
        _resumeBtn.onClick.AddListener(() =>
        {
            Hide();
            UIManager.Instance.ShowPanel(UIConstant.GameplayPanel);
        });
        _settingBtn.onClick.AddListener(() =>
        {
            Hide();
            UIManager.Instance.ShowPanel(UIConstant.SettingsPanel);
        });
        _restartBtn.onClick.AddListener(() =>
        {
            //Unload
        });
        _quitBtn.onClick.AddListener(() =>
        {
            //Unload
        });
    }

    private void Reset()
    {
        ResetButton();
    }

    private void ResetButton()
    {
        var middle = transform.Find(UIConstant.Middle);

        if (!_resumeBtn)
        {
            _resumeBtn = middle.Find("Resume BTN").GetComponentInChildren<Button>();
        }
        
        if (!_settingBtn)
        {
            _settingBtn = middle.Find("Settings BTN").GetComponentInChildren<Button>();
        }
        
        if (!_restartBtn)
        {
            _restartBtn = middle.Find("Restart BTN").GetComponentInChildren<Button>();
        }
        
        if (!_quitBtn)
        {
            _quitBtn = middle.Find("Quit BTN").GetComponentInChildren<Button>();
        }
    }
    
    public override void Show()
    {
        base.Show();
        PlayerInput.Instance.InputActions.BasicAction.Disable();
    }

    public override void Hide()
    {
        base.Hide();
        PlayerInput.Instance.InputActions.BasicAction.Enable();
    }
}