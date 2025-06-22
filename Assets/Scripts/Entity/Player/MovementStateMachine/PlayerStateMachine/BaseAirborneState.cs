using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ResilientCore
{

	public abstract class BaseAirborneState : BasePlayerState
	{
		public BaseAirborneState(CharacterStateMachine stateMachine, ECharacterState state, string boolName) : base(stateMachine, state, boolName) { }

		#region Abstract Methods
		public override void Enter()
		{
			base.Enter();
			StateMachine.MovementSpeedModifier = 0f;
			StateMachine.IsAirborne = true;
		}

		public override ECharacterState GetNextState()
		{
			if (!StateMachine.IsAirborne)
			{
				StateMachine.ResetMovementVelocity();
				return ECharacterState.Idling;
			}
			return Key;
		}

		public override void Update() 
		{
			if (AirborneCheckGround())
			{
				StateMachine.IsAirborne = false;
			}
			if(CheckStandStill())
			{
				StateMachine.RigidBody.transform.position = StateMachine.LastGroundPosition;
			}
		}
		#endregion

		#region State Methods
		public bool AirborneCheckGround()
		{
			//Nudge when collide with wall
			Vector3 pos = StateMachine.Controller.Collider.bounds.center - new Vector3(0f, StateMachine.Controller.Collider.height / 2f - 0.02f/*have to be higher than the collider*/, 0f);
			if (Physics.SphereCast(pos, StateMachine.Controller.Collider.radius, Vector3.down, out RaycastHit hit, 0.05f, StateMachine.Controller.GroundLayer, QueryTriggerInteraction.Ignore))
			{
				Vector3 force = (new Vector3(pos.x, 0, pos.z) - new Vector3(hit.point.x, 0f, hit.point.z)).normalized;
				StateMachine.RigidBody.velocity = new Vector3(force.x,StateMachine.RigidBody.velocity.y,force.z);
			}
			//Check Grounded
			Ray ray = new Ray(pos, Vector3.down);
			if (Physics.Raycast(ray, 0.3f, StateMachine.Controller.GroundLayer, QueryTriggerInteraction.Ignore))
			{
				return true;
			}
			return false;
		}
		public bool CheckStandStill()
		{
			return StateMachine.RigidBody.velocity.magnitude <= 0;
		}
		#endregion
	}
}
