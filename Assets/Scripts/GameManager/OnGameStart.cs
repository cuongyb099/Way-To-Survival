using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGameStart : MonoBehaviour
{
    void Start()
    {
        GameEvent.OnInitializedUI?.Invoke();
    }
}
