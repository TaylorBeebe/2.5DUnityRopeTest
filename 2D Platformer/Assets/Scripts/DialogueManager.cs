using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DialogueManager : MonoBehaviour {

	private Queue<string> sentences;

	public Text nameText;
	public Text dialogueText;

	void Start () {
		sentences = new Queue<string> ();
	}

	public void StartDialogue (Dialogue d){

		Debug.Log ("starting conversation");

		sentences.Clear ();
		nameText.text = d.name;
		foreach (string sentence in d.sentences) {
			sentences.Enqueue (sentence);
		}
		DisplayNextSentence ();
	}

	public void DisplayNextSentence(){
		if (sentences.Count == 0) {
			EndDialogue ();
			return;
		}
		string sentence = sentences.Dequeue ();
		dialogueText.text = sentence;
	}

	void EndDialogue(){
		Debug.Log ("Ending dialogue");
	}
}
