using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState<EState> where EState : Enum
{
    public EState Key { get; private set; }
	public BaseState(EState key)
    {
        Key = key;
    }
	public abstract void Enter();
    public abstract void Exit();
    public abstract EState GetNextState();
    public abstract void Update();
    public abstract void FixedUpdate();
}
