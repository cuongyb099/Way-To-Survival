using System;
using System.Linq;
using DG.Tweening;
using Tech.Logger;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public int CurrentWave { get; private set; }
    private int _activeSpawner;
    private bool _gameDone;
    
    [SerializeField] private int _maxWaveCount = 20;
    [SerializeField] private Wave[] _waves;
    //Dotween
    private Tween _shoppingTimeTween;
    private TweenCallback _tweenCallbackNextWave;
    private TweenCallback<float> _timerCallback;
    
    private void Awake()
    {
        NormalizeWaves();

        _tweenCallbackNextWave += NextWave;
        _timerCallback += HandleShoppingTimeChange;

        GameEvent.EnemyDeadEvent += EndWaveCheck;
        GameEvent.OnStartWinState += () => CurrentWave++;
        GameEvent.OnStartCombatState += StartWave;
    }

    private void OnDestroy()
    {
        _tweenCallbackNextWave -= NextWave;
        _timerCallback -= HandleShoppingTimeChange;
        
        GameEvent.EnemyDeadEvent -= EndWaveCheck;
        GameEvent.OnStartWinState -= () => CurrentWave++;
        GameEvent.OnStartCombatState -= StartWave;
    }
    
    private void SkipToNextWave()
    {
        if(!_shoppingTimeTween.IsActive()) return;
        _shoppingTimeTween.Kill();
    }

    private void Start()
    {
        CurrentWave = 1;
    }

    private void LoadGame(int waveIndex)
    {
        CurrentWave = waveIndex;
        StartWave();
    }

    private void HandleShoppingTimeChange(float currentTime)
    {
        GameEvent.ShoppingTimeChangeEvent?.Invoke(currentTime);
    }
    
    public void StartWave()
    {
        GameEvent.NextWaveEvent?.Invoke(CurrentWave);
        Wave wave = FindWave(CurrentWave);

        for (int i = 0; i < wave.MainWave.Length; i++)
        {
            Spawner spawner = CreateSpawner(wave, wave.MainWave[i]);

            for (int j = 0; j < wave.WaveChild.Length; j++)
            {
                switch (wave.WaveChild[j].SpawnStrategy)
                {
                    case WaveChildSpawnStrategy.CALL_AFTER_SPECIFIED_MAINWAVE_SPAWN_DONE:
                        if(i != wave.WaveChild[j].MainWaveIndex) continue;
                        var tempIndex = j;
                        _activeSpawner++;
                        spawner.OnComplete(() =>
                        {
                            CreateSpawner(wave, wave.WaveChild[tempIndex].Info).StartAsync();
                        });
                        break;
                    case WaveChildSpawnStrategy.CALL_AFTER_ENEMY_SPAWN_AMOUNT:
                        tempIndex = j;
                        spawner.OnSpawn((count) =>
                        {
                            if (count == wave.WaveChild[tempIndex].EnemyAmount)
                            {
                                _activeSpawner++;
                                CreateSpawner(wave, wave.WaveChild[tempIndex].Info).StartAsync();
                            }
                        });
                        break;
                    case WaveChildSpawnStrategy.CALL_AFTER_ENEMY_AMOUNT_DEAD:
                        //Closure allocation But Dont Know How To Optimize :)
                        tempIndex = j;
                        EnemyManager.Instance.OnEnemyDead.AddListener((amount) =>
                        {
                            if (amount == wave.WaveChild[tempIndex].EnemyAmount)
                            {
                                _activeSpawner++;
                                CreateSpawner(wave, wave.WaveChild[tempIndex].Info).StartAsync();
                            }
                        }); 
                        break;
                }
            }
            
            spawner.StartAsync();
            _activeSpawner++;       
        }
    }

    private Spawner CreateSpawner(Wave wave, WaveInfoSO waveInfo)
    {
        var amount = waveInfo.Amount;
        
        if (waveInfo.UseEnemyAmountChangeStrategy)
        {
            switch (waveInfo.EnemyChangeStrategy)
            {
                case EnemyAmountChangeStrategy.INCREASE_STEADILY_FLAT:
                    amount = SteadilyFlatLogic(wave, waveInfo);
                    break;
                case EnemyAmountChangeStrategy.DECREASE_STEADILY_FLAT:
                    amount = SteadilyFlatLogic(wave, waveInfo);
                    break;
                case EnemyAmountChangeStrategy.INCREASE_STEADILY_PERCENT:
                    amount = SteadilyPercentLogic(wave, waveInfo);
                    break;
                case EnemyAmountChangeStrategy.DECREASE_STEADILY_PERCENT:
                    amount = SteadilyPercentLogic(wave, waveInfo);
                    break;
                case EnemyAmountChangeStrategy.CHANGE_WITH_CURVE:
                    amount = ChangeWithCurveLogic(wave, waveInfo);
                    break;
                case EnemyAmountChangeStrategy.CHANGE_WITH_SPECIFIED:
                    amount = ChangeWithSpecified(wave, waveInfo);
                    break;
            }        
        }
        
        return EnemyManager.Instance.Spawn(waveInfo.SpecifiedWaveIndex, amount)
            .SetChoosePointStrategy(waveInfo.ChooseSpawnPointStrategy)
            .SetTimeStrategy(waveInfo.TimeStrategy, waveInfo.DelayTime)
            .SetPrefab(waveInfo.EnemiesPrefabs);
    }
    
    [ContextMenu("Normalize Waves")]
    public void NormalizeWaves()
    {
        //Info Checking
        if (_waves.Length > _maxWaveCount)
        {
            var distance = _waves.Length - _maxWaveCount;
            var tmp = _waves.ToList();
            tmp.RemoveRange(_waves.Length - distance, distance);
            _waves = tmp.ToArray();
        }
        
        _waves[0].Start = 1;
        _waves[^1].End = _maxWaveCount;
        
        if (_waves.Length <= 0)
        {
            LogCommon.LogWarning("Error Game Wave Will Not Start Correctly");
        }
        
        for (int i = 0; i < _waves.Length; i++)
        {
            if (_waves[i].MainWave.Length <= 0)
            {
                LogCommon.LogError("Main Wave Not Null");
            }

            if(i != _waves.Length - 1)
            {
                var remainingWave = _waves.Length - i - 1;
                if (_waves[i].End > _maxWaveCount - remainingWave)
                {
                    _waves[i].End = _maxWaveCount - remainingWave;
                }
                else if(i != 0)
                {
                    if (_waves[i].End <= _waves[i - 1].End)
                    {
                        _waves[i].End = _waves[i - 1].End + 1;
                    }
                }
                else if(i == 0 && _waves[i].End < _waves[i].Start)
                {
                    _waves[i].End = _waves[i].Start;
                }
            }
        }
        
        if (_waves[0].Start > _waves[0].End)
        {
            _waves[0].Start = _waves[0].End;
        }

        for (int i = 1; i < _waves.Length; i++)
        {
            _waves[i].Start = _waves[i - 1].End + 1;
        }
    }
    
    private void EndWaveCheck(int currentEnemyAmount)
    {
        if(_gameDone) return;
        
        if (currentEnemyAmount <= 0 && _activeSpawner <= 0 && CurrentWave < _maxWaveCount )
        {
            GameEvent.WaveDoneEvent?.Invoke(CurrentWave);
            
            return;
        }

        if(CurrentWave < _maxWaveCount || currentEnemyAmount > 0) return;
        
        _gameDone = true;
        GameEvent.GameCompleteEvent?.Invoke();
    }

    public void NextWave()
    {
        if(_gameDone) return;
        
        CurrentWave++;
        StartWave();
    }

    public Wave FindWave(int currentWave)
    {
        for (int i = 0; i < _waves.Length; i++)
        {
            if(_waves[i].End < currentWave) continue;
            
            return _waves[i];
        }

        return default;
    }

    public void DisposeSpawner()
    {
        _activeSpawner--;
    }
    
    private int SteadilyFlatLogic(Wave wave, WaveInfoSO info)
    {
        var distance = CurrentWave - wave.Start;
        var factor = 1;
        
        if (info.EnemyChangeStrategy == EnemyAmountChangeStrategy.DECREASE_STEADILY_FLAT)
        {
            factor = -1;
        }
        
        return info.Amount + info.ChangeFlat * distance * factor;
    }
    
    private int SteadilyPercentLogic(Wave wave, WaveInfoSO info)
    {
        var distance = CurrentWave - wave.Start;
        var factor = 1;
        
        if (info.EnemyChangeStrategy == EnemyAmountChangeStrategy.DECREASE_STEADILY_FLAT)
        {
            factor = -1;
        }
        
        return (int)(info.Amount * (1 + info.ChangePercent * factor * distance));
    }

    private int ChangeWithCurveLogic(Wave wave, WaveInfoSO info)
    {
        var distance = CurrentWave - wave.Start + 1;
        var percent = info.ChangeCurve.Evaluate(distance);
        return (int)(info.Amount * (1 + percent));
    }

    private int ChangeWithSpecified(Wave wave, WaveInfoSO info)
    {
        var distance = CurrentWave - wave.Start;
        if (distance <= 0 || info.SpecifiedChange.Length < distance) return info.Amount;
        return info.SpecifiedChange[distance - 1];
    }
}

[Serializable]
public struct Wave
{
    public int Start;
    public int End;
    public WaveInfoSO[] MainWave;
    public WaveChild[] WaveChild;
}

[Serializable]
public struct WaveChild
{
    public WaveChildSpawnStrategy SpawnStrategy;
    public int EnemyAmount;
    public int MainWaveIndex;
    public WaveInfoSO Info;
}

public enum WaveChildSpawnStrategy
{
    CALL_AFTER_SPECIFIED_MAINWAVE_SPAWN_DONE,
    CALL_AFTER_ENEMY_SPAWN_AMOUNT,
    CALL_AFTER_ENEMY_AMOUNT_DEAD
}

public enum EnemyAmountChangeStrategy
{
    INCREASE_STEADILY_FLAT,
    DECREASE_STEADILY_FLAT,
    INCREASE_STEADILY_PERCENT,
    DECREASE_STEADILY_PERCENT,
    CHANGE_WITH_CURVE,
    CHANGE_WITH_SPECIFIED
}