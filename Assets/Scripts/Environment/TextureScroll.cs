using UnityEngine;
using System.Collections;

public class TextureScroll : MonoBehaviour {
	private Material whichMat;

	// Use this for initialization
	void Start () {
		Renderer myRend = GetComponent<Renderer>();
		whichMat = myRend.material;
	}
	
	// Update is called once per frame
	void Update () {
		float slideBy = Time.realtimeSinceStartup * 0.01f;
		whichMat.SetTextureOffset("_DetailAlbedoMap", new Vector2(
			0,-slideBy));
	}
}