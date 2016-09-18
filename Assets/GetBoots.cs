using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GetBoots : MonoBehaviour {
	public Transform boots;
	public bool isMoving = false;
	
	// Update is called once per frame
	void Update () {
		if(isMoving) {
			Vector3 goTo = Camera.main.transform.position - boots.transform.position;
			boots.transform.position += goTo * Time.deltaTime * 0.2f;
			if(goTo.magnitude < 200.0f) {
				SceneManager.LoadScene(1);
			}
		}
	}

	public void StartGettingBoots() {
		isMoving = true;
	}
}
