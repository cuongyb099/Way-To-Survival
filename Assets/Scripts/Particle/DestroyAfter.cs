using System;
using System.Collections;
using System.Collections.Generic;
using Tech.Pooling;
using UnityEngine;

public class DestroyPropAfter : MonoBehaviour,IPoolObject
{
    public float DestroyAfterTime = 1f;

    public void Initialize()
    {
        StartCoroutine(DestroyAfter());
    }
    public void OnReturnToPool()
    {
        
    }
    private IEnumerator DestroyAfter()
    {
        yield return new WaitForSeconds(DestroyAfterTime);
        ObjectPool.Instance.ReturnObjectToPool(gameObject);
    }
    
}
