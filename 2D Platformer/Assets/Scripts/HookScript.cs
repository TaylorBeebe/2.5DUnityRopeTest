using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookScript : MonoBehaviour {

	private Rigidbody rb;

	public bool hooked = false;

	private float momentum;

	public float speed;

	private LineRenderer line;

	Animator animator;

	public GameObject hookOrigin;

	private GameObject hook;

	public GameObject hook2D;
	
	// Initialize Line, Animator, And RigidBody
	void Start () {
		line = GetComponent<LineRenderer> ();
		animator = this.GetComponent<Animator> ();
		rb = this.GetComponent<Rigidbody> ();
	}

	void Update ()
	{
		if (Input.GetButtonDown ("Fire1")) {

			if (hooked == true) {
				hooked = false;
			}

			//Get Direction for hook shot
			Vector3 shootDirection;
			shootDirection = Input.mousePosition;
			shootDirection.z = 0.0f;
			shootDirection = Camera.main.ScreenToWorldPoint (shootDirection);
			shootDirection = shootDirection - hookOrigin.transform.position;

			//Instantiate hook shot
			GameObject hook = Instantiate (hook2D, hookOrigin.transform.position, Quaternion.Euler (new Vector3 (0, 0, 0))) as GameObject;
			hook.GetComponent<Rigidbody> ().velocity = new Vector2 (shootDirection.x * speed, shootDirection.y * speed);

		}

	}

	Vector3 cubeTransformPosition(){
		return new Vector3 (hookOrigin.transform.position.x, hookOrigin.transform.position.y, 0f);
	}

	void OnAnimatorIK(){
		if (animator.GetBool ("PlatformHookExtended")) {
			if (hook != null) {
				animator.SetLookAtWeight (0.4f);
				animator.SetLookAtPosition (hook.transform.position);
				animator.SetIKPositionWeight (AvatarIKGoal.LeftHand, 0.8f);
				animator.SetIKPosition (AvatarIKGoal.LeftHand, hook.transform.position);
			}
		}
	}
}