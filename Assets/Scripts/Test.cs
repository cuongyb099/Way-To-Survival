using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] ThrowObject prefab;
    public float angle = 45f;
    public Transform obj2;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            var clone = Instantiate(prefab, this.transform.position, Quaternion.identity);
            clone.ThrowProjectionMotion(angle, transform.position, obj2.position);
        }
    }
}
