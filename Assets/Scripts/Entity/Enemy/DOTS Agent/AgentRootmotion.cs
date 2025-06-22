using Unity.Entities;
using Unity.Mathematics;

public struct AgentRootmotion : IComponentData
{
    public float Speed;
    public float Acceleration;
    public float AngularSpeed;
    public float StoppingDistance;
    public bool AutoBreaking;
}