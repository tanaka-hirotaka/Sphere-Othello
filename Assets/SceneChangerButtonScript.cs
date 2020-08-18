using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangerButtonScript : MonoBehaviour {
	
	public string SceneName { set; get; }

	public void OnClick() {
		SceneManager.LoadScene(SceneName);
	}
}
