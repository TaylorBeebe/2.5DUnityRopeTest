using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DialogueManager : MonoBehaviour {

	private Queue<string> sentences;

	public Text nameText;
	public Text dialogueText;
	public Animator animator;

	void Start () {
		sentences = new Queue<string> ();
	}

	public void StartDialogue (Dialogue d){

		//Debug.Log ("starting conversation");

		animator.SetBool ("startDialog", true);

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
		animator.SetBool ("startDialog", false);
		//Debug.Log ("Ending dialogue");
	}
}
