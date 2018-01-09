using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	Vector3 offset;

	public GameObject player;

	void Start(){
		offset = this.transform.position - player.transform.position;
	}

	void LateUpdate(){
		this.transform.position = player.transform.position + offset;
	}

}
