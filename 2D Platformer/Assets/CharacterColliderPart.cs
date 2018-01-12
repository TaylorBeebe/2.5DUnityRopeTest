using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterColliderPart : MonoBehaviour {

	public string triggerName;
	public BaseCharacterController controller;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(){
		controller.triggerEnter (triggerName);
	}

	void OnTriggerExit(){
		controller.triggerExit (triggerName);
	}

	void OnTriggerStay(){
		controller.triggerStay (triggerName);
	}
}
