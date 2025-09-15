using DefaultNamespace.Constructor.Drum;
using DG.Tweening;
using ResilientCore;
using UnityEngine;

public class PlayerController : BasicController
{
	#region AnimationID
	private static readonly int PlayerHit = Animator.StringToHash("PlayerHit");
	private static readonly int Type = Animator.StringToHash("WeaponType");
	private static readonly int ReloadType = Animator.StringToHash("ReloadAnimation");
	private static readonly int SwitchCurWeapon = Animator.StringToHash("SwitchWeapon");
	private static readonly int ReloadGun = Animator.StringToHash("ReloadGun");
	private static readonly int ShootingSpeed = Animator.StringToHash("ShootingSpeed");
	private static readonly int MovementSpeed = Animator.StringToHash("MovementSpeed");
	private static readonly int Shoot = Animator.StringToHash("Shoot");
	private static readonly int PosX = Animator.StringToHash("PosX");
	private static readonly int PosY = Animator.StringToHash("PosY");
	private static readonly int PlayerDead = Animator.StringToHash("IsDead");
	#endregion
	public Rigidbody Rigidbody { get; private set; }
    public FloatingCapsule FloatingCapsule { get; private set; }
    public CapsuleCollider Collider { get; private set; }
    public Animator Animator { get; private set; }
    [field:SerializeField] public CameraZoom CameraZoom { get; private set; }
    public PlayerInteractor PlayerInteractor { get; private set; }
	//Player Data
	[field: SerializeField]public float GunSwitchCooldown { get; private set; } = .1f;
    [field: SerializeField]public LayerMask GroundLayer{ get; private set; }
    [field: SerializeField] public WeaponBaseSO StartingWeapon{ get; private set; }
    public int Money
    {
	    get => money;
	    set
	    {
		    money = value;
		    PlayerEvent.OnCoinChange?.Invoke(money);
	    }
    }
    private int money;
    //WeaponSystem
    public Transform RightHandHoldPoint;
    public Transform LeftHandHoldPoint;
    public LineRendererHelper LineRendererL;
    public LineRendererHelper LineRendererR;
    public WeaponBase[] Weapons ;
    [field: SerializeField] public BoxCollider MeleeHitCollider { get; private set; }  
    public WeaponBase CurrentWeapon => Weapons[currentWeaponIndex];
    private bool weaponSwitchable = true;
    private Camera mainCamera;
    private int currentWeaponIndex;
    private int lastWeaponIndex;
    
    private const int MaxWeapon = 4;
    
    private OverlapDamageSender overlapDamageSender;
    
	//Unity and override methods
    protected override void Awake()
    {
        base.Awake();
        Rigidbody = GetComponent<Rigidbody>();
        Collider = GetComponent<CapsuleCollider>();
        FloatingCapsule = GetComponent<FloatingCapsule>();
        if(!Animator) Animator = GetComponentInChildren<Animator>();
        PlayerInteractor = GetComponentInChildren<PlayerInteractor>();
        mainCamera = Camera.main;
        overlapDamageSender = GetComponent<OverlapDamageSender>();

        Weapons = new WeaponBase[MaxWeapon] {null,null,null,null};
		
		PlayerEvent.OnAttack += SetShootAnim;
		PlayerEvent.OnRecieveCoin += AddCash;
		PlayerEvent.OnRecieveGunAmmo += AddGunAmmo;
        Stats.GetStat(StatType.GunMagMultiplier).OnValueChange += CalculateMaxCap;
		Stats.GetStat(StatType.ATKSpeed).OnValueChange += SetShootingSpeedAnim;
		Stats.GetStat(StatType.Speed).OnValueChange += SetMovementSpeedAnim;
		Stats.GetAttribute(AttributeType.HoldingBullets).OnValueChange += () => { PlayerEvent.OnAmmoPointsChange?.Invoke(); };
    }
    private void OnDestroy()
    {
        hp.OnValueChange -= HandleHealthChange;
        maxHp.OnValueChange -= HandleMaxHpChange;
		
		PlayerEvent.OnAttack -= SetShootAnim;
		PlayerEvent.OnRecieveCoin -= AddCash;
		PlayerEvent.OnRecieveGunAmmo -= AddGunAmmo;
		Stats.GetStat(StatType.GunMagMultiplier).OnValueChange -= CalculateMaxCap;
		Stats.GetStat(StatType.ATKSpeed).OnValueChange -= SetShootingSpeedAnim;
		Stats.GetStat(StatType.Speed).OnValueChange -= SetMovementSpeedAnim;
	}
	private void Start()
    {
	    InstantiateWeapon((WeaponData)StartingWeapon.CreateItemData(), 3);
	    currentWeaponIndex = -1;
	    if(PlayerDataPersistent.Instance != null)
		    PlayerDataPersistent.Instance.ApplyToPlayer(this);
	    
	    EquipAvaliableWeapon();
        InitHealthBar();
        
        Stats.GetAttribute(AttributeType.Hp).SetValueToMax();
        Money = 100000;
    }

    void FixedUpdate()
    {
	    MovePlayer();
        Float();
    }
	private void Update()
	{
		SetLineRenderers();
		
		RotatePlayer();
    }

    public override void Death(GameObject dealer)
    {
        base.Death(dealer);
        Weapons[currentWeaponIndex].gameObject.SetActive(false);
        PlayerInput.Instance.InputActions.Disable();
        Animator.SetBool(PlayerDead, true);
        UIManager.Instance.ShowPanel(UIConstant.LosePanel); 
    }
    public override float Damage(DamageInfo info)
    {
	    
	    Animator.SetTrigger(PlayerHit);
	    return base.Damage(info);
    }
    
	[ContextMenu("Revive")]
    public void Revive()
    {
	    isDead = false;
	    Stats.GetAttribute(AttributeType.Hp).SetValueToMax();
	    Weapons[currentWeaponIndex].gameObject.SetActive(true);
	    //DealDamage
	    overlapDamageSender.Damage = Stats.GetStat(StatType.ATK).Value * 50f;
	    overlapDamageSender.DealDamage();
	    
	    PlayerInput.Instance.InputActions.Enable();
	    Animator.SetBool(PlayerDead, false);
	    UIManager.Instance.HidePanel(UIConstant.LosePanel);
    }

    // Weapon handle
    
    public void InstantiateWeapon(WeaponData weaponData, int index)
    {
	    if (Weapons[index] != null)
	    {
		    Stats.GetStat(StatType.Speed).RemoveModifier(new StatModifier(-Weapons[index].WeaponData.Weight.Value,StatModType.Flat));
		    Destroy(Weapons[index].gameObject);
	    }

	    var weapon = weaponData.GoReference;
	    weapon.gameObject.SetActive(false);
	    weapon.transform.SetParent(RightHandHoldPoint, false);
	    Weapons[index] = weapon.GetComponent<WeaponBase>();
	    Weapons[index].gameObject.layer = gameObject.layer;
	    Weapons[index].OnInit();
	    
	    Animator.SetBool(ReloadGun, false);
	    Animator.SetBool(SwitchCurWeapon, false);
	    
	    //if weapon is a gun
	    if (Weapons[index] is GunBase gun)
	    {
		    CalculateMaxCapacity(Weapons[index]);
		    gun.SetBulletToMax();
	    }
    }
    
    public void SwapGuns(int x, int y)
    {
	    (Weapons[x], Weapons[y]) = (Weapons[y], Weapons[x]);
	    EquipWeapon(currentWeaponIndex);
    }
    
    public bool EquipWeapon(int index)
    {
	    WeaponBase currentSlot = Weapons[index];

	    if (currentSlot == null) return false;
	    if (currentWeaponIndex != -1)
	    {
		    Stats.GetStat(StatType.Speed).RemoveModifier(new StatModifier(-Weapons[currentWeaponIndex].WeaponData.Weight.Value,StatModType.Flat));
		    Weapons[currentWeaponIndex].gameObject.SetActive(false);
	    }
        currentSlot.gameObject.SetActive(true);
        lastWeaponIndex = currentWeaponIndex;
        currentWeaponIndex = index;
        CalculateMaxCapacity(Weapons[index]);
        Animator.SetFloat(Type, (float)currentSlot.WeaponData.WeaponSO.WeaponType);
        Animator.SetBool(ReloadGun, false);
        
        Stats.GetStat(StatType.Speed).AddModifier(new StatModifier(-currentSlot.WeaponData.Weight.Value,StatModType.Flat));
        
        if (currentSlot is not GunBase gun)
        {
	        CameraZoom.SetZoom(1f/Mathf.Cos(45f*Mathf.Deg2Rad));
        }
        else
        {
	        Animator.SetFloat(ReloadType, (float)gun.GunData.GunSO.GunReloadType);
	        CameraZoom.SetZoom(((GunBase)CurrentWeapon).GunData.Aim.Value/Mathf.Cos(45f*Mathf.Deg2Rad));
        }
        PlayerEvent.OnEquipWeapon?.Invoke(currentSlot);
        return true;
    }

    public void SwitchWeapon(int index)
    {
	    if(!weaponSwitchable ||
	       currentWeaponIndex == index ||
	       Weapons[index]== null) return;
	    weaponSwitchable = false;
	    DOVirtual.DelayedCall(GunSwitchCooldown, () => { weaponSwitchable = true; });
	    //Switch
	    BeforeSwitching();
	    Animator.SetBool(SwitchCurWeapon, true);
	    Animator.SetBool(ReloadGun, false);
	    Weapons[currentWeaponIndex].OnSwitchOut();
	    EquipWeapon(index);
    }

    public void SwitchToLastWeapon()
    {
	    if(Weapons[lastWeaponIndex])
			SwitchWeapon(lastWeaponIndex);
    }
    private void EquipAvaliableWeapon()
    {
	    for (int i = 0; i< Weapons.Length;i++)
	    {
		    if (Weapons[i] == null) continue;
		    EquipWeapon(i);
		    return;
	    }
	    
    }
    public bool ContainsWeapon(WeaponBase weapon)
    {
	    foreach (var x in Weapons)
	    {
		    if (x.Equals(weapon)) return true;
	    }

	    return false;
    }


	public void BeforeSwitching()
	{
		foreach (var weapon in Weapons)
		{
			if(!weapon) continue;
			weapon.ShootAble = false;
		}
		Weapons[currentWeaponIndex].OnSwitchOut();

	}
	public void AfterSwitching()
	{
		Weapons[currentWeaponIndex].OnSwitchIn();
		foreach (var weapon in Weapons)
		{
			if(!weapon) continue;
			weapon.ShootAble = true;
		}
	}
    public void AfterReload()
    {
	    GunBase gun = ((GunBase)Weapons[currentWeaponIndex]);
        gun.ReloadBullet();
		PlayerEvent.OnReloaded?.Invoke();
		Animator.SetBool(ReloadGun, false);
        AfterSwitching();
	}
    public void SetShootingSpeedAnim()
    {
		Animator.SetFloat(ShootingSpeed,Stats.GetStat(StatType.ATKSpeed).Value);
	}
	public void SetMovementSpeedAnim()
	{
		float value = Stats.GetStat(StatType.Speed).Value;
		float baseValue = Stats.GetStat(StatType.Speed).BaseValue;
		Animator.SetFloat(MovementSpeed, value>baseValue?(value/baseValue):1f);
	}
	public void SetShootAnim()
	{
		Animator.SetTrigger(Shoot);
	}
	public void EnableLineRenderer()
	{
		LineRendererL.LR.enabled = true;
		LineRendererR.LR.enabled = true;
	}
	public void DisableLineRenderer()
    {
        LineRendererL.LR.enabled = false;
		LineRendererR.LR.enabled = false;
	}

	public Gradient LineDefaultColor;
	public Gradient LineTargetColor;
	public void SetLineRenderers()
	{
        if(CurrentWeapon.WeaponData.WeaponSO.WeaponType is WeaponType.Knife or WeaponType.SpecialWeapon) return;
        GunBase gun = (GunBase)CurrentWeapon;
        float accuracy = gun.GunAccuracy;
		LineRendererL.SetLineRenderer(gun.ShootPoint, gun.GunData.Aim.Value, Quaternion.Euler(
			0, Mathf.Clamp(-accuracy, -GameValues.RecoilMaxValue,0), 0) * transform.forward);
		LineRendererR.SetLineRenderer(gun.ShootPoint, gun.GunData.Aim.Value, Quaternion.Euler(
			0, Mathf.Clamp(accuracy, 0, GameValues.RecoilMaxValue), 0) * transform.forward);
		
        if(Physics.Raycast(gun.ShootPoint.position,transform.forward, out RaycastHit hit, gun.GunData.Aim.Value) &&
           accuracy <= 1f)
        {
	        if (hit.collider.CompareTag("Enemy"))
	        {
		        LineRendererL.LR.colorGradient = LineTargetColor;
		        LineRendererR.LR.colorGradient = LineTargetColor;
	        }
	        
	        return;
        }
        
        LineRendererL.LR.colorGradient = LineDefaultColor;
        LineRendererR.LR.colorGradient = LineDefaultColor;

	}

	// Movement
	Vector2 CurrentBlend;
    private void MovePlayer()
    {
        Vector3 MovementInput = PlayerInput.Instance.MovementInput;
        Rigidbody.velocity = Quaternion.Euler(0,mainCamera.transform.eulerAngles.y,0) * MovementInput * ((Stats.GetStat(StatType.Speed).Value));
        //Animation
        var x =Vector3.Dot(MovementInput, Quaternion.Euler(0,-mainCamera.transform.eulerAngles.y,0)* transform.right);
        var y =Vector3.Dot(MovementInput, Quaternion.Euler(0,-mainCamera.transform.eulerAngles.y,0)* transform.forward);
        CurrentBlend = Vector2.Lerp(CurrentBlend, new Vector2(x,y) , 0.1f);
        Animator.SetFloat(PosX,CurrentBlend.x*Stats.GetStat(StatType.Speed).Value/Stats.GetStat(StatType.Speed).BaseValue);
        Animator.SetFloat(PosY,CurrentBlend.y*Stats.GetStat(StatType.Speed).Value/Stats.GetStat(StatType.Speed).BaseValue);
    }

    public void RotatePlayer()
    {
        Vector2 rotateInput = PlayerInput.Instance.RotationInput;
        if (rotateInput == Vector2.zero) return;
        float atan = Mathf.Atan2(rotateInput.x,rotateInput.y)*Mathf.Rad2Deg;
        Rigidbody.rotation = Quaternion.Euler(0,atan+mainCamera.transform.eulerAngles.y,0);
    }
    public void Float()
    {
        Ray ray = new Ray(FloatingCapsule.CapsuleColliderData.Collider.bounds.center, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, FloatingCapsule.FloatingData.FloatRayLength, GroundLayer, QueryTriggerInteraction.Ignore))
        {
            float distanceFromGround = FloatingCapsule.CapsuleColliderData.ColliderCenterLocalSpace.y * transform.localScale.y - hit.distance;
            if (distanceFromGround == 0)
            {
                return;
            }
            float liftAmount = distanceFromGround * FloatingCapsule.FloatingData.StepHeightMultiplier - Rigidbody.velocity.y;
            Rigidbody.AddForce(new Vector3(0, liftAmount, 0), ForceMode.VelocityChange);
            
            return;
        }
    }

    //Health Bar
    private Attribute hp;
    private Stat maxHp;
    private void InitHealthBar()
    {
        if (Stats.TryGetAttribute(AttributeType.Hp, out hp))
        {
            hp.OnValueChange += HandleHealthChange;
            PlayerEvent.OnInitStatusBar?.Invoke(AttributeType.Hp, hp.Value, hp.MaxValue);
        }

        if (Stats.TryGetStat(StatType.MaxHP, out maxHp))
        {
            maxHp.OnValueChange += HandleMaxHpChange;
        }

        HandleMaxHpChange();
    }

    private void HandleMaxHpChange()
    {
        PlayerEvent.OnMaxHeathChange?.Invoke(hp.Value, maxHp.Value);
    }

    private void HandleHealthChange()
    {
        if (hp == null) return;
        PlayerEvent.OnHeathChange?.Invoke(hp.Value, hp.MaxValue);
    }

    public void CalculateMaxCap()
    {
	    for (int i = 0; i < Weapons.Length; i++)
	    {
		    if(!Weapons[i]) continue;
		    if(Weapons[i] is GunBase gun)
				gun.SetBulletCap(Stats.GetStat(StatType.GunMagMultiplier).Value);
	    }

	    PlayerEvent.OnChangeCap?.Invoke();
    }

    public void CalculateMaxCapacity(WeaponBase weaponBase)
    {
	    if(!weaponBase) return;
	    if(weaponBase is GunBase gun)
			gun.SetBulletCap(Stats.GetStat(StatType.GunMagMultiplier).Value);
    }
    //Cash
    public void AddCash(int amount)
    {
	    DamagePopUpGenerator.Instance.CreateCashPopUp(transform.position, $"+{amount} $");
	    Money += amount;
    }
    
    public void AddGunAmmo(int amount)
    {
	    Stats.GetAttribute(AttributeType.HoldingBullets).Value += amount;
    }
}
