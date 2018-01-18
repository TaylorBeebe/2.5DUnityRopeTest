using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthController : MonoBehaviour {

	private int maxHealth, currentHealth;
	public Image healthBar;
	// Use this for initialization
	void Start () {

		maxHealth = 100;
		currentHealth = 100;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp(KeyCode.X)){
			currentHealth -= 6;
			healthBar.fillAmount = (float)currentHealth/maxHealth;
		}

		if (healthBar.fillAmount <= 0) {
			death ();
		}
	}

	void death(){
		Debug.Log ("PLAYER DIED");
	}
}
