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
	public float HorizontalSpeedMultiplierWhenInAir = 0.5f; // Slows down the player in the x-z plane when jumping/falling.

	public float sneakMultiplier = 0.5f;
	public float sprintMultiplier = 2f;

	private bool isSprinting = false;
	public bool getIsSprinting { get{ return isSprinting; } }


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

		velocity -= Vector3.Project((new Vector3(velocity.x, 0, velocity.z)), transform.up);

		// Calculate how fast we should be moving
		Vector3 movement = velocity;
		
		// Apply a force that attempts to reach our target velocity
		Vector3 velocityChange = (desiredVelocity - velocity);
		movement += new Vector3(velocityChange.x, 0f, velocityChange.z); // Ignore height velocity. Jumping is handled somewhere else.
		if(velocityChange.y > 1f || velocityChange.y < -1f) {
			movement.x *= HorizontalSpeedMultiplierWhenInAir;
			movement.z *= HorizontalSpeedMultiplierWhenInAir;
		}
		
		// Sneaking.
		if(Input.GetKey("left ctrl") || Input.GetButton("Sneak")) {
			movement.x *= sneakMultiplier;
			movement.z *= sneakMultiplier;
		}
		
		// Sprinting.
		isSprinting = Input.GetKey("left shift") || Input.GetButton("Sprint");
		if(isSprinting) {
			movement.x *= sprintMultiplier;
			movement.z *= sprintMultiplier;
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
