using UnityEngine;
using System.Collections;

public class CloudBrain : MonoBehaviour {
	private GameObject myTreasure = null;

	void Start() {
		if(Random.Range(0.0f, 1.0f) < CloudGenerator.instance.treasureSpawnOdds) {
			AddTreasure();
		}
	}

	public void AddTreasure () {
		if(CloudGenerator.instance.treasurePrefabs.Length == 0) {
			return;
		}

		int cloudIndex = Random.Range(0, CloudGenerator.instance.treasurePrefabs.Length);
		myTreasure = (GameObject)Instantiate(CloudGenerator.instance.treasurePrefabs[cloudIndex]);

		myTreasure.transform.Rotate(Vector3.up, Random.Range(0.0f,360.0f));
		myTreasure.transform.position = myTopPt();
		myTreasure.transform.parent = transform;
	}

	public Vector3 myTopPt() {
		return transform.position + Vector3.up * CloudGenerator.heightAboveCloudTreasure * transform.lossyScale.z;
	}

	public bool hasTreasure() {
		return (myTreasure != null);
	}

	public void assignTreasure(GameObject assignThis) {
		myTreasure = assignThis;
		myTreasure.transform.parent = transform;
	}
	public void forgetTreasure() {
		myTreasure = null;
	}

	public void ClearTreasure () {
		if(myTreasure) {
			Destroy(myTreasure);
			Debug.Log("Got treasure!");
		}
	}
}
