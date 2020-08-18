using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultSystemScript : MonoBehaviour {

	public Canvas canvas;
	public Text textPrefab;
	public Button buttonPrefab;

	private Text result;
	private Text resultBlack;
	private Text resultWhite;
	private Text resultWiner;
	private string unit = "個";

	private Button buttonToNext;

	// Use this for initialization
	void Start () {
		result = Instantiate(textPrefab, new Vector3(0, 0, 0), Quaternion.identity, canvas.transform);
		result.rectTransform.anchoredPosition = new Vector2(-50, 100);
		resultBlack = Instantiate(textPrefab, new Vector3(0, 0, 0), Quaternion.identity, canvas.transform);
		resultBlack.rectTransform.anchoredPosition = new Vector2(-100, 50);
		resultWhite = Instantiate(textPrefab, new Vector3(0, 0, 0), Quaternion.identity, canvas.transform);
		resultWhite.rectTransform.anchoredPosition = new Vector2(30, 50);
		resultWiner = Instantiate(textPrefab, new Vector3(0, 0, 0), Quaternion.identity, canvas.transform);
		resultWiner.rectTransform.anchoredPosition = new Vector2(-50, 0);
		result.text = "結果";
		resultBlack.text = "黒" + GameBoardScript.SumBlack.ToString() + unit;
		resultWhite.text = "白" + GameBoardScript.SumWhite.ToString() + unit;
		if(GameBoardScript.SumBlack > GameBoardScript.SumWhite) {
			resultWiner.text = "黒" + "の勝ち";
		}
		else if(GameBoardScript.SumWhite > GameBoardScript.SumBlack) {
			resultWiner.text = "白" + "の勝ち";
		}
		else {
			resultWiner.text = "引き分け";
		}

		buttonToNext = Instantiate(buttonPrefab, new Vector3(458, 136 - 50, 0), Quaternion.identity, canvas.transform);
		buttonToNext.GetComponent<RectTransform>().anchoredPosition = new Vector2(-150, -50);
		buttonToNext.GetComponentInChildren<Text>().text = "次へ";
		buttonToNext.GetComponentInChildren<Text>().fontSize = 40;
		buttonToNext.GetComponent<SceneChangerButtonScript>().SceneName = "Main";
	}
}
