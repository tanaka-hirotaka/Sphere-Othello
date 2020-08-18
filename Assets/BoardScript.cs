using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardScript : MonoBehaviour {
	public int turnFlag = (int)DiskBehaviourScript.DiskColor.BLACK;

	public GameObject diskPrefab;
	public GameObject assistCubePrefab;

	private GameObject[,] field;
	private GameObject[] polarField;
	private GameObject[,] assistCube;
	public const int LONGITUDE_RANGE = 8;
	public const int LATITUDE_RANGE = 8;

	private List<GameObject> reverseList;
	private List<GameObject> reversePointList;
	private List<List<GameObject>> handLog;
	private List<string> handLogS;
	
	public void Init() {
		int i = 0;
		int j = 0;

		field = new GameObject[LONGITUDE_RANGE, LATITUDE_RANGE];
		assistCube = new GameObject[LONGITUDE_RANGE, LATITUDE_RANGE];

		for(i = 0; i < LONGITUDE_RANGE; i++) {
			for(j = 0; j < LATITUDE_RANGE; j++) {
				if(i != 0 && ( j == 0 || j == LATITUDE_RANGE - 1)) {
					field[i, j] = field[0, j];
					assistCube[i, j] = assistCube[0, j];
				}
				else {
					field[i, j] = Instantiate(diskPrefab, new Vector3(0, 0, 0), Quaternion.identity);
					field[i, j].GetComponentInChildren<DiskBehaviourScript>().Init(i, j);
					assistCube[i, j] = Instantiate(assistCubePrefab, new Vector3(0, 0, 0), Quaternion.identity);
					assistCube[i, j].GetComponentInChildren<AssistCubeScript>().Init(i, j);
				}
				//assistCube[i, j].transform.position = new Vector3(i - 3.5f, 0.75f, j - 3.5f);
				/*
				 assistCube[i, j].GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
				assistCube[i, j].GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(0, 0, 0));
				 */

			}
		}

		//石の初期配置
		field[3, 3].GetComponentInChildren<DiskBehaviourScript>().Color = (int)DiskBehaviourScript.DiskColor.BLACK;
		field[4, 4].GetComponentInChildren<DiskBehaviourScript>().Color = (int)DiskBehaviourScript.DiskColor.BLACK;
		field[3, 4].GetComponentInChildren<DiskBehaviourScript>().Color = (int)DiskBehaviourScript.DiskColor.WHITE;
		field[4, 3].GetComponentInChildren<DiskBehaviourScript>().Color = (int)DiskBehaviourScript.DiskColor.WHITE;

		reverseList = new List<GameObject>();
		reversePointList = new List<GameObject>();
		handLog = new List<List<GameObject>>();
		handLogS = new List<string>();
	}

	public int GetCountDisk(int color) {
		int counter = 0;
		int i,
			j;

		for (i = 0; i < LONGITUDE_RANGE; i++) {
			for (j = 0; j < LATITUDE_RANGE; j++) {
				if(i == 0 || (j != 0 && j != LATITUDE_RANGE - 1)) { //重複する極の座標以外
					if (field[i, j].GetComponentInChildren<DiskBehaviourScript>().Color == color) {
						counter++;
					}
				}
			}
		}

		return counter;
	}

	public bool PutDisk(int x, int y, int color) {
		if(field[x,y].GetComponentInChildren<DiskBehaviourScript>().Color != (int)DiskBehaviourScript.DiskColor.NONE) {
			return false; //その場所に石がある = 置けない
		}

		GetReverseList(x, y, color); //返せる石があるかどうか
		if (reverseList.Count == 0) {
			return false; //置けなかった
		}
		else {
			field[x, y].GetComponentInChildren<DiskBehaviourScript>().Color = color;
			foreach (GameObject item in reverseList) {
				item.GetComponentInChildren<DiskBehaviourScript>().Reverse();
			}
			SetHandLog(x, y);
			return true; //置けた
		}
	}

	public List<GameObject> GetReverseList(int x, int y, int color) {
		int i = 0;

		reverseList.Clear();

		if(y == 0) {
			for(i = 0; i < LONGITUDE_RANGE; i++) {
				ReflexReverseList(i, y, 0, 1, color);
			}
		}
		else if (y == LATITUDE_RANGE - 1) {
			for (i = 0; i < LONGITUDE_RANGE; i++) {
				ReflexReverseList(i, y, 0, -1, color);
			}
		}
		else {
			ReflexReverseList(x, y, -1, -1, color);
			ReflexReverseList(x, y, 0, -1, color);
			ReflexReverseList(x, y, 1, -1, color);
			ReflexReverseList(x, y, 1, 0, color);
			ReflexReverseList(x, y, 1, 1, color);
			ReflexReverseList(x, y, 0, 1, color);
			ReflexReverseList(x, y, -1, 1, color);
			ReflexReverseList(x, y, -1, 0, color);
		}
		
		return reverseList;
	}

	private bool ReflexReverseList(int x, int y, int dx, int dy, int color) {
		if(x + dx == -1) {
			x = LONGITUDE_RANGE - 1;
		}
		else if(x + dx == LONGITUDE_RANGE) {
			x = 0;
		}
		else {
			x += dx;
		}

		if (y + dy == -1) {
			y = 1;
			dy *= -1;
			x = (x + (LONGITUDE_RANGE / 2)) % LONGITUDE_RANGE;
		}
		else if (y + dy == LATITUDE_RANGE) {
			y = LATITUDE_RANGE - 2;
			dy *= -1;
			x = (x + (LONGITUDE_RANGE / 2)) % LONGITUDE_RANGE;
		}
		else {
			y += dy;
		}

		if(x < 0 || x >= LONGITUDE_RANGE || y < 0 || y >= LATITUDE_RANGE) {
			return false;
		}

		if (field[x, y].GetComponentInChildren<DiskBehaviourScript>().Color == color * -1) {
			if (ReflexReverseList(x, y, dx, dy, color)) {
				reverseList.Add(field[x, y]);
				return true; //この座標までの石を反転できる
			}
			else {
				return false; //石の反転はできない
			}
		}
		else if (field[x, y].GetComponentInChildren<DiskBehaviourScript>().Color == color) {
			return true; //この座標までの石を反転できる
		}
		else {
			return false; //石の反転はできない
		}
	}

	//置ける石の場所のリスト
	public List<GameObject> GetReversePointList(int color) {
		int i,
			j;

		reversePointList.Clear();

		for (i = 0; i < LONGITUDE_RANGE; i++) {
			for (j = 0; j < LATITUDE_RANGE; j++) {
				if (i == 0 || (j != 0 && j != LATITUDE_RANGE - 1)) { //重複する極の座標以外
					if (field[i, j].GetComponentInChildren<DiskBehaviourScript>().Color == (int)DiskBehaviourScript.DiskColor.NONE) {
						GetReverseList(i, j, color);
						if (reverseList.Count > 0) {
							reversePointList.Add(field[i, j]);
							foreach (Transform child in assistCube[i, j].transform) {
								child.localPosition = new Vector3(0.0f, 4.0f, 0.0f);
							}
							
							//assistCube[i, j].transform.position = new Vector3(0.0f, 4.0f, 0.0f);
						}
						else {
							foreach (Transform child in assistCube[i, j].transform) {
								child.localPosition = new Vector3(0, 0, 0);
							}
							//assistCube[i, j].transform.position = new Vector3(0, 0, 0);
						}
					}
					else {
						foreach (Transform child in assistCube[i, j].transform) {
							child.localPosition = new Vector3(0, 0, 0);
						}
						//assistCube[i, j].transform.position = new Vector3(0, 0, 0);
					}
				}
			}
		}

		return reversePointList;
	}

	private void SetHandLog(int x, int y) {
		List<GameObject> puttedField = new List<GameObject>();
		puttedField.Add(field[x, y]);
		handLog.Add(puttedField);
		handLog.Add(new List<GameObject>(reverseList));
		SetHandLogS(x, y);
		
	}

	private void SetHandLogS(int x, int y) {
		string blackHand = "先手",
			whiteHand = "後手";

		if(field[x,y].GetComponentInChildren<DiskBehaviourScript>().Color == (int)DiskBehaviourScript.DiskColor.BLACK) {
			handLogS.Add(blackHand + ":" + x.ToString() + "," + y.ToString());
		}
		else {
			handLogS.Add(whiteHand + ":" + x.ToString() + "," + y.ToString());
		}
	}

	public void SetHandLogS(string word, int color) {
		string blackHand = "先手",
			whiteHand = "後手";

		if (color == (int)DiskBehaviourScript.DiskColor.BLACK) {
			handLogS.Add(blackHand + ":" + word);
		}
		else {
			handLogS.Add(whiteHand + ":" + word);
		}
	}

	private void OnGUI() {
		int i;
		
		Rect rect = new Rect(0, (handLogS.Count - 1) * 20, 100, 20);
		GUI.color = Color.red;

		for(i = 0; i < handLogS.Count; i++) {
			GUI.Label(rect, handLogS[i]);

			rect.y -= rect.height;
		}
		
	}

	public void Undo() {
		if (handLog.Count == 0) {
			return;
		}
		
		foreach (GameObject item in handLog[handLog.Count - 1]) {
			item.GetComponentInChildren<DiskBehaviourScript>().Reverse();
		}
		handLog.RemoveAt(handLog.Count - 1);

		handLog[handLog.Count - 1][0].GetComponentInChildren<DiskBehaviourScript>().Color = (int)DiskBehaviourScript.DiskColor.NONE;
		handLog.RemoveAt(handLog.Count - 1);

		handLogS.RemoveAt(handLogS.Count - 1);

		turnFlag *= -1;
		GetReversePointList(turnFlag);
	}
}
