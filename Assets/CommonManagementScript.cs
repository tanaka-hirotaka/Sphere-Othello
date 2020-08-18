using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonManagementScript : MonoBehaviour {
	/*
	 public static CommonManagementScript management;
	
	private void Awake() {
		if(management == null) {
			DontDestroyOnLoad(gameObject);
			management = this;
		}
		else {
			Destroy(gameObject);
		}
	}
	
	*/

	public static Hashtable management = new Hashtable();

	private void Awake() {
		if(management[gameObject.name] == null) {
			DontDestroyOnLoad(gameObject);
			management[gameObject.name] = this;
		}
		else {
			Destroy(gameObject);
		}
	}

	private void OnDestroy() {
		Debug.Log("deleted" + name);
	}
}
