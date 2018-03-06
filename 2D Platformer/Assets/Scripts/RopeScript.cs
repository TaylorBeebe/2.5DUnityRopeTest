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

		extending = true;

		StartCoroutine ("HookMovement");

	}

	void Update(){

		if (Vector3.Distance (transform.position, destination) <= 0.2f && !hooked) {

			retracting = true;
			destination = nodes.Peek ().transform.position;
			StartCoroutine ("HookMovement");
		} 

		//Check if rope was interfered with during deployment, destroy if reached origin 
		else if (extending) {
			if (Physics.Linecast (transform.position, hookOrigin.transform.position)) {

				//Debug.Log ("Extension interupted!");
				extending = false;
				retracting = true;
				destination = nodes.Peek ().transform.position;

				this.GetComponent<Collider> ().enabled = false;

				StopCoroutine ("HookMovement");
				StartCoroutine ("HookMovement");
			
			}
		} 

		else if (hooked) {

		}

	}
	void LateUpdate(){

		line.SetPosition (0, hookOrigin.transform.position);
		line.SetPosition (1, this.transform.position);
		
	}

	void OnCollisionEnter(Collision col){
		
		if (col.gameObject.tag == "Player" || col.gameObject.tag == "HookOrigin") {
			Physics.IgnoreCollision (col.collider, this.GetComponent<Collider> ());
		} else {
			Debug.Log ("COLLIDED");
		}

		extending = false;
		hooked = true;
		
	}

	IEnumerator HookMovement(){
//		Debug.Log ("Within Coroutine");

		//Excutes a loop until hook as reached its target
		while (true) {
//			Debug.Log ((Vector3.Distance (this.transform.position, destination) <= 0.05f));
//			Debug.Log (Vector3.Distance (this.transform.position, destination));

			if (hooked) {
				yield return null;
			}

			//Checks the distance between hook and destionation
			if (Vector3.Distance (this.transform.position, destination) <= 0.05f){

				//If the hook is currently retracting, check if there is another node in the sequence to go to
				if (retracting) {
					curHookSpeed = retractSpeed;
					if (nodes.Count > 1) {
						nodes.Pop ();
						destination = nodes.Peek ().transform.position;
					} else {
						Destroy (gameObject);
						yield break;
					}

				//If not retracting, this must be the final destination
				} else {
//					Debug.Log ("REACHED DESTINATION");
//					Debug.Log ("Destination: " + destination);
					extending = false;
					yield break;
				}
			} else {

				if (retracting) {
					curHookSpeed = retractSpeed;
					destination = nodes.Peek ().transform.position;
					
				} else{
					curHookSpeed = extendSpeed;
				}
				transform.position = Vector3.MoveTowards (transform.position, destination, extendSpeed * Time.deltaTime);
				yield return null;
			}
		}
			
	}
}