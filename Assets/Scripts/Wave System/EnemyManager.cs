using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tech.Pooling;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[RequireComponent(typeof(WaveManager))]
public class EnemyManager : Tech.Singleton.Singleton<EnemyManager>
{
    public int GetCurrentWave() => _waveManager.CurrentWave;
    [SerializeField] private SpawnPoint[] _spawnPoints;
    [SerializeField] private Transform _spawnPlane;
    [SerializeField] private string _overlapTag;
    [SerializeField] private float _overlapSpawnRadiusCheck = 1f;
    [SerializeField] private LayerMask _overlapLayerMask;
    private WaveManager _waveManager;
    private int _currentEnemyAmount;
    [HideInInspector] public UnityEvent<int> OnEnemyDead;
    private int _enemyDeadAmount;
    
    protected override void Awake()
    {
        base.Awake();
        cancellationTokenSource = new CancellationTokenSource();
        token = cancellationTokenSource.Token;
        _waveManager = GetComponent<WaveManager>();
        GameEvent.WaveDoneEvent += ResetEnemyDeadAmount;
    }

    private void OnDestroy()
    {
        GameEvent.WaveDoneEvent -= ResetEnemyDeadAmount;
        cancellationTokenSource.Cancel();
	}

    private void ResetEnemyDeadAmount(int currentWave)
    {
        OnEnemyDead.RemoveAllListeners();
        _enemyDeadAmount = 0;
    }
    
    private void Start()
    {
        _currentEnemyAmount = 0;
    }

    public void ReturnEnemyToPool()
    {
        _currentEnemyAmount--;
        _enemyDeadAmount++;
        GameEvent.EnemyDeadEvent?.Invoke(_currentEnemyAmount);
        OnEnemyDead?.Invoke(_enemyDeadAmount);
    }
    
    [ContextMenu("All In Random Strategy")]
    public void Test()
    {
        Spawn(30, ChoosePointStrategy.ALL_IN_RANDOM)
            .SetTimeStrategy(SpawnTimeStrategy.DELAY_TIME, 1f)
            .StartAsync();
    }
    
    [ContextMenu("Space Between Strategy")]
    public void Test2()
    {
        Spawn(30, ChoosePointStrategy.SPLIT_EQUAL_QUEUE)
            .SetTimeStrategy(SpawnTimeStrategy.DELAY_TIME, 1f)
            .StartAsync();
    }
    
    [ContextMenu("Random Split Strategy")]
    public void Test3()
    {
        Spawn(30, ChoosePointStrategy.RANDOM_SPLIT)
            .SetTimeStrategy(SpawnTimeStrategy.DELAY_TIME, 1f)
            .StartAsync();
    }
    
    [ContextMenu("Split Equal Random Strategy")]
    public void Test4()
    {
        Spawn(30, ChoosePointStrategy.SPLIT_EQUAL_RANDOM)
            .SetTimeStrategy(SpawnTimeStrategy.DELAY_TIME, 1f)
            .StartAsync();
    }
    
    public Spawner Spawn(SpawnPoint spawnPoint, int amount)
    {
        if(!_spawnPoints.Contains(spawnPoint)) return null;

        Spawner spawner = new Spawner()
        {
            M_SpawnPoint = spawnPoint,
            Amount = amount,
        };
       
        return spawner;
    }

    public Spawner Spawn(int amount, ChoosePointStrategy pointStrategy)
    {
        Spawner spawner = new Spawner()
        {
            M_ChoosePointStrategy = pointStrategy,
            Amount = amount,
        };
        
        return spawner;
    }

    public Spawner Spawn(int specifiedWaveIndex, int amount)
    {
        SpawnPoint point = _spawnPoints[math.clamp(specifiedWaveIndex, 0, _spawnPoints.Length - 1)];
        Spawner spawner = new Spawner()
        {
            M_SpawnPoint = point,    
            Amount = amount,
            M_ChoosePointStrategy = ChoosePointStrategy.NONE
        };
        
        return spawner;
    }
    
    private CancellationTokenSource cancellationTokenSource;
    private CancellationToken token;
    public async void StartSpawn(Spawner spawner)
    {
        try
        {
            await SpawnEnemyAsync(spawner, token);
        }
        catch (Exception e)
        {
            throw e;
        }
    }
    
    
    
    public async Task SpawnEnemyAsync(Spawner spawner, CancellationToken token)
    {
        try
        {
            var count = 0;
            var spaceBetween = spawner.Amount / _spawnPoints.Length;
            var spaceBetweenIndex = 0;
            List<SplitEqual> waypointsIndexes = null;

            if (spawner.M_ChoosePointStrategy == ChoosePointStrategy.SPLIT_EQUAL_RANDOM)
            {
                waypointsIndexes = new ();
                for (int i = 0; i < _spawnPoints.Length; i++)
                {
                    waypointsIndexes.Add(new SplitEqual()
                    {
                        Count = 0,
                        Index = i
                    });
                }
            }
            
            Vector3 finalPosition = Vector3.zero;
            SpawnPoint currentSpawnPoint = spawner.M_SpawnPoint;
            
            switch (spawner.M_ChoosePointStrategy)
            {
                case ChoosePointStrategy.ALL_IN_RANDOM:
                    currentSpawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)] ;
                    break;
            }

            var colliders = new Collider[10];

            while (count < spawner.Amount)
            {
                if (!Application.isPlaying) break;

                var isOverlap = false;

                switch (spawner.M_ChoosePointStrategy)
                {
                    case ChoosePointStrategy.SPLIT_EQUAL_QUEUE:
                        currentSpawnPoint = _spawnPoints[spaceBetweenIndex];

                        if (count > 0 && count % spaceBetween == 0)
                        {
                            spaceBetweenIndex++;

                            if (spaceBetweenIndex > _spawnPoints.Length - 1)
                            {
                                spaceBetweenIndex = _spawnPoints.Length - 1;
                            }
                        }

                        break;
                    case ChoosePointStrategy.RANDOM_SPLIT:
                        currentSpawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
                        break;
                    case ChoosePointStrategy.SPLIT_EQUAL_RANDOM:
                        if (waypointsIndexes.Count <= 0)
                        {
                            currentSpawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
                            break;
                        }

                        var randomIndex = Random.Range(0, waypointsIndexes.Count);
                        currentSpawnPoint = _spawnPoints[randomIndex];
                        waypointsIndexes[randomIndex].Count++;
                        if (waypointsIndexes[randomIndex].Count % spaceBetween == 0
                            && waypointsIndexes[randomIndex].Count > 0)
                        {
                            waypointsIndexes.RemoveAt(randomIndex);
                        }

                        break;
                }

                finalPosition = currentSpawnPoint.M_Transform.position;
                finalPosition.y = _spawnPlane.position.y;
                finalPosition.x += Random.Range(-currentSpawnPoint.Radius, currentSpawnPoint.Radius);
                finalPosition.z += Random.Range(-currentSpawnPoint.Radius, currentSpawnPoint.Radius);
                finalPosition += currentSpawnPoint.SpawnOffset;

                var hitCount = Physics.OverlapSphereNonAlloc(finalPosition,
                    _overlapSpawnRadiusCheck, colliders, _overlapLayerMask);
                for (int i = 0; i < hitCount; i++)
                {
                    if (!colliders[i].transform.CompareTag(_overlapTag)) continue;

                    await Task.Delay(TimeSpan.FromSeconds(Time.deltaTime));
                    isOverlap = true;
                    break;
                }

                if (isOverlap) continue;

                switch (spawner.M_SpawnTimeStrategy)
                {
                    case SpawnTimeStrategy.DEFAULT:
                        break;
                    case SpawnTimeStrategy.DELAY_TIME:
                        await Task.Delay(TimeSpan.FromSeconds(spawner.DelayTime));
                        break;
                }

                GameObject disiredPrefab = null;
                var randomValue = Random.value;
                float minRate = 0;
                float maxRate = spawner.EnemyPrefabs[0].SpawnRate;
                var index = 0;
                
                while (!disiredPrefab || minRate >= 1)
                {
                    if (randomValue >= minRate && randomValue <= maxRate)
                    {
                        disiredPrefab = spawner.EnemyPrefabs[index].Prefab;
                    }

                    index++;
                    if (index > spawner.EnemyPrefabs.Length - 1) break;
                    minRate = maxRate;
                    maxRate += spawner.EnemyPrefabs[index].SpawnRate;
                }
                count++;
                spawner.CallbackSpawn?.Invoke(count);
                if(!disiredPrefab) continue; 
                _currentEnemyAmount++;
                GameObject obj = ObjectPool.Instance.SpawnObject(disiredPrefab, finalPosition, Quaternion.identity, PoolType.ENEMY);
                GameEvent.EnemySpawnEvent?.Invoke(obj);
            }
            
            //Spawn At Least 1 Enemy If Spawn Rate Random All In To Null Value Prefab
            if (_currentEnemyAmount == 0)
            {
                _currentEnemyAmount++;
                GameObject obj = ObjectPool.Instance.SpawnObject(spawner.EnemyPrefabs[0].Prefab, finalPosition,
                    Quaternion.identity, PoolType.ENEMY);
                GameEvent.EnemySpawnEvent?.Invoke(obj);
            }
            _waveManager.DisposeSpawner();
            spawner.CallbackSpawnerDone?.Invoke();
        }
        catch (Exception e)
        {
          
        }
        
        if (token.IsCancellationRequested)
        {
           
        }
    }
    
#if UNITY_EDITOR
    public bool DrawDebug;
    private void OnDrawGizmos()
    {
        if(!DrawDebug) return;
        
        if(_spawnPoints.Length <= 0) return;
        foreach (var point in _spawnPoints)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(point.M_Transform.position, point.Radius);
            //Draw Overlap Size Check
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(point.M_Transform.position, _overlapSpawnRadiusCheck);
        }
    }
    
    //Utilities Funciton
    [ContextMenu("Auto Find All Spawn Points")]
    public void AutoFindAllSpawnPoints()
    {
        var temp = new List<SpawnPoint>();
        foreach (Transform child in GetComponentsInChildren<Transform>())
        {
            if(child.name.StartsWith("Spawn Point"))
            {
                temp.Add(new SpawnPoint()
                {
                    Radius = 15f,
                    M_Transform = child
                });       
            }
        }
        
        _spawnPoints = temp.ToArray();
    }
#endif
}

public static class SpawnerExtension
{
     public static Spawner SetTimeStrategy(this Spawner spawner, SpawnTimeStrategy strategy, float delayTime = 0)
    {
        spawner.M_SpawnTimeStrategy = strategy;
        spawner.DelayTime = delayTime;
        return spawner;
    }

    public static Spawner SetPrefab(this Spawner spawner, EnemyPrefab[] prefabs)
    {
        spawner.EnemyPrefabs = prefabs;
        return spawner;
    }
     
    public static Spawner SetChoosePointStrategy(this Spawner spawner, ChoosePointStrategy strategy)
    {
        spawner.M_ChoosePointStrategy = strategy;
        return spawner;
    }
    
    public static Spawner OnSpawn(this Spawner spawner, Action<int> onSpawn)
    {
        spawner.CallbackSpawn += onSpawn;
        return spawner;
    }
    
    public static Spawner OnComplete(this Spawner spawner, Action onComplete)
    {
        spawner.CallbackSpawnerDone += onComplete;
        return spawner;
    }
    
    public static void StartAsync(this Spawner spawner)
    {
        EnemyManager.Instance.StartSpawn(spawner);
    }
}

public class Spawner
{
    public int Amount = 1;
    public float DelayTime = 0;
    public SpawnPoint M_SpawnPoint = null;
    public SpawnTimeStrategy M_SpawnTimeStrategy = SpawnTimeStrategy.DEFAULT;
    public ChoosePointStrategy M_ChoosePointStrategy = ChoosePointStrategy.NONE;
    public EnemyPrefab[] EnemyPrefabs = null;
    public Action CallbackSpawnerDone;
    public Action<int> CallbackSpawn;
}

public enum SpawnTimeStrategy
{
    DEFAULT,
    DELAY_TIME
}
    
public enum ChoosePointStrategy
{
    //Special Waypoint
    NONE,
    //Chose Random Waypoint In Array And Spawn All In These
    ALL_IN_RANDOM,
    //Split Equal Into Each Waypoint With Order Waypoint
    SPLIT_EQUAL_QUEUE,
    //Split Equal Into Each Waypoint But Random Waypoint Each Spawn
    SPLIT_EQUAL_RANDOM,
    //Random Waypoint To Spawn
    RANDOM_SPLIT,
}

[Serializable]
public class SpawnPoint 
{
    public float Radius;
    public Vector3 SpawnOffset;
    public Transform M_Transform;
}

public class SplitEqual
{
    public int Index;
    public int Count;
}