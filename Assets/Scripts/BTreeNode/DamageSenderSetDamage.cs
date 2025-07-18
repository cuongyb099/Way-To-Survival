using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using KatLib.Serialized_Type;
using UnityEngine;

namespace BTreeNode
{
    [TaskCategory("Damage")]
    public class DamageSenderSetDamage : Action
    {
        public SharedFloat DamageScaling;
        protected IDamageDealer damageDealer;
        protected StatsController stats;
        public StringSO DetectionName;
        protected Stat atkStat; 
        
        public override void OnAwake()
        {
            base.OnAwake();
            var colliderDetection = this.transform
                .GetComponentInChildren<ColliderDetectionCtrl>()
                .GetDetection(DetectionName);

            stats = GetComponent<StatsController>();
            atkStat = stats.GetStat(StatType.ATK);
            damageDealer = colliderDetection as IDamageDealer;
        }

        public override TaskStatus OnUpdate()
        {
            damageDealer?.SetDamage(new DamageInfo()
            {
                Damage = atkStat.Value * DamageScaling.Value,
                Dealer = this.gameObject
            });
            return TaskStatus.Success;
        }
    }
}