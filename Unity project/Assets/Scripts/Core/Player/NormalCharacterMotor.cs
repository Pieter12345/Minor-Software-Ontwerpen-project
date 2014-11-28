using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class NormalCharacterMotor : CharacterMotor {
	
	public float maxRotationSpeed = 270;
	
	private bool firstframe = true;

	// Jumping/falling variables.
	private float ySpeed     = 0f  ; // [m/s]
	public float ySpeedJump  = 1.2f; // [m/s] - The speed that will be set when the player jumps.
	public float gravitation = 4.0f; // [m/(s*s)]

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
		if (OnGround) {
			// Apply a force that attempts to reach our target velocity
			Vector3 velocityChange = (desiredVelocity - velocity);
			if (velocityChange.magnitude > maxVelocityChange) {
				velocityChange = velocityChange.normalized * maxVelocityChange;
			}
			movement += velocityChange;
		}


		// If the player is on the ground with a negative speed, set his speed to 0.
		if(OnGround && ySpeed < 0) { ySpeed = 0; }

		// Update y speed.
		float yPosDelta = ySpeed; // Time.deltaTime is added later.
		if(!OnGround) { ySpeed -= gravitation * Time.deltaTime; }

		movement += transform.up * yPosDelta;

		// If Jump button pressed and able to jump.
		if (Input.GetButton("Jump") && OnGround) {
			ySpeed = ySpeedJump;
		}
		
		// Apply movement.
		CollisionFlags flags = controller.Move(movement * Time.deltaTime);
		OnGround = (flags & CollisionFlags.CollidedBelow) != 0;
	}
	

}
