using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Utilities")]
public class FloatRound : Action
{
    public SharedFloat Value;
    public SharedFloat StoredValue;
    
    public override TaskStatus OnUpdate()
    {
        StoredValue.Value = Mathf.RoundToInt(Value.Value);
        return TaskStatus.Success;
    }
}