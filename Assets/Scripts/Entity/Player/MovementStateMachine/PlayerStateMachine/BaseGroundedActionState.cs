using System.Collections;
using UnityEngine;

namespace ResilientCore
{
	public class BaseGroundedActionState : BaseGroundedState
	{
		public BaseGroundedActionState(CharacterStateMachine stateMachine, ECharacterState state, string boolName) : base(stateMachine, state, boolName) { }

		public override ECharacterState GetNextState()
		{
			ECharacterState state = base.GetNextState();
			if (state != Key)
			{
				return state;
			}

			if (PlayerInput.Instance.IsAttackInput)
			{
				return ECharacterState.Shooting;
			}
			return Key;
		}
	}
}