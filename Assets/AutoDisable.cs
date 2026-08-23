using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDisable : MonoBehaviour
{
    [SerializeField]
    private float lifeTime = 5f;

    private void OnEnable()
    {
        CancelInvoke();
        Invoke(nameof(DisableObject), lifeTime);
    }

    private void DisableObject()
    {
        gameObject.SetActive(false);
    }
}