using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Tech.Singleton;
using UnityEngine;

public enum CameraType
{
    MainCamera,
    SettingCamera,
}
public class CameraManager : Singleton<CameraManager>
{
    [SerializeField] private CinemachineVirtualCamera mainCam;
    [SerializeField] private CinemachineVirtualCamera settingCam;
    public CinemachineVirtualCamera CurrentCam { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        CurrentCam = mainCam;
    }

    public void SetCam(CameraType cameraType)
    {
        switch (cameraType)
        {
            case CameraType.MainCamera:
                SetCamera(mainCam);
                break;
            case CameraType.SettingCamera:
                SetCamera(settingCam);
                break;
        }
    }

    private void SetCamera(CinemachineVirtualCamera vcam)
    {
        if (vcam == CurrentCam) return;
        CurrentCam.enabled = false;
        vcam.enabled = true;
        CurrentCam = vcam;
    }

    [ContextMenu("Set Main Camera")]
    public void SetMainCamera()
    {
        SetCamera(settingCam);
    }
}
