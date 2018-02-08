using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour {


	public float runSpeed = 6;
	public float walkSpeed = 2;
	public float jumpSpeed = 6;

	Animator animator;

	private Rigidbody rb;
	public float turnSmoothTime = 0.05f;

	float turnSmoothVelocity;

	public float speedSmoothTime = 0.1f;
	float speedSmoothVelocity;
	float currentSpeed;

	public bool canJump = true;

	private Vector2 currentDir;

	public float distFromGroundCanJump = 0.1f;

	// Use this for initialization
	void Start(){
		animator = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody> ();
		currentDir = new Vector2 (1f, 0f);
	}

	
	// Update is called once per frame
	void Update () {
		
		Vector2 input = new Vector2 (Input.GetAxisRaw ("Horizontal"), 0f);
		if (input != Vector2.zero) {
			currentDir = new Vector2 (input.x, 0f);
		}
		float targetRotation = Mathf.Atan2 (currentDir.x, 0f) * Mathf.Rad2Deg;
		transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle (transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);

		currentSpeed = Mathf.SmoothDamp (currentSpeed, runSpeed, ref speedSmoothVelocity, speedSmoothTime);

		transform.Translate (input * currentSpeed * Time.deltaTime, Space.World);

		animator.SetFloat ("speedPercent", input.magnitude, speedSmoothTime, Time.deltaTime);

	}

	void FixedUpdate(){

		//Debug.Log (canJump);
		//Debug.DrawRay (Physics.Raycast(transform.position, -Vector3.up, distFromGroundCanJump + 0.1f));
		//Debug.Log (Physics.Raycast (transform.position + new Vector3 (0f, 1f, 0f), Vector3.down, (0.1f + distFromGroundCanJump)));
		if (Input.GetKeyDown ("space") && canJump) {
			canJump = false;
			animator.SetTrigger ("Jump");
			rb.velocity += new Vector3(0f, jumpSpeed, 0f);
			//Debug.Log ("RAN JUMP");
			animator.ResetTrigger ("Jump");
		}
	}


	public void setFalling(bool b){
		animator.SetBool ("Falling", b);
	}

	public bool getFalling(){
		return animator.GetBool ("Falling");
	}
}
