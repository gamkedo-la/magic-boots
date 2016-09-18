using UnityEngine;
using System.Collections;

public class CloudBrain : MonoBehaviour {
	private GameObject myTreasure = null;
	public enum BumpKind {empty,monster,treasure};
	public bool hasPlayer = false;

	public static bool enemiesAwake = false;

	ParticleSystem[] emittersToFade;

	ParticleSystem.MinMaxGradient colorGrad;

	public GameObject sleepingEnemy;


	void Start() {
		if(Random.Range(0.0f, 1.0f) < CloudGenerator.instance.treasureSpawnOdds) {
			AddTreasure();
		}
		emittersToFade = GetComponentsInChildren<ParticleSystem>();
	}

	void Update() {
		if(enemiesAwake && sleepingEnemy && sleepingEnemy.activeSelf==false) {
			sleepingEnemy.SetActive(true);
		}

		Color fadedCol = Color.white;
		float camDist = Vector3.Distance(transform.position, Camera.main.transform.position);

		fadedCol.a = 1.0f-(camDist / CloudGenerator.instance.maxDist());
		// fadedCol.a *= fadedCol.a;
		colorGrad = new ParticleSystem.MinMaxGradient(fadedCol);

		for(int i = 0; i < emittersToFade.Length; i++) {
			// emittersToFade[i].startColor = fadedCol;
			ParticleSystem.ColorOverLifetimeModule overLifetime = emittersToFade[i].colorOverLifetime;
			overLifetime.color = colorGrad;
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

		if(myTreasure.GetComponent<MonsterLeapAI>()) {
			sleepingEnemy = myTreasure;
			if(enemiesAwake == false) {
				sleepingEnemy.SetActive(false);
			}
		}
	}

	public Vector3 myTopPt() {
		return transform.position + Vector3.up * CloudGenerator.heightAboveCloudTreasure * transform.lossyScale.z -
			Vector3.up * 62.0f;
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

	public BumpKind ClearTreasure () { // returns true if it was a monster
		if(myTreasure) {
			MonsterLeapAI mlaiScript = myTreasure.GetComponent<MonsterLeapAI>();
			Destroy(myTreasure);
			if(mlaiScript != null) {
				// Debug.Log("Removed monster!");
				return BumpKind.monster;
			} else {
				// Debug.Log("Removed treasure!");
				return BumpKind.treasure;
			}
		}
		return BumpKind.empty;
	}
}
