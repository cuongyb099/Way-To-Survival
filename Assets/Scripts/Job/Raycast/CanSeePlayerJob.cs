using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Job/Can See Player")]
public class CanSeePlayerJob : RaycastJobSO
{
    public const string EnemyTag = "Enemy"; 
    public const string PlayerTag = "Player"; 
    
    protected override void OnBatchCompleted(List<JobData> jobData, ref NativeArray<RaycastHit> hits)
    {
        for (int i = 0; i < jobData.Count; i++)
        {
            for (int j = 0; j < MaxHits; j++)
            {
                var index = i * MaxHits + j;

                if (hits[index].collider && !hits[index].collider.CompareTag(EnemyTag) && !hits[index].collider.CompareTag(PlayerTag))
                {
                    ((CanSeePlayerData)jobData[i]).SeePlayer = false;
                    break;
                }
                
                ((CanSeePlayerData)jobData[i]).SeePlayer = true;
            }
        }
    }
}