using UnityEngine;
using System.Collections;

public class CloudBrain : MonoBehaviour {
	private GameObject myTreasure = null;

	void Start() {
		AddTreasure();
	}

	public void AddTreasure () {
		int cloudIndex = Random.Range(0, CloudGenerator.instance.treasurePrefabs.Length);
		myTreasure = (GameObject)Instantiate(CloudGenerator.instance.treasurePrefabs[cloudIndex]);

		myTreasure.transform.Rotate(Vector3.up, Random.Range(0.0f,360.0f));
		myTreasure.transform.position = transform.position +
			Vector3.up * CloudGenerator.heightAboveCloudTreasure * transform.lossyScale.z;
		myTreasure.transform.parent = transform;
	}

	public void ClearTreasure () {
		if(myTreasure) {
			Destroy(myTreasure);
			Debug.Log("Got treasure!");
		}
	}
}
