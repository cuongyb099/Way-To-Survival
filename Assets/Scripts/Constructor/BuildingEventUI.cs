using UnityEngine;
using UnityEngine.Events;

public class BuildingEventUI : MonoBehaviour
{
    public UnityEvent OnIdleMode;
    public UnityEvent OnBuildingMode;
    public UnityEvent OnBuildSuccess;

    private void Awake()
    {
        BuildingSystem.Instance.OnIdleMode += HandleIdleMode;
        BuildingSystem.Instance.OnBuildSuccess += HandleBuildSucess;
        BuildingSystem.Instance.OnBuildingMode += HandleBuildingMode;
    }

    private void OnDestroy()
    {
        if(!BuildingSystem.Instance) return;
        
        BuildingSystem.Instance.OnIdleMode -= HandleIdleMode;
        BuildingSystem.Instance.OnBuildSuccess -= HandleBuildSucess;
        BuildingSystem.Instance.OnBuildingMode -= HandleBuildingMode;
    }

    private void HandleBuildSucess()
    {
        OnBuildSuccess?.Invoke();
    }

    private void HandleBuildingMode()
    {
        OnBuildingMode?.Invoke();
    }

    private void HandleIdleMode()
    {
        OnIdleMode?.Invoke();
    }

    public void Build() => BuildingSystem.Instance.Build();
    public void ChangeToBuildingMode() => BuildingSystem.Instance.SetMode(BuildingMode.BUILDING);
    public void ChangeToIdleMode() => BuildingSystem.Instance.SetMode(BuildingMode.IDLE);
}