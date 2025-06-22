using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class SettingsPanel : FadeBlurPanel
{
    [Header("Button")] 
    [SerializeField] private Button _backBtn;
    [Header("Slider")] 
    [SerializeField] private Slider _mainVolumeSlider;
    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private Slider _gameFXVolumeSlider;
    [SerializeField] private Slider _UIVolumeSlider;
    
    protected override void OnAwake()
    {
        LoadButton();
    }

    private void LoadButton()
    {
        _backBtn.onClick.AddListener(() =>
        {
            Hide();
            UIManager.Instance.ShowPanel(UIConstant.PausePanel);
        });
        _mainVolumeSlider.onValueChanged.AddListener(x =>
        {
            AudioManager.Instance.SetMasterVolume(x);
        });
        _musicVolumeSlider.onValueChanged.AddListener(x =>
        {
            AudioManager.Instance.SetMusicVolume(x);
        });
        _gameFXVolumeSlider.onValueChanged.AddListener(x =>
        {
            AudioManager.Instance.SetGameVolume(x);
        });
        _UIVolumeSlider.onValueChanged.AddListener(x =>
        {
            AudioManager.Instance.SetUIVolume(x);
        });
        OnShowDo.AddListener(() =>
        {
            CameraManager.Instance.SetCam(CameraType.SettingCamera);
        });
        OnHideDo.AddListener(() =>
        {
            CameraManager.Instance.SetCam(CameraType.MainCamera);
        });
    }
    
}
