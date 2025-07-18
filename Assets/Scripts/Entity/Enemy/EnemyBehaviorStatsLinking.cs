using System.Collections;
using BehaviorDesigner.Runtime;
using Cysharp.Threading.Tasks;
using ProjectDawn.Navigation;
using ProjectDawn.Navigation.Hybrid;
using UnityEngine;

public class EnemyBehaviorStatsLinking
{
    private static readonly int ATKSpeed = Animator.StringToHash("Atk Speed");
    private static readonly int MoveSpeed = Animator.StringToHash("Move Speed");
    private static readonly string ATK = "Atk";
    private EnemyCtrl _enemyCtrl;
    private Stat atkSpeedStat;
    private Stat speedStat;
    private Animator animator;
    private AgentAuthoring agent;
    public EnemyBehaviorStatsLinking(EnemyCtrl enemyCtrl)
    {
        _enemyCtrl = enemyCtrl;
        _ = WaitInit();
    }
    private async UniTaskVoid WaitInit()
    {
        await UniTask.DelayFrame(2);
        
        animator = _enemyCtrl.Anim;
        var statsCtrl = _enemyCtrl.Stats;
        agent = _enemyCtrl.Authoring;

        atkSpeedStat = statsCtrl.GetStat(StatType.ATKSpeed);
        speedStat = statsCtrl.GetStat(StatType.Speed);

        //ATK Speed
        animator.SetFloat(ATKSpeed, atkSpeedStat.Value);
        atkSpeedStat.OnValueChange += () =>
        {
            animator.SetFloat(ATKSpeed, atkSpeedStat.Value);
        };
        
        //Move Speed
        speedStat.OnValueChange += SetSpeed;
        SetSpeed();
    }

    private void SetSpeed()
    {
        var locomotion = agent.EntityLocomotion;
        locomotion.Speed = speedStat.Value;
        agent.EntityLocomotion = locomotion;
        animator.SetFloat(MoveSpeed, Mathf.Clamp(speedStat.Value , 0, _enemyCtrl.MaxMoveAnimationSpeed));
    }
}