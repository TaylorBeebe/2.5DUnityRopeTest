using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 
public class Hook : MonoBehaviour {
	public Transform parentTransform;
	private RaycastHit hit;
	public Rigidbody rb;
	public bool hooked = false;
	private float momentum;
	public float speed;
	//private float step;
	private LineRenderer line;
	Animator animator;
	private Vector3 ropeTarget;
	public GameObject swingHand;

	void Start () {
		line = GetComponent<LineRenderer> ();
		animator = GameObject.FindWithTag("Player").GetComponent<Animator> ();
//		upperArm = animator.GetBoneTransform(HumanBodyBones.LeftUpperArm);
		Debug.Log (animator);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Fire1")) {
			Debug.DrawRay (cubeTransformPosition (), this.transform.rotation * Vector3.forward * 10, Color.black, 2);

			if(Physics.Raycast(cubeTransformPosition(), this.transform.rotation * Vector3.forward, out hit, 10, ~(1<<2))) {
				ropeTarget = hit.point;
				ropeTarget.z = 0;
//				Debug.Log ("hooked");
				hooked = true;
				animator.SetBool ("Hooked", true);
			}
		}
		if (Input.GetButtonUp ("Fire1")) {
			if (hooked) {
				hooked = false;
				animator.SetBool ("Hooked", false);
				//rb.isKinematic = false;
				rb.velocity = this.transform.rotation * Vector3.forward * momentum;
			}
		}
		if (hooked) {
			this.transform.LookAt (hit.point);
			line.SetPosition (0, cubeTransformPosition());
			line.SetPosition (1, hit.point);
		} else {
			// keep the distance locked, so that the player can swing like a pendulum
			var v3 = Input.mousePosition;
			v3.z = 0;
			v3 = Camera.main.ScreenToWorldPoint (v3);
			v3.z = 0;
			this.transform.LookAt (v3);
			line.SetPosition (0, cubeTransformPosition());
			line.SetPosition (1, cubeTransformPosition());
		}
			
		if (hooked && Input.GetKey(KeyCode.E)) {
			momentum += Time.deltaTime * speed;
			//step = momentum * Time.deltaTime;
			rb.AddForce (Vector3.Normalize(hit.point - cubeTransformPosition()) * 10000 * Time.deltaTime);
			//parentTransform.position = Vector3.MoveTowards (parentTransform.position, hit.point, step);
		}
		else if (momentum >= 0) {
			momentum = 0;
			//ep = 0;
		}
	}

	Vector3 cubeTransformPosition(){
		return new Vector3 (this.transform.position.x, this.transform.position.y, 0);
	}
		
//	void LateUpdate(){
//
//		if (animator.GetBool("Hooked")){
//			setArmRotation ();
//		}
//
//	}
	 
	void setArmRotation(){

//		animator.SetLookAtWeight (1);
//		animator.SetLookAtPosition (ropeTarget);
//		//animator.SetIKPositionWeight (AvatarIKGoal.LeftHand, 1);
//		animator.SetIKRotationWeight (AvatarIKGoal.LeftHand, 1);
//		//animator.SetIKPosition (AvatarIKGoal.LeftHand, swingHand.transform.position);
//		animator.SetIKRotation (AvatarIKGoal.LeftHand, swingHand.transform.rotation);
	}

	void OnAnimatorIK(){
		Debug.Log ("ON ANIM IK");
		if (animator.GetBool ("Hooked")) {

			animator.SetLookAtWeight (1);
			animator.SetLookAtPosition (ropeTarget);
			//animator.SetIKPositionWeight (AvatarIKGoal.LeftHand, 1);
			animator.SetIKRotationWeight (AvatarIKGoal.LeftHand, 1);
			//animator.SetIKPosition (AvatarIKGoal.LeftHand, swingHand.transform.position);
			animator.SetIKRotation (AvatarIKGoal.LeftHand, swingHand.transform.rotation);
		}
	}
}
