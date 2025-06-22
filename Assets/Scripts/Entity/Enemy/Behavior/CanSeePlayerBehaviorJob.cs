using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class CanSeePlayerBehaviorJob : Conditional
{
    public SharedTransform PlayerTransform;
    public CanSeePlayerData Data;
    public override void OnAwake()
    {
        base.OnAwake();
        EntitiesJobManager.Instance.Add(Data);
    }

    public override TaskStatus OnUpdate()
    {
        Data.Distance = Vector3.Distance(transform.position, PlayerTransform.Value.position);
        Data.Direction = (PlayerTransform.Value.position - transform.position).normalized;
        Data.OriginPoint = transform.position + Vector3.up;
        
        return !Data.SeePlayer ? TaskStatus.Failure : TaskStatus.Success;
    }
}