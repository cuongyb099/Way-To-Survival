using UnityEngine;

namespace ResilientCore
{
    
    public abstract class BaseGroundedState : BasePlayerState
    {
		public BaseGroundedState(CharacterStateMachine stateMachine, ECharacterState state, string boolName) : base(stateMachine, state, boolName) { }

		#region Abstract Methods
		public override void Enter()
		{
			base.Enter();
			StateMachine.IsGrounded = true;
		}

		public override ECharacterState GetNextState()
		{
			//Check grounded
			if (!StateMachine.IsGrounded)
			{
				return ECharacterState.Falling;
			}
			return Key;

		}

		public override void FixedUpdate() 
		{
			//Float();
			if(StateMachine.IsGrounded) StateMachine.LastGroundPosition = StateMachine.transform.position;
		}
		#endregion

		#region State Methods
		public void Move()
		{
			Vector3 movementDirection = StateMachine.GetMovementDirection();

			//Rotate
			float targetAngle = StateMachine.GetTargetAngle(movementDirection);
			StateMachine.UpdateSmoothRotateTargetAngle(targetAngle);
			StateMachine.RotateTowardsTargetAngle();

			Vector3 targetRotationDirection = StateMachine.GetTargetRotationVector(targetAngle);


			Vector3 checkWallpos = StateMachine.Controller.Collider.bounds.center - new Vector3(0, StateMachine.Controller.Collider.height / 2f, 0);
			if(Physics.Raycast(checkWallpos, targetRotationDirection, 0.4f, StateMachine.Controller.GroundLayer))
			{
				return;
			}

			Vector3 currentPlayerHorizontalVelocity = StateMachine.GetCurrentHorizontalVelocity();

			float movementSpeed = StateMachine.GetMovementSpeed();
			
			StateMachine.RigidBody.AddForce(targetRotationDirection * movementSpeed - currentPlayerHorizontalVelocity, ForceMode.VelocityChange);
		}

		//public void Float()
		//{
		//	Ray ray = new Ray(FloatingCapsule.CapsuleColliderData.Collider.bounds.center,Vector3.down);
		//	if (Physics.Raycast(ray, out RaycastHit hit, FloatingCapsule.FloatingData.FloatRayLength, StateMachine.Controller.DropinData.LayerData.GroundLayer, QueryTriggerInteraction.Ignore))
		//	{
		//		float distanceFromGround = FloatingCapsule.CapsuleColliderData.ColliderCenterLocalSpace.y * StateMachine.Controller.transform.localScale.y - hit.distance;
		//		if (distanceFromGround == 0)
		//		{
		//			return;
		//		}
		//		float liftAmount = distanceFromGround * FloatingCapsule.FloatingData.StepHeightMultiplier - StateMachine.GetVerticalVelocity();
		//		StateMachine.RigidBody.AddForce(new Vector3(0, liftAmount,0),ForceMode.VelocityChange);
		//		return;
		//	}
		//	StateMachine.IsGrounded = false;
		//}
		public void Rotate()
		{
			Vector3 movementDirection = new Vector3(PlayerInput.Instance.MovementInput.x, 0, PlayerInput.Instance.MovementInput.y);

			if (movementDirection == Vector3.zero)
			{
				return;
			}

			float targetAngle = StateMachine.GetTargetAngle(movementDirection);
			StateMachine.UpdateSmoothRotateTargetAngle(targetAngle);
			Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
			StateMachine.Controller.transform.rotation = targetRotation;
		}
		#endregion
	}
}
