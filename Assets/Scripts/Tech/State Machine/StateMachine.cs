using System;
using System.Collections;
using System.Collections.Generic;
using Tech.Singleton;
using UnityEngine;

public abstract class StateMachine<EState> : MonoBehaviour where EState : Enum
{
	public Dictionary<EState, BaseState<EState>> States = new Dictionary<EState, BaseState<EState>>();
	public EState NextState { get; private set; }
	public BaseState<EState> CurrentState { get; protected set; }

	protected bool IsTransitioning = false;

	#region Unity Methods

	protected virtual void Update()
	{
		NextState = CurrentState.GetNextState();
		if(NextState.Equals(CurrentState.Key)) 
		{ 
			CurrentState.Update();
		}
		else if(!IsTransitioning)
		{
			TransitionToState(NextState);
		}
	}
	protected virtual void TransitionToState(EState state)
	{
		IsTransitioning = true;
		CurrentState.Exit();
		CurrentState = States[state];
		CurrentState.Enter();
		IsTransitioning = false;
	}

	protected virtual void FixedUpdate()
	{
		CurrentState.FixedUpdate();
	}
	#endregion

}


