using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ResilientCore
{
    public class PlayerWalkingState : BaseGroundedState
	{
		public PlayerWalkingState(CharacterStateMachine stateMachine, string boolName) : base(stateMachine, ECharacterState.Walking, boolName) { }
		public override void Enter()
		{
			base.Enter();
		}

		public override void Exit()
		{
			base.Exit();
			StateMachine.ResetMovementVelocity();
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
				return ECharacterState.Idling;
			}
			if (!PlayerInput.Instance.IsWalkInput)
			{
				return ECharacterState.Running;
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
