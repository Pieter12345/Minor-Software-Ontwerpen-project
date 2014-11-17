using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class NormalCharacterMotor : CharacterMotor {
	
	public float maxRotationSpeed = 270;
	
	private bool firstframe = true;

	// Update is called once per frame
	void Update () {
		if (Time.deltaTime == 0 || Time.timeScale == 0)
			return;
		
		UpdateDirection();
		UpdateVelocity();
	}

	private void UpdateDirection() {
		// Calculate which way character should be facing
		float facingWeight = desiredFacingDirection.magnitude;
		Vector3 combinedFacingDirection = (
			transform.rotation * desiredMovementDirection * (1-facingWeight)
			+ desiredFacingDirection * facingWeight
		);
		combinedFacingDirection -= Vector3.Project(combinedFacingDirection, transform.up);
		combinedFacingDirection = alignCorrection * combinedFacingDirection;
		
		if (combinedFacingDirection.sqrMagnitude > 0.01f) {
			float value = Mathf.Min(1, maxRotationSpeed*Time.deltaTime / Vector3.Angle(transform.forward, combinedFacingDirection));
			Vector3 newForward = Vector3.Slerp(transform.forward, combinedFacingDirection, value);
			newForward -= Vector3.Project(newForward, transform.up);
			Quaternion q = new Quaternion();
			q.SetLookRotation(newForward, transform.up);
			transform.rotation = q;
		}
	}
	
	private void UpdateVelocity() {
		CharacterController controller = GetComponent(typeof(CharacterController)) as CharacterController;
		Vector3 velocity = controller.velocity;
		if (firstframe) {
			velocity = Vector3.zero;
			firstframe = false;
		}
		if (OnGround) velocity -= Vector3.Project(velocity, transform.up);
		
		// Calculate how fast we should be moving
		Vector3 movement = velocity;
		IsJumping = false;
		if (OnGround) {
			// Apply a force that attempts to reach our target velocity
			Vector3 velocityChange = (desiredVelocity - velocity);
			if (velocityChange.magnitude > maxVelocityChange) {
				velocityChange = velocityChange.normalized * maxVelocityChange;
			}
			movement += velocityChange;
			
			// Jump
			if (canJump && Input.GetButton("Jump")) {
				movement += transform.up * Mathf.Sqrt(2 * jumpHeight * gravity);
				IsJumping = true;
			}
		}
		
		float maxVerticalVelocity = 1.0f;
		if (Mathf.Abs(velocity.y) > maxVerticalVelocity) {
			movement *= Mathf.Max(0.0f, Mathf.Abs(maxVerticalVelocity / velocity.y));
		}
		
		// Apply downwards gravity
		movement += transform.up * -gravity * Time.deltaTime;
		
		if (IsJumping) {
			movement -= transform.up * -gravity * Time.deltaTime / 2;
			
		}
		
		// Apply movement
		CollisionFlags flags = controller.Move(movement * Time.deltaTime);
		OnGround = (flags & CollisionFlags.CollidedBelow) != 0;
	}
	

}
