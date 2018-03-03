using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookScript : MonoBehaviour {

//	private Rigidbody rb;

	public float shootSpeed = 50f;

	public float maxRopeLength = 30f;

	public bool extending = false;

	private bool hookExtended = false;

	Animator animator;

	public GameObject hookOrigin;

	public GameObject hook;

	private GameObject hookShot;

	private Vector3 shootDirection;

	public GameObject reticuleTestCube;

	
	// Initialize Line, Animator, And RigidBody
	void Start () {
		animator = this.GetComponent<Animator> ();
		//rb = this.GetComponent<Rigidbody> ();
	}

	void Update (){

		hookOrigin.transform.LookAt (shootDirection);

		shootDirection = Input.mousePosition;
		shootDirection.z = 5f; // Camera is at z = -5
		shootDirection = Camera.main.ScreenToWorldPoint(shootDirection);

		if (Input.GetButtonDown ("Fire1") && !hookExtended) {

			//Set both animator and internal hookExtended variables to true
//			hookExtended = true;
			animator.SetBool ("PlatformHookExtended", true);
//			extending = true;

//			shootDirection = Input.mousePosition;
//			shootDirection.z = 5f; // Camera is at z = -5
//			shootDirection = Camera.main.ScreenToWorldPoint(shootDirection);

			//reticuleTestCube.transform.position = shootDirection;

			//Instantiate hook shot after arm has time to move
			Invoke("WaitToShoot", 0.1f);
		}

		//Sets the hookOrigin cube to face the shootdirection at all times
		if (hookExtended) {
			hookOrigin.transform.LookAt (shootDirection);
		}

//		if (!extending) {
//			shootDirection = Vector3.zero;
//		}

		//If hook has been destroyed, set hookExtended to false
		if (hook == null) {
			hookExtended = false;
			animator.SetBool ("PlatformHookExtended", false);
		}
	}

	Vector3 cubeTransformPosition(){
		return new Vector3 (hookOrigin.transform.position.x, hookOrigin.transform.position.y, 0f);
	}

	//Directs the animator to move left arm toward the hook
	void OnAnimatorIK(){
		//shootDirection = Input.mousePosition;
		//if (animator.GetBool ("PlatformHookExtended")) {
			
			//Vector3 IKTarget;

			//Sets the target of the IK based on whether the hook was just shot or
			//has attached to an object
//			if (hook != null && shootDirection != Vector3.zero) {
//				IKTarget = shootDirection;
//			} else {
//				IKTarget = hook.transform.position;
//			}

			//Sets the weights of the IK bones
			animator.SetLookAtWeight (0.4f);
			animator.SetLookAtPosition (shootDirection);
			animator.SetIKPositionWeight (AvatarIKGoal.LeftHand, 0.8f);
			animator.SetIKPosition (AvatarIKGoal.LeftHand, shootDirection);
			//}
		}

	void WaitToShoot(){
		hookOrigin.transform.LookAt (shootDirection, Vector3.forward);
		hookShot = (GameObject) Instantiate (hook, hookOrigin.transform.position + hookOrigin.transform.forward * 0.09f, hookOrigin.transform.rotation);

		RaycastHit hit;

		//Debug.DrawRay (hookShot.transform.position, hookShot.transform.forward * maxRopeLength, Color.green, 2f);

		if (Physics.Raycast (hookShot.transform.position, hookShot.transform.forward * maxRopeLength, out hit, maxRopeLength)) {
			hookShot.GetComponent<RopeScript> ().destination = hit.point;
		} else {
			hookShot.GetComponent<RopeScript>().destination = hookShot.transform.position + hookShot.transform.forward * maxRopeLength;
		}
	}

}