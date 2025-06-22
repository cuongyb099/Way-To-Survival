using System.Threading.Tasks;
using Tech.Singleton;
using UnityEngine;

public class BuildingSystem : Singleton<BuildingSystem>
{
    private Transform _player;
    private Material _indicatorPlacementMat;
    [SerializeField] private float _rangeIndicatorPlace = 3f;
    private static readonly Color _canPlaceColor = new (0.13f, 1, 0 ,0.09f);
    private static readonly Color _overlapColor = new (1, 0, 0 ,0.09f);
    
    private Vector3 curPosToPlace;
	private Transform _indicatorPrefab;
    
    protected override void Awake()
    {
        base.Awake();
        _player = FindObjectOfType<PlayerController>().transform;
        _indicatorPrefab = new GameObject("Indicator").transform;
        _indicatorPrefab.parent = transform;
        LoadMaterialAsync();
        InputEvent.OnBuilding += Build;
        InputEvent.OnRotateStructure += RotateIndicator;
    }

    private async void LoadMaterialAsync()
    {
	    while (!AddressablesManager.Instance)
	    {
		    await Task.Delay(100);
	    }
	    
	    _indicatorPlacementMat = await AddressablesManager.Instance
		    .LoadAssetAsync<Material>(MaterialConstant.BuildingIndicator);
    }

    private void Update()
    {
	    
    }

    private void OnDestroy()
	{
		InputEvent.OnBuilding -= Build;
		InputEvent.OnRotateStructure -= RotateIndicator;
	}
    private void RotateIndicator()
    {
	    
    }

    private void Build()
    {
	    
    }
}
