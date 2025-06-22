using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

namespace ResilientCore
{
    public class PlayerFallingState : BaseAirborneState
	{
		public PlayerFallingState(CharacterStateMachine stateMachine, string boolName) : base(stateMachine, ECharacterState.Falling, boolName) { }
		
		public override void Enter()
		{
			base.Enter();
		}
		public override void Exit()
		{
			base.Exit();
		}
		public override void Update()
		{
			base.Update();
		}
		public override ECharacterState GetNextState()
		{
			if (!StateMachine.IsAirborne)
			{
				return ECharacterState.Idling;
			}
			return Key;
		}
	}
}
