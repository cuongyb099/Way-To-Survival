using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ZombieRagdollAnimation : RagdollAnimationBase
{
    private Animator _animator;
    private Rigidbody[] _ragdollRigidbodies;
    private Collider[] _collider;
    private const string Player = "Player";
    private const string ZombieBody = "Zombie Body";
    private void Awake()
    {
        LoadComponent();
    }

    private void LoadComponent()
    {
        if(_animator) return;

        _animator = GetComponent<Animator>();
        _ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach (var joint in GetComponentsInChildren<CharacterJoint>())
        {
            joint.enableProjection = true;
        }
        _collider = new Collider[_ragdollRigidbodies.Length];
     
        var layer = LayerMask.GetMask(Player, ZombieBody);
        for (var i = 0; i < _ragdollRigidbodies.Length; i++)
        {
            _ragdollRigidbodies[i].excludeLayers = LayerMask.NameToLayer(Player);
            _collider[i] = _ragdollRigidbodies[i].GetComponent<Collider>();
        }
    }

    public override void DisableRagdoll()
    {
        LoadComponent();
        for (var i = 0; i < _ragdollRigidbodies.Length; i++)
        {
            _collider[i].enabled = false;
            _ragdollRigidbodies[i].isKinematic = true;
        }

        _animator.enabled = true;
    }

    public override void EnableRagdoll()
    {
        LoadComponent();
        for (var i = 0; i < _ragdollRigidbodies.Length; i++)
        {
            _collider[i].enabled = true;
            _ragdollRigidbodies[i].isKinematic = false;
        }
        
        _animator.enabled = false;
    }
}