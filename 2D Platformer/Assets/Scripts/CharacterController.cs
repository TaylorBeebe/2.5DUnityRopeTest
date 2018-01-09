using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour {


	public float runSpeed = 6;
	public float walkSpeed = 2;
	public float jumpSpeed = 6;

	Animator animator;

	private Rigidbody rb;
	public float turnSmoothTime = 0.1f;

	float turnSmoothVelocity;

	public float speedSmoothTime = 0.1f;
	float speedSmoothVelocity;
	float currentSpeed;

	public bool canJump = true;

	private Vector3 distFromGroundJump = new Vector3(0f, -0.9f, 0f);

	// Use this for initialization
	void Start(){
		animator = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody> ();
	}

	
	// Update is called once per frame
	void Update () {

		Vector2 input = new Vector2 (Input.GetAxisRaw ("Horizontal"), 0f);
		Vector2 inputDir = input.normalized;

		if (inputDir != Vector2.zero) {
			float targetRotation = Mathf.Atan2 (inputDir.x, 0f) * Mathf.Rad2Deg;
			transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle (transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
		}

		bool running = Input.GetKey (KeyCode.LeftShift);
		float targetSpeed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;
		currentSpeed = Mathf.SmoothDamp (currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);

		transform.Translate (inputDir * currentSpeed * Time.deltaTime, Space.World);

		float animationSpeedPercent = ((running)?1 : .5f) * inputDir.magnitude;
		animator.SetFloat ("speedPercent", animationSpeedPercent, speedSmoothTime, Time.deltaTime);

	}

	void FixedUpdate(){

		canJump = Physics2D.Linecast (transform.position, transform.position + distFromGroundJump);
		Debug.Log(Physics2D.Linecast(transform.position, transform.position + distFromGroundJump));
		Debug.Log (canJump);

		Debug.DrawLine (transform.position, transform.position + distFromGroundJump, Color.red);

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
