using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookScript : MonoBehaviour {



	//Max length hook can travel
	public float maxRopeLength = 30f;



	//Stores destination of hook
	private Vector3 hookDestination;



	//List contating all the nodes in the rope
	private Stack<GameObject> nodes = new Stack<GameObject> ();

	//stores current speed of hook
	private float curHookSpeed;

	//Speed the hook retracts
	public float retractSpeed = 100f;

	//Speed the hook extends
	public float extendSpeed = 35f;



	//Boolean holds if the hook has hit an object or has been interupted
	public bool extending;

	//Boolean holds if the hook is currently retracting
	public bool retracting = false;

	//True if hook is currently deployed
	private bool hookExtended = false;

	//True if player is holding down hook fire button
	private bool firing = false;

	//True if player is currently hooked
	public bool hooked = false;



	//Main animator
	Animator animator;



	//Last node in the rope (Nodes are set when player is out of sight of hook)
	private GameObject lastNode;

	//cube located on player hook arm
	public GameObject hookOrigin;

	//Hook prefab
	public GameObject hook;

	//Instance of hook
	public GameObject hookShot;

	//Cube that follows mouse to test for mouse location on screen
	public GameObject reticuleTestCube;



	//Color of linerenderer when aiming hook
	public Material lineRendererFiringColor;

	//Color of linerender when rope is deployed
	public Material lineRendererRopeColor;



	//Direction hook will be fired
	private Vector3 shootDirection;



	//Renders line when player is holding down hook fire button
	private LineRenderer lineRenderer;



	//Ray used to make sure nothing has come between hook and origin
	private Ray ray;

	//Raycast for finding destination of hook
	private RaycastHit hit;



	// Initialize Line, Animator
	void Start () {
		
		animator = this.GetComponent<Animator> ();

		lineRenderer = this.GetComponent<LineRenderer> ();
		//rb = this.GetComponent<Rigidbody> ();

		//Push the origin of hook onto stack
		nodes.Push (hookOrigin);

		extending = false;

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

			hooked = false;
			extending = false;
			retracting = true;

			StopCoroutine ("HookMovement");
			StartCoroutine ("HookMovement");

		} 

		else if (Input.GetKeyDown("e") && hooked) {
		}
			
		if (Input.GetButtonUp ("Fire1") && !hookExtended && firing) {
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

		//Check if rope was interfered with during deployment, destroy if reached origin 
		if (extending) {
			
			if (Physics.Linecast (hookShot.transform.position, hookOrigin.transform.position)) {

				Debug.Log ("Extension interupted!");

				extending = false;
				retracting = true;
				hookDestination = nodes.Peek ().transform.position;

				hookShot.GetComponent<Collider> ().enabled = false;

				StopCoroutine ("HookMovement");
				StartCoroutine ("HookMovement");

			}
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

			lineRenderer.material = lineRendererFiringColor;

			//Set the vertices of Line Renderer to display where the hook will land
			lineRenderer.SetPosition (0, hookOrigin.transform.position);
			lineRenderer.SetPosition (1, hookDestination);
		} else if (extending || retracting || hooked) {
			

			lineRenderer.material = lineRendererRopeColor;

			lineRenderer.SetPosition (0, hookOrigin.transform.position);
			lineRenderer.SetPosition (1, hookShot.transform.position);

		} else {
			lineRenderer.SetPosition (0, Vector3.zero);
			lineRenderer.SetPosition (1, Vector3.zero);
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

		//Debug.DrawRay (hookSpawn, hookOrigin.transform.forward * maxRopeLength, Color.green, 2f);

		//Calculate where hook will hit
		if (Physics.Raycast (hookSpawn, hookOrigin.transform.forward * maxRopeLength, out hit, maxRopeLength)) {
			hookDestination = hit.point;
		} else {
			hookDestination = hookOrigin.transform.position + hookOrigin.transform.forward * maxRopeLength;
		}

		//Always align hook destination with z = 0
		hookDestination.z = 0f; 
//		Debug.Log ("Destination at instantiation: " + hookDestination);

		hookShot = (GameObject) Instantiate (hook, hookSpawn, hookOrigin.transform.rotation);
		extending = true;
		StartCoroutine ("HookMovement");

		//hookShot.GetComponent<RopeScript> ().destination = hookDestination;
	}

	IEnumerator HookMovement(){
		//Excutes a loop until hook as reached its target

		Debug.Log ("Extending initially set to true");
		while (true) {
//			Debug.Log ((Vector3.Distance (this.transform.position, destination) <= 0.05f));
//			Debug.Log (Vector3.Distance (this.transform.position, destination));
//			Debug.Log ("Within Coroutine");

			if (hooked || hookShot == null) {
				yield break;
			}
			//Checks the distance between hook and destionation
			if (Vector3.Distance (hookShot.transform.position, hookDestination) <= 0.01f){
				
				Debug.Log ("Distance between this and destination is less than 0.01f: " + hookDestination);

				//If the hook is currently retracting, check if there is another node in the sequence to go to
				if (retracting) {

//					Debug.Log ("Hook is retracting");

					curHookSpeed = retractSpeed;
					if (nodes.Count > 1) {
						nodes.Pop ();
						hookDestination = nodes.Peek ().transform.position;
					} else {
						retracting = false;
						hookExtended = false;
//						lineRenderer.SetPosition (0, Vector3.zero);
//						lineRenderer.SetPosition (1, Vector3.zero);
 						Destroy (hookShot);
						yield break;
					}

					//If not retracting, this must be the final destination
				} else if (!hooked) {

//					Debug.Log ("Reached limit and not hooked. Retracting");

					extending = false;
					retracting = true;
					hookShot.GetComponent<Collider> ().enabled = false;
					hookDestination = nodes.Peek ().transform.position;
					yield return null;
				}
			} else {

//				Debug.Log ("In else statement of coroutine");
//				Debug.Log (Vector3.Distance (hookShot.transform.position, hookDestination));

				if (retracting) {
					Debug.Log ("Retracting = true");
					curHookSpeed = retractSpeed;
					hookDestination = nodes.Peek ().transform.position;

				} else{
					curHookSpeed = extendSpeed;
				}
				hookShot.transform.position = Vector3.MoveTowards (hookShot.transform.position, hookDestination, extendSpeed * Time.deltaTime);

//				Debug.Log ("Yielding return null");

				yield return null;
			}
		}

	}

}