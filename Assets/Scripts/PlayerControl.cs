using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerControl : MonoBehaviour {
	public GameObject cloudJumpReady;
	public GameObject cloudJumpTooFar;
	public AudioSource nightmareSound;

	float playerJumpRange = 20000f;

	float jumpHeightStretch = 1250.0f;
		
	int cloudLayerMask;
	int cloudLayer;

	float turnLat;
	float turnLong;

	float driftTime = 1.5f;
	Vector3 cameFrom;
	Vector3 goTo;
	float destTime = 0.0f;

	public GameObject recentCloud = null;
	public MonoBehaviour colorCorrectionNightmare;
	bool alreadyCrazy = false;
	bool dead = false;
	public GameObject gameOverMsg;
	public Text scoreMsg;
	int scoreNow;
	public int dangerTimeSec = 12;

	// Use this for initialization
	void Start () {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		cloudLayer = LayerMask.NameToLayer("Cloud");
		cloudLayerMask = LayerMask.GetMask("Cloud");

		cameFrom = goTo = transform.position;
		scoreNow = 0;
	}

	IEnumerator CrazyMode() {
		if(alreadyCrazy == false) {
			alreadyCrazy = true;
			nightmareSound.Play();
			colorCorrectionNightmare.enabled = true;
			Time.timeScale = 3.0f;

			for(int sec = 0; sec < dangerTimeSec; sec++) {
				int timeLeft = dangerTimeSec-sec;
				scoreMsg.text = ""+scoreNow + " (" + timeLeft+" sec)";
				yield return new WaitForSeconds(Time.timeScale);
			}

			colorCorrectionNightmare.enabled = false;
			Time.timeScale = 1.0f;
			alreadyCrazy = false;
		} else if(dead == false) {
			dead = true;
			scoreMsg.text = ""+scoreNow;
			gameOverMsg.SetActive(true);
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit rhInfo;

		if(Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit();
		}

		if(dead) {
			return;
		}

		bool showCloudJump;
		if(Physics.Raycast(transform.position, transform.forward, out rhInfo, playerJumpRange, cloudLayerMask)) {
			// Debug.Log(rhInfo.collider.name);
			showCloudJump = true;
		} else {
			showCloudJump = false;
		}

		if(destTime > Time.realtimeSinceStartup) {
			float interpPerc = 1.0f - (destTime - Time.realtimeSinceStartup) / driftTime;
			if(interpPerc < 1.0f) {
				float heightBoostPerc;
				float heightTempToSQ = (interpPerc - 0.5f)*2.0f;
				heightBoostPerc = -(heightTempToSQ * heightTempToSQ) + 1;
				Camera.main.fieldOfView = 50 + 22 * heightBoostPerc;
				transform.position = Vector3.Lerp(cameFrom, 
					recentCloud.transform.position + goTo, interpPerc) +
								Vector3.up * heightBoostPerc * jumpHeightStretch;
			}
			showCloudJump = false;
		} else if(recentCloud) {
			CloudBrain cbScript = recentCloud.GetComponentInParent<CloudBrain>();
			if(cbScript) {
				CloudBrain.BumpKind treasureReturnKind = cbScript.ClearTreasure();
				switch(treasureReturnKind) {
				case CloudBrain.BumpKind.monster:
					StartCoroutine(CrazyMode());
					break;
				case CloudBrain.BumpKind.treasure:
					scoreNow++;
					scoreMsg.text = ""+scoreNow;
					break;
				}
						
			}
			transform.position = recentCloud.transform.position + goTo;
		}

		if(showCloudJump) {
			if(cloudJumpReady.activeSelf == false) {
				cloudJumpReady.SetActive(true);
				cloudJumpTooFar.SetActive(false);
			}

			if( Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) ) {
				if(recentCloud != null) {
					recentCloud.layer = cloudLayer;
				}
				recentCloud = rhInfo.collider.gameObject;
				goTo = Vector3.up * CloudGenerator.heightAboveCloud * recentCloud.transform.lossyScale.z;
				recentCloud.layer = 0;
				cameFrom = transform.position;
				destTime = Time.realtimeSinceStartup + driftTime;
			}

		} else {
			if(cloudJumpReady.activeSelf) {
				cloudJumpReady.SetActive(false);
				cloudJumpTooFar.SetActive(false);
			}
		}

		if(showCloudJump == false && destTime < Time.realtimeSinceStartup) {
			if(Physics.Raycast(transform.position, transform.forward, out rhInfo, 401000.0f, cloudLayerMask)) {
				if(cloudJumpTooFar.activeSelf == false) {
					cloudJumpTooFar.SetActive(true);
				}
			} else {
				if(cloudJumpTooFar.activeSelf) {
					cloudJumpTooFar.SetActive(false);
				}
			}
		}

		/*transform.position += transform.forward * 120.0f * Time.deltaTime * Input.GetAxis("Vertical");
		transform.position += transform.right * 100.0f * Time.deltaTime * Input.GetAxis("Horizontal");
		transform.position += Vector3.up * 100.0f * Time.deltaTime * Input.GetAxis("RiseFall");*/
		turnLong += Input.GetAxis("Mouse X") * 120.0f * Time.deltaTime / Time.timeScale;
		turnLat += -Input.GetAxis("Mouse Y") * 120.0f * Time.deltaTime / Time.timeScale;

		turnLat = Mathf.Clamp(turnLat, -55.0f, 55.0f);

		transform.rotation = Quaternion.identity *
		Quaternion.AngleAxis(turnLong, Vector3.up) *
		Quaternion.AngleAxis(turnLat, Vector3.right);
	}
}
