using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HelpToggle : MonoBehaviour {
	public Text helpText;
	bool showingHelp = false;
	// Use this for initialization
	void Start () {
		helpText = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.H)) {
			showingHelp = !showingHelp;
			if(showingHelp) {
				helpText.text = "Mouse Click or Spacebar to move\nGather your toys! Avoid nightmares!";
			} else {
				helpText.text = "Press H for Help";
			}
		}
	}
}
