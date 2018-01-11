using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

	public Dialogue d;

	public void TriggerDialog(){
		FindObjectOfType<DialogueManager> ().StartDialogue (d);
	}
}
