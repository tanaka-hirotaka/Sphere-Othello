using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviourScript : MonoBehaviour {
	float x = -90;
	float y = 0;
	float zoomOfset = 0;
	Vector3 mousePoint;
	Vector3 dVector;

	// Use this for initialization
	void Start () {
		mousePoint = Input.mousePosition;
	}
	
	// Update is called once per frame
	void Update () {

		//ドラッグ＆スクロール処理
		if (Input.GetMouseButtonDown(0)) {
			mousePoint = Input.mousePosition;
		}
		if (Input.GetMouseButton(0)) {
			dVector = mousePoint - Input.mousePosition;
			mousePoint = Input.mousePosition;
			x += dVector.y;
			y -= dVector.x;
		}

		//ズーム処理
		foreach (Transform child in gameObject.transform) {
			zoomOfset = Input.mouseScrollDelta.y;
			if (child.localPosition.y + zoomOfset > 4 && child.localPosition.y + zoomOfset < 16) {
				child.localPosition += new Vector3(0.0f, zoomOfset, 0.0f);
			}
		}

		x += Input.GetAxis("Vertical");
		y += Input.GetAxis("Horizontal");
		if (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(1)) {
			x = 0;
			y = 0;
			foreach (Transform child in gameObject.transform) {
				child.localPosition = new Vector3(0.0f, 8.0f, 0.0f);
			}
		}
		transform.rotation = Quaternion.Euler(x, y, 0);
    }
}
