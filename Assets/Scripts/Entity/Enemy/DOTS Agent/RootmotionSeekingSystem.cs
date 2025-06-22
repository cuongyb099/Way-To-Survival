using ProjectDawn.Navigation;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
[RequireMatchingQueriesForUpdate]
[UpdateInGroup(typeof(AgentSeekingSystemGroup))]
public partial struct RootmotionSeekingSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {

    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {

    }
    
    [BurstCompile]
    partial struct RootmotionSeekingJob : IJobEntity
    {
        public void Execute(ref AgentBody body, in AgentRootmotion agentRootmotion, in LocalTransform transform)
        {
            if (body.IsStopped)
                return;
            
            float3 tmp = transform.Position;
            tmp.y = body.Destination.y;
            
            float3 towards = body.Destination - transform.Position;
            float distance = math.length(towards);
            float3 desiredDirection = distance > math.EPSILON ? towards / distance : float3.zero;
            body.Force = desiredDirection;
            body.RemainingDistance = distance;
        }
    }
}
