using System.Collections;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Cysharp.Threading.Tasks;
using Tech.Pooling;
using Unity.VisualScripting;
using UnityEngine;
using ObjectPool = Tech.Pooling.ObjectPool;
using Task = System.Threading.Tasks.Task;

[TaskCategory("AI/GameObject")]
[TaskDescription("Instantiates a new GameObject. Returns Success.")]
public class ZombieExplosion : Action
{
    [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
    public SharedGameObject targetGameObject;
    [BehaviorDesigner.Runtime.Tasks.Tooltip("The position of the new GameObject")]
    public SharedVector3 position;
    [BehaviorDesigner.Runtime.Tasks.Tooltip("The rotation of the new GameObject")]
    public SharedQuaternion rotation = Quaternion.identity;
    [SharedRequired]
    [BehaviorDesigner.Runtime.Tasks.Tooltip("The instantiated GameObject")]
    public AudioClip audioClip;
    public SharedSoundSetting SoundSetting;
    public SharedFloat ATK;
    public float RadiusExplosion = 2f;
    private Collider[] colliders = new Collider[10];
    public SharedLayerMask LayerMask;
    public const string PlayerTag = "Player";
    public override TaskStatus OnUpdate()
    {
        _ = SpawnPrefab();
        return TaskStatus.Success;
    }
    
    public static WaitForSeconds wait = new WaitForSeconds(0.1f);
    
    private async UniTaskVoid SpawnPrefab()
    {
        await UniTask.Delay(100);
        var clone = ObjectPool.Instance.SpawnObject(targetGameObject.Value, position.Value, rotation.Value, PoolType.ParticleSystem);;
        AudioManager.Instance.PlaySound(audioClip, SoundSetting.Value, clone.transform);
        int count = Physics.OverlapSphereNonAlloc(position.Value, RadiusExplosion, colliders, LayerMask.Value);

        for (int i = 0; i < count; i++)
        {
            var go = colliders[i].gameObject;
            if (colliders[i].gameObject.CompareTag(PlayerTag))
            {
                if (go.TryGetComponent(out IDamagable damagable))
                {
                    damagable.Damage(new DamageInfo(null, ATK.Value * 2f, false, DamageType.Melee));
                }
                return;
            }
        }
    }

    public override void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(position.Value, RadiusExplosion);
    }
}
