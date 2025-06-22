using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CompoundCollider))]
public class PoisonArea : MonoBehaviour
{
    private CompoundCollider _collider;
    [SerializeField] private StatusEffectSO effectData;
    private PoisonEffect effect;
    private Dictionary<Collider, Tween> targets = new ();
    public float Damage = 8f;
    private void Awake()
    {
        _collider = GetComponent<CompoundCollider>();
        effect = new PoisonEffect(effectData, null, Damage);
        _collider.OnEnter += ApplyPoisonEffect;
        _collider.OnExit += StopApplyEffect;
    }

    private void ApplyPoisonEffect(Collider target)
    {
        if(!target.TryGetComponent(out StatsController stats)) return;
        
        DealEffect(stats);
        
        var tween = DOVirtual.DelayedCall(effectData.Duration + 0.01f, () => {
            DealEffect(stats);
            
        }).SetLoops(-1);

        targets.Add(target, tween);
    }

    private void StopApplyEffect(Collider target)
    {
        targets[target].Kill();
        targets.Remove(target);
    }

    private void DealEffect(StatsController stats)
    {
        effect.ChangeTarget(stats);
        stats.ApplyEffect(effect);
        BlindPlayer.Instance.Blind(effectData.Duration +1f);
    }
}
