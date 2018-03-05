using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookScript : MonoBehaviour {

//	private Rigidbody rb;

	public float shootSpeed = 50f;

	public float maxRopeLength = 30f;

	public bool extending = false;

	private bool hookExtended = false;

	private bool firing;

	Animator animator;

	public GameObject hookOrigin;

	public GameObject hook;

	public GameObject hookShot;

	private Vector3 shootDirection;

	public GameObject reticuleTestCube;

	public LineRenderer lineRenderer;

	private Vector3 hookDestination;

	private RaycastHit hit;

	// Initialize Line, Animator
	void Start () {
		animator = this.GetComponent<Animator> ();
		lineRenderer = this.GetComponent<LineRenderer> ();
		//rb = this.GetComponent<Rigidbody> ();
	}

	void Update (){



		shootDirection = Input.mousePosition;
		shootDirection.z = 5f; // Camera is at z = -5
		shootDirection = Camera.main.ScreenToWorldPoint(shootDirection);

		hookOrigin.transform.LookAt (shootDirection);

		//reticuleTestCube.transform.position = shootDirection;

		if (Input.GetButtonDown ("Fire1") && !hookExtended) {
			firing = true;
		}

		if (firing) {
			
			if (Physics.Raycast (hookOrigin.transform.position, hookOrigin.transform.forward * maxRopeLength, out hit, maxRopeLength)) {
				hookDestination = hit.point;
			} else {
				hookDestination = hookOrigin.transform.position + hookOrigin.transform.forward * maxRopeLength;
			}

			hookDestination.z = 0f;

			lineRenderer.SetPosition (0, hookOrigin.transform.position);
			lineRenderer.SetPosition (1, hookDestination);
		}

		if (Input.GetButtonUp ("Fire1") && !hookExtended) {
			firing = false;

			lineRenderer.SetPosition (0, Vector3.zero);
			lineRenderer.SetPosition (1, Vector3.zero);

			hookExtended = true;

			//Instantiate hook shot after arm has time to move
			Invoke("WaitToShoot", 0.1f);
		}

		//If hook has been destroyed, set hookExtended to false
		if (hookShot == null) {
			hookExtended = false;
		}
	}

	Vector3 cubeTransformPosition(){
		return new Vector3 (hookOrigin.transform.position.x, hookOrigin.transform.position.y, 0f);
	}

	//Directs the animator to move left arm toward the hook
	void OnAnimatorIK(){
			animator.SetLookAtWeight (0.4f);
			animator.SetLookAtPosition (shootDirection);
			animator.SetIKPositionWeight (AvatarIKGoal.LeftHand, 0.8f);
			animator.SetIKPosition (AvatarIKGoal.LeftHand, shootDirection);
		}

	void WaitToShoot(){
		
		hookOrigin.transform.LookAt (shootDirection, Vector3.forward);

		hookShot = (GameObject) Instantiate (hook, hookOrigin.transform.position + hookOrigin.transform.forward * 0.09f, hookOrigin.transform.rotation);

		//Debug.DrawRay (hook.transform.position, hook.transform.forward * maxRopeLength, Color.green, 2f);

		if (Physics.Raycast (hookShot.transform.position, hookShot.transform.forward * maxRopeLength, out hit, maxRopeLength)) {
			hookDestination = hit.point;
		} else {
			hookDestination = hookShot.transform.position + hookShot.transform.forward * maxRopeLength;
		}
		hookDestination.z = 0f;
		hookShot.GetComponent<RopeScript> ().destination = hookDestination;
	}

}