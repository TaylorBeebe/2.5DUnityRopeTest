using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookScript : MonoBehaviour {

	//Max length hook can travel
	public float maxRopeLength = 30f;

	//True if hook is currently deployed
	private bool hookExtended = false;

	//True if player is holding down hook fire button
	private bool firing = false;

	//True if player is currently hooked
	public bool hooked = false;

	//Main animator
	Animator animator;

	//cube located on player hook arm
	public GameObject hookOrigin;

	//Hook prefab
	public GameObject hook;

	//Instance of hook
	public GameObject hookShot;

	//Direction hook will be fired
	private Vector3 shootDirection;

	//Cube that follows mouse to test for mouse location on screen
	public GameObject reticuleTestCube;

	//Renders line when player is holding down hook fire button
	public LineRenderer lineRenderer;

	//Stores destination of hook
	private Vector3 hookDestination;

	//Raycast for finding destination of hook
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

		//If player hitting hook fire button, set firing to true so destination line can be rendered
		if (Input.GetButtonDown ("Fire1") && !hookExtended) {
			firing = true;
		}

		//If player clicks while hook is deployed, retract hook
		else if (Input.GetButtonDown ("Fire1") && (hookExtended || hooked)) {
			
//			Debug.Log ("Setting retracting to true");

			hookShot.GetComponent<RopeScript> ().retracting = true;
			hooked = false;
		} 

		else if (Input.GetKeyDown("e") && hooked) {
		}
			
		if (Input.GetButtonUp ("Fire1") && !hookExtended) {
			//Stop the firing loop from running
			firing = false;
			//Reset Vertices in Line Renderer
			lineRenderer.SetPosition (0, Vector3.zero);
			lineRenderer.SetPosition (1, Vector3.zero);

			hookExtended = true;

			hookShot = hook;

			//Instantiate hook shot after arm has time to move
			Invoke("WaitToShoot", 0.1f);
		}

		//If hook has been destroyed, set hookExtended to false
		else if (hookShot == null) {
			hookExtended = false;
		}
	}

	void LateUpdate(){
		if (firing) {
			//Check where hook will land
			if (Physics.Raycast (hookOrigin.transform.position, hookOrigin.transform.forward * maxRopeLength, out hit, maxRopeLength)) {
				hookDestination = hit.point;
			} else {
				hookDestination = hookOrigin.transform.position + hookOrigin.transform.forward * maxRopeLength;
			}

			//Always set z = 0
			hookDestination.z = 0f;

			//Set the vertices of Line Renderer to display where the hook will land
			lineRenderer.SetPosition (0, hookOrigin.transform.position);
			lineRenderer.SetPosition (1, hookDestination);
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

		Vector3 hookSpawn = hookOrigin.transform.position + hookOrigin.transform.forward * 0.09f;

		//Debug.DrawRay (hook.transform.position, hook.transform.forward * maxRopeLength, Color.green, 2f);

		if (Physics.Raycast (hookSpawn, hookOrigin.transform.forward * maxRopeLength, out hit, maxRopeLength)) {
			hookDestination = hit.point;
		} else {
			hookDestination = hookOrigin.transform.position + hookOrigin.transform.forward * maxRopeLength;
		}

		//Always align hook destination with z = 0
		hookDestination.z = 0f; 
		Debug.Log ("Destination at instantiation: " + hookDestination);
		hookShot = (GameObject) Instantiate (hook, hookSpawn, hookOrigin.transform.rotation);
		hookShot.GetComponent<RopeScript> ().destination = hookDestination;
	}
}