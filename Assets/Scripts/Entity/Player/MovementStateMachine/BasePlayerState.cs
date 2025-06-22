using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ResilientCore
{
    
    public abstract class BasePlayerState : BaseState<ECharacterState>
    {
		protected string AnimationOverrideName { get; private set; } = null;
		protected int? AnimationID { get; private set; } = null;
        protected CharacterStateMachine StateMachine { get; private set; }
		protected CapsuleCollider FloatingCapsule { get; private set; }
		public BasePlayerState(CharacterStateMachine playerMovementStateMachine,ECharacterState state, string boolName) : base(state)
        {
            StateMachine = playerMovementStateMachine;
			FloatingCapsule = StateMachine.Controller.Collider;
			AnimationOverrideName = boolName;
			AnimationID = Animator.StringToHash(boolName);
		}

		#region Abstract Methods
		public override void Enter() 
		{
			UpdateAnimatorValue(true);
		}

		public override void Exit()
		{
			UpdateAnimatorValue(false);
		}

		public override ECharacterState GetNextState() => Key;

		public override void FixedUpdate() { }

		public override void Update() {  }
		#endregion
		#region State Methods
		private void UpdateAnimatorValue(bool value)
		{
			if (AnimationID == null) return;
			StateMachine.Controller.Animator.SetBool((int)AnimationID, value);
		}
		#endregion
	}
}
