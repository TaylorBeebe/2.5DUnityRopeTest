using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacterController : MonoBehaviour {


	public float runSpeed = 12;
	public float walkSpeed = 4;
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

	private bool backColliding;
	private bool frontColliding;

	// Use this for initialization
	void Start(){
		animator = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody> ();
		currentDir = new Vector2 (1f, 0f);
	}

	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate(){
		Vector2 input = new Vector2 (Input.GetAxisRaw ("Horizontal"), 0f);
		if (input != Vector2.zero) {
			currentDir = new Vector2 (input.x, 0f);
		}
		float targetRotation = Mathf.Atan2 (currentDir.x, 0f) * Mathf.Rad2Deg;
		transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle (transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);

		bool running = Input.GetKey (KeyCode.LeftShift);
		float targetSpeed = ((running) ? runSpeed : walkSpeed) * input.magnitude;
		currentSpeed = Mathf.SmoothDamp (currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);


		if (!frontColliding) {
			transform.Translate (input * currentSpeed * Time.deltaTime, Space.World);
		}

		float animationSpeedPercent = ((running)?1 : .5f) * input.magnitude;
		animator.SetFloat ("speedPercent", animationSpeedPercent, speedSmoothTime, Time.deltaTime);


		canJump = Physics.Raycast (transform.position + new Vector3 (0f, 0.1f, 0f), Vector3.down, (0.1f + distFromGroundCanJump));
		//Debug.Log (canJump);
		//Debug.DrawRay (transform.position + new Vector3 (0f, 0.1f, 0f), Vector3.down, Color.red, (0.1f + distFromGroundCanJump));
		//Debug.Log ());
		if (Input.GetKeyDown ("space") && canJump) {
			canJump = false;
			animator.SetTrigger ("Jump");
			rb.velocity += new Vector3(0f, jumpSpeed, 0f);
			//Debug.Log ("RAN JUMP");
			//animator.ResetTrigger ("Jump");
		}

		setFalling (!canJump);
		this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0);
	}


	public void setFalling(bool b){
		animator.SetBool ("Falling", b);
	}

	public bool getFalling(){
		return animator.GetBool ("Falling");
	}

	public void triggerEnter(string triggerName){
		if (triggerName == "Back") {
			backColliding = true;
		}
		if (triggerName == "Front") {
			frontColliding = true;
		}
	}

	public void triggerExit(string triggerName){
		if (triggerName == "Back") {
			backColliding = false;
		}
		if (triggerName == "Front") {
			frontColliding = false;
		}
	}

	public void triggerStay(string triggerName){

	}
}
