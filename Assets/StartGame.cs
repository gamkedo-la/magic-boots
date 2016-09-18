using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour {
	// Use this for initialization
	public void LoadScene () {
		SceneManager.LoadScene(0);
	}
}
