using UnityEngine;
using System.Collections;

public class MonsterLeapAI : MonoBehaviour {
	float minDist = 1000.0f;
	int layerMask;
	float jumpHeightStretch = 750.0f;
	public static bool seekPlayer = false;

	// Use this for initialization
	void Start () {
		layerMask = LayerMask.GetMask("Cloud");
		StartCoroutine(PickTarget());
	}
	
	// Update is called once per frame
	IEnumerator PickTarget () {
		while(true) {
			yield return new WaitForSeconds( Random.Range(0.2f,1.0f));
			Collider[] nearBy = Physics.OverlapSphere(transform.position,9000.0f,layerMask);
			if(nearBy.Length > 0) {
				int whichCloud = Random.Range(0, nearBy.Length);

				if(seekPlayer) {
					for(int i = 0; i < nearBy.Length; i++) {
						CloudBrain cbScriptTest =
							nearBy[i].gameObject.GetComponentInParent<CloudBrain>();
						if(cbScriptTest && cbScriptTest.hasPlayer) {
							whichCloud = i;
							break;
						}
					}
				}

				CloudBrain cbScript = nearBy[whichCloud].gameObject.GetComponentInParent<CloudBrain>();
				float distTo = Vector3.Distance(nearBy[whichCloud].transform.position, transform.position);
				bool hasScript = (cbScript!=null);
				bool hasTreasureAlready = (cbScript && cbScript.hasTreasure());
				if(hasScript && hasTreasureAlready==false) {
					Vector3 startedAt = transform.position;
					if(transform.parent == null) {
						break; // test prefab, no cloud, turn off its ai
					}
					CloudBrain comingFrom = transform.parent.GetComponent<CloudBrain>();
					comingFrom.forgetTreasure();
					cbScript.assignTreasure(gameObject);
					for(float f = 0f; f < 50.0f; f+=1.0f) {
						transform.position = 
							Vector3.Lerp(startedAt,
								cbScript.myTopPt(),f/50.0f);

						// transform.LookAt(cbScript.myTopPt());
						transform.rotation = Quaternion.Slerp(
							transform.rotation,
							Quaternion.LookRotation(
								cbScript.myTopPt() - transform.position),
							Time.deltaTime * 2.0f);
						

						float interpPerc = f/50.0f;
						if(interpPerc < 1.0f) {
							float heightBoostPerc;
							float heightTempToSQ = (interpPerc - 0.5f)*2.0f;
							heightBoostPerc = -(heightTempToSQ * heightTempToSQ) + 1;
							transform.position += 
								Vector3.up * heightBoostPerc * jumpHeightStretch;
						}

						yield return new WaitForSeconds( 0.025f );
					}
				} else {
					// Debug.Log("didn't jump because: " + hasScript + " / " + hasTreasureAlready);
				}
			} else {
				Debug.Log("no cloud in range to jump to, vanishing");
				Destroy(gameObject);
			}
		}
	}
}
