///Package Create By Kat
using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Not Stackable Is Clone Effect
/// Stackable Is Reference Effect 
/// </summary>
public abstract class BaseStatusEffect :IEquatable<BaseStatusEffect>
{
    public Action OnStart, OnEnd, OnActive;
    protected float timer;
    protected StatsController stats;
    public bool ForceStop { get; protected set; }
    public StatusEffectSO Data;
    public int CurrentStack
    {
        get => currentStack;
        set
        {
            currentStack = value;
            HandleStackChange();
        } 
    }
    private int currentStack;
    protected BaseStatusEffect(StatusEffectSO data, StatsController target,
        Action OnStart = null, Action onEnd = null, Action onActive = null)
    {
        OnInit(data, target);
        
        this.OnStart = OnStart;
        this.OnEnd = onEnd;
        this.OnActive = onActive;
    }
    
    protected BaseStatusEffect(){}

    /// <summary>
    /// This Function To Active Effect
    /// </summary>
    public void Begin()
    {
        timer = 0f;
        ForceStop = false;
        currentStack = 0;
        HandleStart();
        OnStart?.Invoke();
        if (Data.UseAdvanceUpdate)
        {
            _ = UpdateAsync();
        }
    }
    
    protected virtual void OnInit(StatusEffectSO data, StatsController target)
    {
        ForceStop = false;
        timer = 0f;
        Data = data;
        stats = target;
        currentStack = 0;
    }
    
    /// <summary>
    /// This MonoUpdate Work With Effect Stack Same Timer  
    /// </summary>
    public virtual bool MonoUpdate()
    {
        if (!Data.UseAdvanceUpdate)
        {
            HandleOnUpdate();
            OnActive?.Invoke();
        }
        if (!Data.HasDuration) return false;
        timer += Time.deltaTime;
        return timer > Data.Duration;
    }

    private async UniTaskVoid UpdateAsync()
    {
        while (!ForceStop)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) break;
#endif
            HandleOnUpdate();
            OnActive?.Invoke();
            await UniTask.Yield(PlayerLoopTiming.Update);
        }
    }

    public void Stop()
    {
        ForceStop = true;
        HandleEnd();
        OnEnd?.Invoke();
    }

    
    /// <summary>
    /// Clone Must Override In Other Effect
    /// </summary>
    public virtual BaseStatusEffect Clone()
    {
        return (BaseStatusEffect)this.MemberwiseClone();
    }
    
    /// <summary>
    /// HandleStackChange Not Apply When Begin
    /// </summary>
    public abstract void HandleStackChange();
    protected abstract void HandleStart();
    protected abstract void HandleOnUpdate();
    protected abstract void HandleEnd();

    public bool Equals(BaseStatusEffect other)
    {
        return (Data.ID == other.Data.ID) && timer.Equals(other.timer);
    }
	public virtual void ChangeTarget(StatsController target)
	{
		stats = target;
	}
}

