﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookScript : MonoBehaviour {



	//List contating all the nodes in the rope
	private List<GameObject> nodes = new List<GameObject> ();



	//stores current speed of hook
	private float curHookSpeed;

	//Speed the hook retracts
	public float retractSpeed = 75f;

	//Speed the hook extends
	public float extendSpeed = 50f;

	//Max length hook can travel
	public float maxRopeLength = 30f;

	public float nodeOffset = 0.01f;



	//Boolean holds if the hook has hit an object or has been interupted
	public bool extending;

	//Boolean holds if the hook is currently retracting
	private bool retracting = false;

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

	private GameObject newNode;

	//cube located on player hook arm
	public GameObject hookOrigin;

	//Hook prefab
	public GameObject hook;

	//Instance of hook
	public GameObject hookShot;

	//Rope node instantiated if player and hook are not in LOS
	public GameObject ropeNode;

	//Cube that follows mouse to test for mouse location on screen
	public GameObject reticuleTestCube;



	//Color of linerenderer when aiming hook
	public Material lineRendererFiringColor;

	//Color of linerender when rope is deployed
	public Material lineRendererRopeColor;



	//Direction hook will be fired
	private Vector3 shootDirection;

	//Stores destination of hook
	private Vector3 hookDestination;



	//Renders line when player is holding down hook fire button
	private LineRenderer lineRenderer;



	//Ray used to make sure nothing has come between hook and origin
	private Ray ray;

	//Raycast for finding destination of hook
	private RaycastHit hit;



	// Initialize Line, Animator
	void Start () {
		
		//Grab animator component
		animator = this.GetComponent<Animator> ();

		//Renders rope and firing lines
		lineRenderer = this.GetComponent<LineRenderer> ();

		//Push the origin of hook onto stack
		nodes.Add (hookOrigin);

		//Set extending to be false initially
		extending = false;

	}

	void Update (){

		//Calculates position of the mouse in worldspace
		shootDirection = Input.mousePosition;
		shootDirection.z = 5f; // Camera is at z = -5
		shootDirection = Camera.main.ScreenToWorldPoint(shootDirection);

		//Sets the hookOrigin to look at the worldspace mouse position
		hookOrigin.transform.LookAt (shootDirection);

//		reticuleTestCube.transform.position = shootDirection;

		//If player hitting hook fire button, set firing to true so destination line can be rendered
		if (Input.GetButtonDown ("Fire1") && !hookExtended) {
			firing = true;
		}

		//If player clicks while hook is deployed, retract hook
		else if (Input.GetButtonDown ("Fire1") && (hookExtended || hooked)) {

			beginHookRetraction();

		} 

		//Used to propel player toward the hook
		else if (Input.GetKeyDown("e") && hooked) {
		}

		//Fire the hook if firing and !hookextended are true to avoid being able to fire nonstop
		if (Input.GetButtonUp ("Fire1") && !hookExtended && firing) {
			
			//Stop the firing loop from running
			firing = false;

			//Reset Vertices in Line Renderer

			setLineRendererPositionCount (0);

			//Set extended to true for other method dependencies
			hookExtended = true;

			//Instantiate hook shot after arm has time to move
//			Invoke("WaitToShoot", 0.1f);
			WaitToShoot();
		}

		//Check if rope was interfered with during deployment, destroy if reached origin 
		if (hookExtended) {
			
			if (Physics.Linecast (lastNode.transform.position, hookOrigin.transform.position, out hit)) {
//				Debug.Log (lastNode.transform.position);
				if (extending) {

					beginHookRetraction ();

				} else if (hooked) {

					spawnNode (hit);

				}
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
			if (!checkLineRendererPositionCount(2)){
				setLineRendererPositionCount (2);
			}
			lineRenderer.SetPosition (0, hookOrigin.transform.position);
			lineRenderer.SetPosition (1, hookDestination);
		} else if ((extending || retracting || hooked) && hookShot != null) {
			
//			Debug.Log("hooked: " + hooked);
//			Debug.Log("extending: " + extending);
//			Debug.Log("retracting: " + retracting);

			lineRenderer.material = lineRendererRopeColor;
			if (!checkLineRendererPositionCount(nodes.Count)) {
				setLineRendererPositionCount (nodes.Count);
			}
			int x = 0;
			foreach (GameObject node in nodes) {
				lineRenderer.SetPosition (x, node.transform.position);
				x++;
			}


		} 
	}

	Vector3 cubeTransformPosition(){
		return new Vector3 (hookOrigin.transform.position.x, hookOrigin.transform.position.y, 0f);
	}

	//Directs the animator to move left arm toward the hook
	void OnAnimatorIK(){

		Vector3 ikGoal = Vector3.zero;
		bool animate = false;
		if (firing) {
			ikGoal = shootDirection;
			animate = true;
		} else if (extending || retracting || hooked) {
			if (nodes.Count > 2) {
				ikGoal = nodes [nodes.Count - 1].transform.position;
			} else {
				ikGoal = nodes [nodes.Count - 1].transform.position;
			}
			animate = true;
		}
		if (animate) {
			animator.SetLookAtWeight (0.4f);
			animator.SetLookAtPosition (ikGoal);
			animator.SetIKPositionWeight (AvatarIKGoal.LeftHand, 0.8f);
			animator.SetIKPosition (AvatarIKGoal.LeftHand, ikGoal);
		}
	}

	void spawnNode(RaycastHit nodeHit){
		Debug.Log ("Creating new node");
		newNode = (GameObject) Instantiate (ropeNode, hit.point + (hit.normal * nodeOffset), Quaternion.identity);

		int index = nodes.IndexOf (lastNode);

		if (index != -1) {
			nodes.Insert (index, newNode);
		} else {
			nodes.Add (newNode);
		}

		if(nodes.Count > 3 && !Physics.Linecast(nodes[index + 2].transform.position, newNode.transform.position)){
			Debug.Log ("Destroying Unnecessary Node");
			nodes.RemoveAt (index + 1);
			Destroy (lastNode);
		}

		lastNode = newNode;
	}

	void beginHookRetraction(){
		StopCoroutine ("HookMovement");
		extending = false;
		retracting = true;
		hooked = false;
		lastNode = nodes [nodes.Count - 2];
		hookDestination = lastNode.transform.position;

		hookShot.GetComponent<Collider> ().enabled = false;

		StartCoroutine ("HookMovement");
	}

	bool checkLineRendererPositionCount(int count){
		return lineRenderer.positionCount == count;
	}

	void setLineRendererPositionCount(int count){
		lineRenderer.positionCount = count;
	}

	void WaitToShoot(){
		
		hookOrigin.transform.LookAt (shootDirection, Vector3.forward);

		Vector3 hookSpawn = hookOrigin.transform.position + hookOrigin.transform.forward * 0.09f;

//		Debug.DrawRay (hookSpawn, hookOrigin.transform.forward * maxRopeLength, Color.green, 2f);

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
		nodes.Add (hookShot);
		lastNode = hookShot;
		extending = true;
		StartCoroutine ("HookMovement");

		//hookShot.GetComponent<RopeScript> ().destination = hookDestination;
	}

	IEnumerator HookMovement(){

		//Excutes a loop until hook as reached its target
		while (true) {

			//Make sure hook is not hooked on object and an instance of hookShot exists
			if (hooked || hookShot == null) {
				yield break;
			}
			//Checks the distance between hook and destionation
			if (Vector3.Distance (hookShot.transform.position, hookDestination) <= 0.01f){
				
//				Debug.Log ("Distance between this and destination is less than 0.01f: " + hookDestination);

				//If the hook is currently retracting, check if there is another node in the sequence to go to
				if (retracting) {

//					Debug.Log ("Hook is retracting");

					curHookSpeed = retractSpeed;
					if (nodes.Count > 2) {
						
						nodes.RemoveAt (nodes.Count - 2);
						Destroy (lastNode);
						lastNode = nodes [nodes.Count - 2];
						hookDestination = lastNode.transform.position;
					} else {

						//Set retracting and hookExtended to false
						retracting = false;
						hookExtended = false;


						//Setting extending to false is a quick-fix to avoid bug where extending is
						//not set to false before getting to this point. TODO: Identify issue in logic!!!
						extending = false;
 						
						//Destroy instance of hookshot
						Destroy (hookShot);
						lastNode = null;
						nodes.Clear ();
						nodes.Add (hookOrigin);
						setLineRendererPositionCount (0);
						yield break;
					}

				//If hook has not latched, begin to retract
				} else if (!hooked) {

//					Debug.Log ("Reached limit and not hooked. Retracting");
					beginHookRetraction();

				}
			} else {

//				Debug.Log ("In else statement of coroutine");

				//If retracting, update position of last node and set current hook speed to retract speed
				if (retracting) {
//					Debug.Log ("Retracting = true");
					curHookSpeed = retractSpeed;
					hookDestination = nodes [nodes.Count - 2].transform.position;
//					hookDestination = nodes.Last.Value.transform.position;

				//If not retracting, must be extending. Set hook speed to extend speed
				} else{
					curHookSpeed = extendSpeed;
				}
			}
			//Update hooks positon and return to top of coroutine
			hookShot.transform.position = Vector3.MoveTowards (hookShot.transform.position, hookDestination, curHookSpeed * Time.deltaTime);
			yield return null;
		}

	}

}