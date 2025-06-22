using System;
using System.Collections;
using Tech.Pooling;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioChild : MonoBehaviour
{
    public AudioSource Source {get; private set;}
    private Coroutine _coroutine;
    
    private void Awake()
    {
        Source = GetComponent<AudioSource>();
    }

    /*public void WaitToReturnPool()
    {
        _coroutine = StartCoroutine(ReturnPool());
    }*/

    private void Update()
    {
        CheckingPlaying();
    }

    private void CheckingPlaying()
    {
        if(Source.isPlaying) return;
        Transform parentRoot = ObjectPool.Instance.GetParentObject(PoolType.Audio).transform;
        if (parentRoot != transform.parent)
        {
            transform.SetParent(parentRoot);
        }
        ObjectPool.Instance.ReturnObjectToPool(gameObject);        
    }

    /*private IEnumerator ReturnPool()
    {
        yield return new WaitUntil (() => !Source.isPlaying);
        Transform parentRoot = ObjectPool.Instance.GetParentObject(PoolType.Audio).transform;
        if (parentRoot != transform.parent)
        {
            transform.SetParent(parentRoot);
        }
        ObjectPool.Instance.ReturnObjectToPool(gameObject);
    }*/
}
