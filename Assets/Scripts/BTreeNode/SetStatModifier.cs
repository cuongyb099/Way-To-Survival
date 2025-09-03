using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Stats")]
public class SetStatModifier : Action
{
    public StatType Stat;
    public StatModifier modifier;
    protected StatsController stats;
    public override void OnAwake()
    {
        stats = GetComponent<StatsController>();
    }

    public override TaskStatus OnUpdate()
    {
        stats.AddModifier(Stat, modifier);
        return base.OnUpdate();
    }
}