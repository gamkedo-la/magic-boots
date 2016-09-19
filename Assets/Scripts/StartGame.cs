using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour {
	// Use this for initialization
	public void LoadScene () {
		Time.timeScale = 1.0f;
		SceneManager.LoadScene(0);
	}
}
