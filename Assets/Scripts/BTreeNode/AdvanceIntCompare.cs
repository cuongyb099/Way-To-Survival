using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Unity.Math;

[TaskCategory("Utilities")]
public class AdvanceIntCompare : Conditional
{
    [Tooltip("The first variable to compare")]
    public SharedInt variable;
    [Tooltip("The variable to compare to")]
    public SharedInt compareTo;
    public FloatComparison.Operation CompareType;
    
    public override TaskStatus OnUpdate()
    {
        switch (CompareType)
        {
            case FloatComparison.Operation.LessThan:
                return variable.Value < compareTo.Value ? TaskStatus.Success : TaskStatus.Failure;
            case FloatComparison.Operation.LessThanOrEqualTo:
                return variable.Value <= compareTo.Value ? TaskStatus.Success : TaskStatus.Failure;
            case FloatComparison.Operation.EqualTo:
                return variable.Value.Equals(compareTo.Value) ? TaskStatus.Success : TaskStatus.Failure;
            case FloatComparison.Operation.NotEqualTo:
                return !variable.Value.Equals(compareTo.Value) ? TaskStatus.Success : TaskStatus.Failure;
            case FloatComparison.Operation.GreaterThanOrEqualTo:
                return variable.Value >= compareTo.Value ? TaskStatus.Success : TaskStatus.Failure;
            case FloatComparison.Operation.GreaterThan:
                return variable.Value > compareTo.Value ? TaskStatus.Success : TaskStatus.Failure;
        }

        return TaskStatus.Failure;
    }

    public override void OnReset()
    {
        CompareType = FloatComparison.Operation.LessThan;
        variable = 0;
        compareTo = 0;
    }
}

