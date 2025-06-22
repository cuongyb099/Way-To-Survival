using System;
using Tech.Singleton;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : Singleton<PlayerInput>
{
	public PlayerControls InputActions { get; private set; }
	public PlayerControls.BasicActionActions PlayerControlActions { get; private set; }
	//Input
	public Vector3 MovementInput => GetMovementInput();
	public Vector3 RotationInput => PlayerControlActions.PlayerRotate.ReadValue<Vector2>();
    public Vector3 ShootStickInput => PlayerControlActions.ShootStick.ReadValue<Vector2>();

    public bool IsWalkInput => walking && EnableWalk;
	public bool IsSprintInput => sprinting && EnableSprint;
	public bool IsAttackInput => attaking && EnableAttack;

	
	public bool IsInBuildingMode { get; private set; }

	public bool EnableAttack { get; set; } = true;
	public bool EnableSkills { get; set; } = true;
	public bool EnableWalk { get; set; } = true;
	public bool EnableSprint { get; set; } = true;

	private bool attaking = false;
	private bool walking = false;
	private bool sprinting = false;


    protected override void Awake()
	{
		base.Awake();
		InputActions = new PlayerControls();
		PlayerControlActions = InputActions.BasicAction;
        InputActions.Enable();
		AddListeners();
    }

	private void OnDestroy()
	{
		RemoveListeners();
	}

	private void OnEnable()
	{
		InputActions.Enable();
    }
	private void OnDisable()
	{
		InputActions.Disable();
	}
	private void AddListeners()
	{
		InputActions.BasicAction.BuidingMode.performed += HandleBuildingMode;
		InputActions.BasicAction.Buiding.performed += HandleBuilding;
		InputActions.BasicAction.SwitchGuns.performed += HandleSwitchGuns;
		InputActions.BasicAction.Rotate.performed += HandleRotateStructure;
        InputActions.BasicAction.Shoot.started += ShootInput;
        InputActions.BasicAction.Shoot.canceled += ShootInput;
        InputActions.BasicAction.ShootStick.canceled += ShootStick_canceled;
		InputActions.BasicAction.Reload.performed += HandleReloading;
		InputActions.BasicAction.Interact.started += InteractInput;
    }

    private void RemoveListeners()
	{
		InputActions.BasicAction.BuidingMode.performed -= HandleBuildingMode;
		InputActions.BasicAction.Buiding.performed -= HandleBuilding;
		InputActions.BasicAction.SwitchGuns.performed -= HandleSwitchGuns;
		InputActions.BasicAction.Rotate.performed -= HandleRotateStructure;
        InputActions.BasicAction.Shoot.started -= ShootInput;
        InputActions.BasicAction.Shoot.canceled -= ShootInput;
        InputActions.BasicAction.ShootStick.canceled -= ShootStick_canceled;
        InputActions.BasicAction.Reload.performed -= HandleReloading;
        InputActions.BasicAction.Interact.started -= InteractInput;
    }

    private void HandleReloading(InputAction.CallbackContext obj)
    {
        InputEvent.OnInputReloadGun?.Invoke();
    }
    private void ShootStick_canceled(InputAction.CallbackContext obj)
    {
		InputEvent.OnShootStickCanceled?.Invoke();
    }

    private void HandleRotateStructure(InputAction.CallbackContext context)
	{
		InputEvent.OnRotateStructure?.Invoke();
	}
	private void HandleBuildingMode(InputAction.CallbackContext ctx)
	{
		IsInBuildingMode = !IsInBuildingMode;
		InputEvent.OnBuildingMode?.Invoke();
	}

	private void HandleBuilding(InputAction.CallbackContext ctx)
	{
		InputEvent.OnBuilding?.Invoke();
	}

	private void HandleSwitchGuns(InputAction.CallbackContext ctx)
	{
		InputEvent.OnInputWeaponWheel?.Invoke();
	}
    private void ShootInput(InputAction.CallbackContext obj)
    {
		if (obj.canceled)
		{ 
			attaking = false;
			return; 
		}
		attaking = true;
    }
    private void InteractInput(InputAction.CallbackContext obj)
    {
        InputEvent.OnInputInteract?.Invoke();
    }

    //Methods
    private Vector3 GetMovementInput()
	{
		Vector2 v = PlayerControlActions.Movement.ReadValue<Vector2>();

        return new Vector3(v.x,0,v.y);
	}
}
