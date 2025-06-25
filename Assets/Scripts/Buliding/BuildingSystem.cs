using System;
using System.Collections;
using Tech.Singleton;
using UnityEngine;

public class BuildingSystem : Singleton<BuildingSystem>
{
    public Action OnBuildingMode, OnIdleMode, OnBuildSuccess;
    
    protected BuildingMode currentMode = BuildingMode.IDLE;
    protected Coroutine checkingInput;
    protected Vector3 lastInput;
    
    public float MaxRangeBuild = 5f;
    [SerializeField] protected Transform buildPoint;
    
    public Color CanBuildColor, CanNotBuildColor;
    public TurretData CurrentData;
    public LayerMask ObstacleLayer;
    protected Structure CurrentBuilding;
    protected bool canBuild;
    protected Collider[] hitColliders = new Collider[3];
    
    public void SetMode(BuildingMode mode)
    {
        if(currentMode == mode) return;
        
        switch (mode)
        {
            case BuildingMode.IDLE:
                OnIdleMode?.Invoke();
                break;
            case BuildingMode.BUILDING:
                checkingInput = StartCoroutine(CheckingInput());
                break;
        }
        currentMode = mode;
    }
    
    protected IEnumerator CheckingInput()
    {
        CurrentBuilding = CurrentData.GetStructureRef();
        
        if(!CurrentBuilding) yield break;
        
        
        PlayerInput.Instance.EnableAttack = false;
        OnBuildingMode?.Invoke();
        while (true)
        {
            var input = PlayerInput.Instance.ShootStickInput * MaxRangeBuild;
            if (input != Vector3.zero)
            {
                lastInput = input;
            }
            
            CurrentBuilding.transform.position = transform.position + transform.forward * lastInput.magnitude;
            PlaceHitBox hitBox = CurrentBuilding.PlacmentHitBox;
            int hitCount = Physics.OverlapBoxNonAlloc(hitBox.transform.position, hitBox.Size / 2, hitColliders, hitBox.transform.rotation, ObstacleLayer);
            
            if (hitCount > 0)
            {
                CurrentBuilding.SetIndicator(CanNotBuildColor);
                canBuild = false;
                yield return null;
                continue;    
            }
            
            canBuild = true;
            CurrentBuilding.SetIndicator(CanBuildColor);
            yield return null;
        }
    }

    public void Build()
    {
        if(!CurrentBuilding || !canBuild) return;
        
        CurrentData.Quantity--;
        CurrentBuilding.ReturnDefault();
        PlayerInput.Instance.EnableAttack = true;
        StopCoroutine(checkingInput);
        OnBuildSuccess?.Invoke();
        SetMode(BuildingMode.IDLE);
    }
}

public enum BuildingMode
{
    IDLE,
    BUILDING,
}