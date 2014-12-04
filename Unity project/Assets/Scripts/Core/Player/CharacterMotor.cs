using UnityEngine;
using System.Collections;

public abstract class CharacterMotor : MonoBehaviour {
	
	public float maxForwardSpeed = 1.5f; // While walking.
	public float maxBackwardsSpeed = 1.5f;
	public float maxSidewaysSpeed = 1.5f;
	
	public Vector3 forwardVector = Vector3.forward;
	protected Quaternion alignCorrection;
	
	private bool onGround = false;
	public bool OnGround {
		get { return onGround; }
		protected set { onGround = value; }
	}
	
	private bool isJumping = false;
	public bool IsJumping	{
		get { return isJumping; }
		protected set { isJumping = value; }
	}
	
	private Vector3 movementDirection;
	private Vector3 directionFaced;
	
	void Start () {
		alignCorrection = new Quaternion();
		alignCorrection.SetLookRotation(forwardVector, Vector3.up);
		alignCorrection = Quaternion.Inverse(alignCorrection);
	}
	
	public Vector3 desiredMovementDirection {
		get { return movementDirection; }
		set {
			movementDirection = value;
			if (movementDirection.magnitude>1) movementDirection = movementDirection.normalized;
		}
	}
	public Vector3 desiredFacingDirection {
		get { return directionFaced; }
		set {
			directionFaced = value;
			if (directionFaced.magnitude>1) directionFaced = directionFaced.normalized;
		}
	}
	public Vector3 desiredVelocity {
		get {
			if (movementDirection==Vector3.zero) return Vector3.zero;
			else {
				float zAxisEllipseMultiplier = (movementDirection.z>0 ? maxForwardSpeed : maxBackwardsSpeed) / maxSidewaysSpeed;
				Vector3 temp = new Vector3(movementDirection.x, 0, movementDirection.z/zAxisEllipseMultiplier).normalized;
				float length = new Vector3(temp.x, 0, temp.z*zAxisEllipseMultiplier).magnitude * maxSidewaysSpeed;
				Vector3 velocity = movementDirection * length;
				return transform.rotation * velocity;
			}
		}
	}
}