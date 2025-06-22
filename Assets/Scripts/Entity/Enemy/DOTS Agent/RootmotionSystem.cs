using ProjectDawn.Navigation;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
[RequireMatchingQueriesForUpdate]
[UpdateInGroup(typeof(AgentLocomotionSystemGroup))]
public partial struct RootmotionSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        new RootmotionJob
        {
            DeltaTime = state.WorldUnmanaged.Time.DeltaTime
        }.ScheduleParallel();
    }

    [BurstCompile]
    partial struct RootmotionJob : IJobEntity
    {
        public float DeltaTime;

        public void Execute(ref LocalTransform transform, ref AgentBody body, ref AgentRootmotion agent, in AgentShape shape)
        {
            if (body.IsStopped)
                return;

            // Check, if we reached the destination
            float remainingDistance = body.RemainingDistance;
            if (remainingDistance <= agent.StoppingDistance + 1e-3f)
            {
                body.Velocity = 0;
                body.IsStopped = true;
                return;
            }

            float maxSpeed = agent.Speed;

            // Start breaking if close to destination
            if (agent.AutoBreaking)
            {
                float breakDistance = shape.Radius * 2 + agent.StoppingDistance;
                if (remainingDistance <= breakDistance)
                {
                    maxSpeed = math.lerp(agent.Speed * 0.25f, agent.Speed, remainingDistance / breakDistance);
                }
            }

            // Force force to be maximum of unit length, but can be less
            float forceLength = math.length(body.Force);
            if (forceLength > 1)
                body.Force = body.Force / forceLength;

            // Update rotation
            if (shape.Type == ShapeType.Circle)
            {
                float angle = math.atan2(body.Velocity.x, body.Velocity.y);
                transform.Rotation = math.slerp(transform.Rotation, quaternion.RotateZ(-angle), DeltaTime * agent.AngularSpeed);
            }
            else if (shape.Type == ShapeType.Cylinder)
            {
                float angle = math.atan2(body.Velocity.x, body.Velocity.z);
                transform.Rotation = math.slerp(transform.Rotation, quaternion.RotateY(angle), DeltaTime * agent.AngularSpeed);
            }

            // Tank should only move, if facing direction and movement direction is within certain degrees
            float3 direction = math.normalizesafe(body.Velocity);
            float3 facing = math.mul(transform.Rotation, new float3(1, 0, 0));
            if (math.dot(direction, facing) > math.radians(10))
            {
                maxSpeed = 0;
            }

            // Interpolate velocity
            body.Velocity = math.lerp(body.Velocity, body.Force * maxSpeed, DeltaTime * agent.Acceleration);

            float speed = math.length(body.Velocity);

            // Early out if steps is going to be very small
            if (speed < 1e-3f)
                return;

            float3 finalPosition;
            
            // Avoid over-stepping the destination
            if (speed * DeltaTime > remainingDistance)
            {
                finalPosition = transform.Position + (body.Velocity / speed) * remainingDistance;
                finalPosition = new float3(transform.Position.x, finalPosition.y, transform.Position.z);
                transform.Position = finalPosition;
                return;
            }

            // Update position
            finalPosition = transform.Position + DeltaTime * body.Velocity;
            finalPosition = new float3(transform.Position.x, finalPosition.y, transform.Position.z);
            transform.Position = finalPosition;
        }
    }
}
