using DG.Tweening;
using UnityEngine;

public class DespawnObject : MonoBehaviour
{
    [SerializeField] protected float timeToDespawn;
    [SerializeField] protected bool ignoreTimeScale;
    protected Tween despawnTween;
    protected virtual void OnEnable()
    {
        despawnTween = DOVirtual.DelayedCall(timeToDespawn, () =>
        {
            gameObject.SetActive(false);
        })
        .SetUpdate(ignoreTimeScale);
    }
}