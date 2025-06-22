using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Wave System/Wave Info")]
public class WaveInfoSO : ScriptableObject
{
    [field: SerializeField] public ChoosePointStrategy ChooseSpawnPointStrategy { get; protected set; }
    [field: SerializeField] public SpawnTimeStrategy TimeStrategy { get; protected set; }
    [field: SerializeField] public EnemyPrefab[] EnemiesPrefabs { get; protected set; }
    [field: SerializeField] public int SpecifiedWaveIndex { get; protected set; }
    [field: SerializeField, Range(0, 100f)] public float DelayTime { get; protected set; } = 0.25f;
    [field: SerializeField, Range(1, 1000)] public int Amount { get; protected set; } = 10;
    [field: SerializeField] public bool UseEnemyAmountChangeStrategy { get; protected set; }
    [field: SerializeField] public EnemyAmountChangeStrategy EnemyChangeStrategy { get; protected set; }
    [field: Header("Begin From Wave Start + 1 to Wave End")]
    [field: SerializeField, Range(0, 1000)] public int ChangeFlat { get; protected set; }
    [field: SerializeField, Range(0, 10)] public float ChangePercent { get; protected set; }
    [field: SerializeField] public AnimationCurve ChangeCurve { get; protected set; }
    [field: SerializeField] public int[] SpecifiedChange { get; protected set; }
}

[Serializable]
public class EnemyPrefab
{
    [Range(0,1)] public float SpawnRate = 1f;
    public GameObject Prefab;
}