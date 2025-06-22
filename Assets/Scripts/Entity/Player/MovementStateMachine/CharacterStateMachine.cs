using Cinemachine;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ResilientCore
{
    public enum ECharacterState
    {
        Idling,
        Walking,
        Running,
		Falling,
		Shooting,
    }
	public class CharacterStateMachine : StateMachine<ECharacterState>
	{
		public PlayerController Controller { get; private set; }
		public Rigidbody RigidBody { get; private set; }
		//***Character Movement Data***
		public Vector3 LastGroundPosition;
		public float MovementSpeedModifier { get; set; }
		public float RotateDampTargetAngle { get; private set; }
		public float RotateDampSmooth { get; private set; }
		public bool IsAirborne { get; set; }
		public bool IsGrounded { get; set; }

		private float rotateDampCurrentVelocity;
		//Vault Hit Result
		public RaycastHit HitResult;

		//Time to trigger
		[field: Header("Time Data")]
		[field: SerializeField] public float HardfallTime { get; private set; } = 0.1f;
		[field: SerializeField] public float BlockCooldownTime { get; private set; } = 2f;
		[field: SerializeField] public float RollCooldownTime { get; private set; } = 2f;


		#region Unity Methods
		protected void Awake()
		{
			Controller = GetComponent<PlayerController>();		}
		protected void Start()
		{
			States.Add(ECharacterState.Idling, new PlayerIdlingState(this, "IsIdling"));
			States.Add(ECharacterState.Walking, new PlayerWalkingState(this, "IsWalking"));
			States.Add(ECharacterState.Running, new PlayerRunningState(this, "IsRunning"));

			States.Add(ECharacterState.Falling, new PlayerFallingState(this, "IsFalling"));

			CurrentState = States[ECharacterState.Idling];
			RotateDampSmooth = 0.02f;
		}
		protected override void Update()
		{
			base.Update();
			SetRunningAnimFloat();
		}
		#endregion

		#region Private Methods
		private float AddCameraDirectionToAngle(float angle)
		{
			angle += Camera.main.transform.rotation.eulerAngles.y;

			if (angle > 360)
			{
				angle -= 360;
			}

			return angle;
		}

		#endregion

		#region Reusable Methods
		//Get the Angle from direction with 0 to 360 clamp
		public float GetDirectionAngle(Vector3 _direction)
		{
			float directionAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg;

			if (directionAngle < 0)
			{
				directionAngle += 360;
			}

			return directionAngle;
		}
		//Get the input movement vector with wall checking
		public Vector3 GetMovementDirection()
		{
			Vector3 movementDirection = new Vector3(PlayerInput.Instance.MovementInput.x, 0f, PlayerInput.Instance.MovementInput.y);

			SetTargetRunningAnimFloat(movementDirection.x, movementDirection.z);

			float targetAngle = GetTargetAngle(movementDirection);

			Vector3 targetRotationDirection = GetTargetRotationVector(targetAngle);

			Vector3 checkWallpos = Controller.Collider.bounds.center;
			//Raycast wall ahead
			if (Physics.Raycast(checkWallpos, targetRotationDirection, out RaycastHit hit, 0.2f, LayerMask.GetMask("Default")))
			{
				float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
				if(slopeAngle > 65f) 
					return Vector3.zero;
			}
			return movementDirection;
		}
		public float TargetRunningX, TargetRunningY;
		public void SetTargetRunningAnimFloat(float x, float y)
		{
			if (!Controller)
			{
				TargetRunningX = 0; 
				TargetRunningY = 1;
				return;
			}
			TargetRunningX = x;
			TargetRunningY = y;

		}
		public float CurrentRunningX, CurrentRunningY;
		public void SetRunningAnimFloat()
		{
			CurrentRunningX = Mathf.Lerp(CurrentRunningX, TargetRunningX, 0.1f);
			CurrentRunningY = Mathf.Lerp(CurrentRunningY, TargetRunningY, 0.1f);
			Controller.Animator.SetFloat("ValX", CurrentRunningX);
			Controller.Animator.SetFloat("ValY", CurrentRunningY);
		}

		public float GetMovementSpeed()
		{
			return 1 * MovementSpeedModifier;
		}

		public Vector3 GetCurrentHorizontalVelocity()
		{
			Vector3 playerHorizontalVelocity = RigidBody.velocity;

			playerHorizontalVelocity.y = 0f;

			return playerHorizontalVelocity;
		}

		public Vector3 GetTargetRotationVector(float targetAngle)
		{
			return Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
		}

		//Smooth rotation
		public void RotateTowardsTargetAngle()
		{
			float currentYAngle = RigidBody.rotation.eulerAngles.y;

			if (currentYAngle == RotateDampTargetAngle)
			{
				return;
			}

			float smoothDampAngle = Mathf.SmoothDampAngle(currentYAngle, RotateDampTargetAngle, ref rotateDampCurrentVelocity, RotateDampSmooth);
			Quaternion targetRotation = Quaternion.Euler(0, smoothDampAngle, 0);
			RigidBody.MoveRotation(targetRotation);
		}
		//Get the target angle and add camera rotation if needed
		public float GetTargetAngle(Vector3 _direction, bool considerCameraRotation = true)
		{
			float directionAngle = GetDirectionAngle(_direction);
			if (considerCameraRotation)
			{
				directionAngle = AddCameraDirectionToAngle(directionAngle);
			}
			return directionAngle;
		}
		//Update Smooth target angle
		public void UpdateSmoothRotateTargetAngle(float directionAngle)
		{
			if (directionAngle != RotateDampTargetAngle)
			{
				RotateDampTargetAngle = directionAngle;
			}
		}
		public void ResetMovementVelocity()
		{
			RigidBody.velocity = Vector3.zero;
		}
		public void ResetYVelocity()
		{
			RigidBody.velocity = new Vector3(RigidBody.velocity.x,0f, RigidBody.velocity.z);
		}
		public float GetVerticalVelocity()
		{
			return RigidBody.velocity.y;
		}

		#endregion

		#region Animator methods

		#endregion

		#region IEnumerators
		#endregion
	}
}
