using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookScript : MonoBehaviour {

//	private Rigidbody rb;

	public float shootSpeed = 1;

	public bool extending = false;

	private bool hookExtended = false;

	Animator animator;

	public GameObject hookOrigin;

	public GameObject hook;

	private Vector3 shootDirection;
	
	// Initialize Line, Animator, And RigidBody
	void Start () {
		animator = this.GetComponent<Animator> ();
		//rb = this.GetComponent<Rigidbody> ();
	}

	void Update (){
		if (Input.GetButtonDown ("Fire1") && !hookExtended) {

			//Set both animator and internal hookExtended variables to true
			hookExtended = true;
			animator.SetBool ("PlatformHookExtended", true);
			extending = true;
			Debug.Log (Input.mousePosition);
			//Get Direction for hook shot
			shootDirection = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0f);
			shootDirection = Vector3.Normalize(shootDirection);
			Debug.Log (shootDirection);
			//Instantiate hook shot after arm has time to move
			Invoke("WaitToShoot", 0.5f);
		}

		//Sets the hookOrigin cube to face the shootdirection at all times
		if (hookExtended) {
			hookOrigin.transform.LookAt (shootDirection);
		}

		if (!extending) {
			shootDirection = Vector3.zero;
		}

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
		if (animator.GetBool ("PlatformHookExtended")) {
			
			Vector3 IKTarget;

			//Sets the target of the IK based on whether the hook was just shot or
			//has attached to an object
			if (hook != null && shootDirection != Vector3.zero) {
				IKTarget = shootDirection;
			} else {
				IKTarget = hook.transform.position;
			}

			//Sets the weights of the IK bones
			animator.SetLookAtWeight (0.4f);
			animator.SetLookAtPosition (shootDirection);
			animator.SetIKPositionWeight (AvatarIKGoal.LeftHand, 0.8f);
			animator.SetIKPosition (AvatarIKGoal.LeftHand, shootDirection);
			}
		}

	void WaitToShoot(){
		hook = (GameObject) Instantiate (hook, hookOrigin.transform.position, Quaternion.Euler (new Vector3 (0, 0, 0)));
		hook.transform.LookAt (shootDirection);
		hook.GetComponent<Rigidbody> ().AddForce(shootDirection * shootSpeed);
		Debug.Log ("Hook velocity is: " + hook.GetComponent<Rigidbody> ().velocity);
	}

}
