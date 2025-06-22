using UnityEngine;

namespace ResilientCore
{
    public class PlayerIdlingState : BaseGroundedActionState
    {
		public PlayerIdlingState(CharacterStateMachine StateMachine, string boolName) : base(StateMachine, ECharacterState.Idling, boolName) { }

        public override void Enter()
        {
            base.Enter();
            StateMachine.MovementSpeedModifier = 0;
			StateMachine.RigidBody.drag = 5f;
		}
		public override void Exit()
		{
			base.Exit();
			StateMachine.RigidBody.drag = 1f;
		}
		public override ECharacterState GetNextState()
		{
			ECharacterState state = base.GetNextState();
			if(state != Key)
			{
				return state;
			}
			if (StateMachine.GetMovementDirection() == Vector3.zero)
			{
				return Key;
			}
			if (PlayerInput.Instance.IsWalkInput)
			{
				return ECharacterState.Walking;
			}
            return ECharacterState.Running;
		}
		public override void FixedUpdate()
		{
			base.FixedUpdate();
		}
    }
}
