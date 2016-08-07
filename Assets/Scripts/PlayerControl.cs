using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerControl : MonoBehaviour {
	public GameObject cloudJumpReady;
	public GameObject cloudJumpTooFar;

	float playerJumpRange = 20000f;

	float heightAboveCloud = 2.6f; // how high above center to "perch" on it (affect by cloud scale?)
	float jumpHeightStretch = 1250.0f;
		
	int cloudLayerMask;
	int cloudLayer;

	float turnLat;
	float turnLong;

	float driftTime = 1.5f;
	Vector3 cameFrom;
	Vector3 goTo;
	float destTime = 0.0f;

	GameObject recentCloud = null;

	// Use this for initialization
	void Start () {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		cloudLayer = LayerMask.NameToLayer("Cloud");
		cloudLayerMask = LayerMask.GetMask("Cloud");

		cameFrom = goTo = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit rhInfo;

		bool showCloudJump;
		if(Physics.Raycast(transform.position, transform.forward, out rhInfo, playerJumpRange, cloudLayerMask)) {
			// Debug.Log(rhInfo.collider.name);
			showCloudJump = true;
		} else {
			showCloudJump = false;
		}

		if(destTime > Time.realtimeSinceStartup) {
			float interpPerc = 1.0f - (destTime - Time.realtimeSinceStartup) / driftTime;
			if(interpPerc >= 1.0f) {
				destTime = 0.0f;
				transform.position = recentCloud.transform.position + goTo;
			} else {
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
				goTo = Vector3.up * heightAboveCloud * recentCloud.transform.lossyScale.z;
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
		turnLong += Input.GetAxis("Mouse X") * 120.0f * Time.deltaTime;
		turnLat += -Input.GetAxis("Mouse Y") * 120.0f * Time.deltaTime;

		turnLat = Mathf.Clamp(turnLat, -55.0f, 55.0f);

		transform.rotation = Quaternion.identity *
		Quaternion.AngleAxis(turnLong, Vector3.up) *
		Quaternion.AngleAxis(turnLat, Vector3.right);
	}
}
