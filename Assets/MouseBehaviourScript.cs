using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseBehaviourScript : MonoBehaviour {

	public Material mat;

	private void OnMouseEnter() {
		mat.color = Color.blue;
	}

	private void OnMouseExit() {
		mat.color = Color.white;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
