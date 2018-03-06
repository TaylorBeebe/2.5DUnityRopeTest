using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeScript : MonoBehaviour {

	//Boolean holds if the hook has hit an object or has been interupted
	private bool extending = true;

	//Boolean holds if the hook successfully attached to an object
	private bool hooked = false;

	//Boolean holds if the hook is currently retracting
	public bool retracting = false;

	//Boolean holds if hook has reached last node during retraction
	private bool arrivedToLastRetractionNode;

	//Ray used to make sure nothing has come between hook and origin
	private Ray ray;

	//Speed the hook retracts
	public float retractSpeed = 100f;

	//Speed the hook extends
	public float extendSpeed = 35f;

	//furthest the rope can go
	public float maxDistance = 50f;

	//stores current speed of hook
	private float curHookSpeed;

	//Where hook is heading
	public Vector3 destination;

	//Last node in the rope (Nodes are set when player is out of sight of hook)
	public GameObject lastNode;

	//distance constant that sets how far away the last node is before
	//a new node is created
	public float minDistance = 0.3f;

	//renders line between each rope node
	private LineRenderer line;

	//origin of the hook on players arm
	private GameObject hookOrigin;

	//player gameobject
	private GameObject player;

	//List contating all the nodes in the rope
	private Stack<GameObject> nodes = new Stack<GameObject> ();


	void Start(){
		//Gets the line render component
		line = this.GetComponent<LineRenderer> ();

		//Get reference to Player
		player = GameObject.FindGameObjectWithTag ("Player");

		//Get the hook's origin
		hookOrigin = GameObject.FindGameObjectWithTag ("HookOrigin");

		//Push the origin of hook onto stack
		nodes.Push (hookOrigin);

		//last node is intially the hoook
		lastNode = this.gameObject;

		//Hook is initially extending
		extending = true;

		retracting = false;

		//Start coroutine that controls hook movement
		StartCoroutine ("HookMovement");

	}

	void Update(){

//		if (Vector3.Distance (transform.position, destination) <= 0.2f && !hooked) {
//			Debug.Log ("Destination: " + destination);
//			Debug.Log ("Reached limit and not hooked. Retracitng");
//			retracting = true;
//			destination = nodes.Peek ().transform.position;
//			StartCoroutine ("HookMovement");
//		} 

		//Check if rope was interfered with during deployment, destroy if reached origin 
		if (extending) {
			if (Physics.Linecast (transform.position, hookOrigin.transform.position)) {

				Debug.Log ("Extension interupted!");
				extending = false;
				retracting = true;
				destination = nodes.Peek ().transform.position;

				this.GetComponent<Collider> ().enabled = false;

				StopCoroutine ("HookMovement");
				StartCoroutine ("HookMovement");
			
			}
		} 

//		else if (hooked) {
//
//		}

	}
	void LateUpdate(){

		line.SetPosition (0, hookOrigin.transform.position);
		line.SetPosition (1, this.transform.position);
		
	}

	void OnCollisionEnter(Collision col){
		Debug.Log ("On collision enter");
		if (col.gameObject.tag == "Player" || col.gameObject.tag == "HookOrigin") {
			Physics.IgnoreCollision (col.collider, this.GetComponent<Collider> ());
		} else {
			Debug.Log ("COLLIDED");
			extending = false;
			hooked = true;

			Debug.Log (player);
			player.GetComponent<HookScript> ().hooked = true;
		}
	}

	IEnumerator HookMovement(){
		//Excutes a loop until hook as reached its target
		while (true) {
//			Debug.Log ((Vector3.Distance (this.transform.position, destination) <= 0.05f));
//			Debug.Log (Vector3.Distance (this.transform.position, destination));

//			Debug.Log ("Within Coroutine");
//			Debug.Log ("Hooked: " + hooked);
//			Debug.Log ("Retracting: " + retracting);

			if (hooked) {
				yield break;
			}

			//Checks the distance between hook and destionation
			if (Vector3.Distance (this.transform.position, destination) <= 0.01f){
				
//				Debug.Log ("Distance between this and destination is less than 0.05f");

				//If the hook is currently retracting, check if there is another node in the sequence to go to
				if (retracting) {
					
//					Debug.Log ("Hook is retracting");

					curHookSpeed = retractSpeed;
					if (nodes.Count > 1) {
						nodes.Pop ();
						destination = nodes.Peek ().transform.position;
					} else {
						Destroy (gameObject);
						yield break;
					}

				//If not retracting, this must be the final destination
				} else if (!hooked) {
					
//					Debug.Log ("Reached limit and not hooked. Retracting");

					extending = false;
					retracting = true;
					destination = nodes.Peek ().transform.position;
					yield return null;
				}
			} else {
				
//				Debug.Log ("In else statement of coroutine");

				if (retracting) {
					curHookSpeed = retractSpeed;
					destination = nodes.Peek ().transform.position;
					
				} else{
					curHookSpeed = extendSpeed;
				}
				transform.position = Vector3.MoveTowards (transform.position, destination, extendSpeed * Time.deltaTime);

//				Debug.Log ("Yielding return null");

				yield return null;
			}
		}
			
	}
}