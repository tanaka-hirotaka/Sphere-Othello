using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameBoardScript : MonoBehaviour {
	BoardScript gameBoard;
	public GameObject board;
	
	public Text textPrefab;
	public Button buttonPrefab;
	public Button buttonUnDoPrefab;
	public Canvas canvas;

	public static int SumBlack { private set; get; }
	public static int SumWhite { private set; get; }

	private Text textTurn;
	private Text textPass;
	private Button buttonToNext;
	private Button buttonUnDo;

	bool runFlag = true;
	
	
	string turn1 = "1P turn";
	string turn2 = "2P turn";
	string pass = "PASS";

	// Use this for initialization
	void Start () {
		gameBoard = GetComponent<BoardScript>();
		textTurn = Instantiate(textPrefab, new Vector3(0, 0, 0), Quaternion.identity, canvas.transform);
		textPass = Instantiate(textPrefab, new Vector3(0, 0, 0), Quaternion.identity, canvas.transform);
		
		textTurn.text = turn1;
		//textTurn.fontSize = 40;
		textTurn.rectTransform.anchoredPosition = new Vector2(-100, 0);
		//textTurn.rectTransform.anchorMax = new Vector2(1, 0.5f);
		//textTurn.rectTransform.anchorMin = new Vector2(1, 0.5f);
		//textTurn.rectTransform.sizeDelta = new Vector2(textTurn.fontSize * textTurn.text.Length + 5, textTurn.fontSize + 5);
		textPass.text = pass;
		textPass.rectTransform.anchoredPosition = new Vector2(-100, 60);
		textPass.color = new Color(0, 0, 0, 0);

		buttonUnDo = Instantiate(buttonUnDoPrefab, new Vector3(0, 0, 0), Quaternion.identity, canvas.transform);
		buttonUnDo.GetComponent<RectTransform>().anchoredPosition = new Vector2(-150, -100);
		buttonUnDo.GetComponentInChildren<Text>().text = "1手戻す";
		buttonUnDo.GetComponentInChildren<Text>().fontSize = 40;
		buttonUnDo.onClick.AddListener(() => gameBoard.Undo());
		//ゲーム版の作成
		Instantiate(board);

		gameBoard.Init();
		gameBoard.GetReversePointList(gameBoard.turnFlag);
	}

	// Update is called once per frame
	void Update () {
		if(runFlag) {
			switch (gameBoard.turnFlag) {
				case (int)DiskBehaviourScript.DiskColor.BLACK:
					textTurn.text = turn1;
					break;
				case (int)DiskBehaviourScript.DiskColor.WHITE:
					textTurn.text = turn2;
					break;
				default:
					break;
			}
		}
	}

	public void TurnAction(int x, int y) {
		if (x < 0 || x >= BoardScript.LONGITUDE_RANGE || y < 0 || y >= BoardScript.LATITUDE_RANGE) {
			return;
		}

		if (gameBoard.PutDisk(x, y, gameBoard.turnFlag)) {
			if (gameBoard.GetReversePointList(gameBoard.turnFlag * -1).Count == 0) {
				if (gameBoard.GetReversePointList(gameBoard.turnFlag).Count == 0) {
					//終了処理
					SumBlack = gameBoard.GetCountDisk((int)DiskBehaviourScript.DiskColor.BLACK);
					SumWhite = gameBoard.GetCountDisk((int)DiskBehaviourScript.DiskColor.WHITE);

					buttonToNext = Instantiate(buttonPrefab, new Vector3(458, 136 - 50, 0), Quaternion.identity, canvas.transform);
					buttonToNext.GetComponent<RectTransform>().anchoredPosition = new Vector2(-150, -50);
					buttonToNext.GetComponentInChildren<Text>().text = "次へ";
					buttonToNext.GetComponentInChildren<Text>().fontSize = 40;
					buttonToNext.GetComponent<SceneChangerButtonScript>().SceneName = "Result";
					runFlag = false;
				}
				else { //パスのとき
					textPass.color = Color.black;
					gameBoard.SetHandLogS(textPass.text, gameBoard.turnFlag * -1);
				}
			}
			else {
				//ターンチェンジ処理
				textPass.color = new Color(0, 0, 0, 0);
				gameBoard.turnFlag *= -1;

			}
		}
		
	}
}
