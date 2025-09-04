using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Core.Job_System.JobLogicHandle
{
    [CreateAssetMenu(menuName = "SO/Jobs/OverlapSphereJobHandle")]
    public class OverlapSphereJobHandleSO : JobLogicHandleSO<OverlapSphereData>
    {
        [SerializeField] protected int maxHit = 5;
        [SerializeField] protected int minCommandPerJob = 3;
        
        public AYellowpaper.SerializedCollections.SerializedDictionary<int, List<Collider>> resultRef = new();

        public override void AddJobData(OverlapSphereData jobData)
        {
            base.AddJobData(jobData);
            resultRef[jobData.InstanceID] = null;
        }

        public List<Collider> GetResult(int instanceID) => resultRef.GetValueOrDefault(instanceID);
        
        protected override void OnDisable()
        {
            base.OnDisable();
            resultRef?.Clear();
        }

        public override void HandleJobs()
        {
            if(!this.jobDatas.IsCreated) return;
            
            int lenght = this.jobDatas.Length;
            
            var commands = new NativeArray<OverlapSphereCommand>(lenght, Allocator.TempJob);
            var results = new NativeArray<ColliderHit>(lenght * maxHit, Allocator.TempJob);
            
            CreateOverlapSphereJob createJob = new CreateOverlapSphereJob()
            {
                JobDatas = this.jobDatas,
                Commands = commands,
            };
            createJob.Schedule(lenght, minCommandPerJob).Complete();
            
            OverlapSphereCommand.ScheduleBatch(commands, results, minCommandPerJob, maxHit).Complete();
            
            for (int i = 0; i < lenght; i++)
            {
                var hitTargets = KatLib.Pooling.GenericPool<List<Collider>>.Get();
                var data = jobDatas[i];
                hitTargets.Clear();
                
                for (int j = 0; j < this.maxHit; j++)
                {
                    var index = i * this.maxHit + j;
                        
                    var hitCollider = results[index].collider;
                    if (!hitCollider) continue;
                
                    hitTargets.Add(hitCollider);
                }

                if (resultRef.TryAdd(data.InstanceID, hitTargets)) continue;
                
                resultRef[data.InstanceID] = hitTargets;
            }

            
            this.jobDatas.Dispose();
            commands.Dispose();
            results.Dispose();
        }
    }

    [BurstCompile]
    public struct CreateOverlapSphereJob : IJobParallelFor
    {
        [ReadOnly]
        public NativeList<OverlapSphereData> JobDatas;
        public NativeArray<OverlapSphereCommand> Commands;
        
        public void Execute(int index)
        {
            var data = JobDatas[index];
            Commands[index] = new OverlapSphereCommand()
            {
                point = data.Point,
                radius = data.Radius,
                queryParameters = new QueryParameters()
                {
                    layerMask = data.Layer
                }
            };
        }
    }
    
    [System.Serializable]
    public struct OverlapSphereData
    {
        public Vector3 Point;
        public float Radius;
        public LayerMask Layer;
        public int InstanceID;
    }
}