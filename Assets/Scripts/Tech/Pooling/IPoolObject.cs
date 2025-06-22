using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolObject
{
    public void Initialize();
    public void OnReturnToPool();
}
