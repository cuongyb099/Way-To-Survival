using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Utilities")]
public class ReinitStats : Action
{
    public override TaskStatus OnUpdate()
    {
        StatsController stats = GetComponent<StatsController>();
        stats.ReInit();
        StatModifier statModifier =
            new StatModifier(40f * EnemyManager.Instance.GetCurrentWave(), StatModType.Percentage);
        stats.GetStat(StatType.MaxHP).AddModifier(statModifier);
        stats.GetAttribute(AttributeType.Hp).SetValueToMax();
        
        return TaskStatus.Success;
    }
}