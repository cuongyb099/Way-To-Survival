using System.Collections;
using System.Collections.Generic;
using Tech.Pooling;
using UnityEngine;

public class ParticlePooling : MonoBehaviour
{
    public ParticleSystem ParticleSystem { get; private set; }
    private void Awake()
    {
        ParticleSystem = GetComponent<ParticleSystem>();
    }
    private void OnEnable()
    {
        ParticleSystem.Play();
    }
    private void OnDisable()
    {
        ObjectPool.Instance?.ReturnObjectToPool(gameObject);
    }
}
