using DG.Tweening;
using Tech.Pooling;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour,IPoolable
{
	[field:SerializeField] public int DamageTime { get; private set; } = 1;
	[field:SerializeField,Range(0f,1f)] public float DamageReduction { get; private set; }= 1f;
	public float LiveTime = 3f;
	public float Force = 10f;
	public GameObject HitEffectWall;
	private DamageInfo damageInfo;
	private Rigidbody rb;
	private TrailRenderer trailRenderer;
	private Tween seq;
	private int countDMG = 0;
	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
		trailRenderer = GetComponent<TrailRenderer>();
	}
	public void OnEnable()
	{
		seq = DOVirtual.DelayedCall(LiveTime, Free).SetUpdate(false);
	}
	
	private void OnCollisionEnter(Collision other)
	{
		ObjectPool.Instance.SpawnObject(HitEffectWall, other.contacts[0].point, Quaternion.identity, PoolType.ParticleSystem);
		seq.Kill();
		Collider otherCollider = other.collider;
		if (!otherCollider.CompareTag(damageInfo.Dealer.tag))
		{
			if (otherCollider.TryGetComponent(out IDamagable damagable))
			{
				DamageHandler.Damage(damagable, damageInfo);
				
				countDMG--;
				damageInfo = new DamageInfo(damageInfo.Dealer,damageInfo.Damage*DamageReduction,damageInfo.IsCrit);
				HandleBulletPenetration(other,countDMG);
				return;
			}
		}
		Free();
	}

	private void HandleBulletPenetration(Collision collision, int countPenetrate)
	{
		if (collision != null &&
		    countPenetrate > 0)
		{
			Vector3 direction = spawnVelocity.normalized;
			ContactPoint contact = collision.GetContact(0);
			Vector3 backCastOrigin = contact.point + direction * 1f;

			if (Physics.Raycast(
				    backCastOrigin,
				    -direction,
				    out RaycastHit hit,
				    1f,
				    rb.includeLayers
			    ))
			{
				rb.position = hit.point + direction * 0.01f;

				rb.velocity = spawnVelocity - direction;

			}
			else
			{
				Free();
			}
		}
		else
		{
			Free();
		}
	}
	private Vector3 spawnVelocity ;
	public void InitBullet(Vector3 point, float accuracy, DamageInfo info)
	{
		damageInfo = info;
		rb.position = point;
		countDMG = DamageTime;
		Vector3 angle = info.Dealer.gameObject.transform.rotation.eulerAngles;

		Quaternion temp = Quaternion.Euler(angle.x, angle.y + Mathf.Clamp(Random.Range(-accuracy, accuracy), 
			-GameValues.RecoilMaxValue, GameValues.RecoilMaxValue), angle.z);
		trailRenderer.Clear();
		spawnVelocity = temp * Vector3.forward * Force;
		rb.AddForce(spawnVelocity, ForceMode.VelocityChange);
	}
	public void Free()
	{
		rb.velocity = Vector3.zero;
		
		trailRenderer.Clear();
		ObjectPool.Instance.ReturnObjectToPool(gameObject);
	}

	public void New()
	{
		
	}
	
}
