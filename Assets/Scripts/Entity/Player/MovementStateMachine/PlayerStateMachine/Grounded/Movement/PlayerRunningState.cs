using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ResilientCore
{
	public class PlayerRunningState : BaseGroundedActionState
	{
		public PlayerRunningState(CharacterStateMachine StateMachine, string boolName) : base(StateMachine, ECharacterState.Running, boolName) { }
		
		public override void Enter()
        {
            base.Enter();

            //StateMachine.MovementSpeedModifier = StateMachine.Controller.MovementData.MovementRunModifier;
        }
		public override void Exit()
		{
			base.Exit();
		}

		public override ECharacterState GetNextState()
		{
			ECharacterState state = base.GetNextState();
			if (state != Key)
			{
				return state;
			}
			if (StateMachine.GetMovementDirection() == Vector3.zero)
			{
				StateMachine.ResetMovementVelocity();
				return ECharacterState.Idling;
			}
			if (PlayerInput.Instance.IsWalkInput)
			{
				return ECharacterState.Walking;
			}
			return Key;
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			Move();
		}
	}
}
