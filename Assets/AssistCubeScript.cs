using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssistCubeScript : MonoBehaviour {

	public GameBoardScript gameMgr;

	public int XPoint { private set; get; }
	public int YPoint { private set; get; }

	private void Awake() {
		gameMgr = GameObject.Find("EmptyObject for Board").GetComponent<GameBoardScript>();
	}
	private void OnMouseDown() {
		gameMgr.TurnAction(XPoint, YPoint);
	}
	public void Init(int x, int y) {
		if (x < 0 || x >= BoardScript.LONGITUDE_RANGE || y < 0 || y >= BoardScript.LATITUDE_RANGE) {
			Debug.Log("AssistCube Init false");
		}
		XPoint = x;
		YPoint = y;
		transform.root.gameObject.transform.rotation = Quaternion.Euler((YPoint - 3) * 22.5f + 90 - 22.5f * (BoardScript.LATITUDE_RANGE - YPoint) / 8, XPoint * 45f + 22.5f, 0.0f);
	}
}
