using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DiskBehaviourScript : MonoBehaviour {
	public enum DiskColor {
		BLACK = -1,
		NONE,
		WHITE,
		WALL
	}

	public int XPoint { private set; get; }
	public int YPoint { private set; get; }

	private int color;
	public int Color {
		set {
			color = value;
			switch (value) {
				case (int)DiskColor.BLACK:
					
					transform.localPosition = new Vector3(0.0f, 4.0f, 0.0f);
					
					break;
				case (int)DiskColor.WHITE:
					
					transform.localPosition = new Vector3(0.0f, 4.0f, 0.0f);
					transform.Rotate(180, 0, 0);
					transform.localPosition += new Vector3(0, 0.05f, 0);
					break;
				default:
					transform.localPosition = new Vector3(0, 0, 0);
					break;
			}
		}

		get {
			return color;
		}
	}
	
	public void Reverse() {
		color = Color * -1;
		transform.Rotate(180, 0, 0);
		transform.localPosition += new Vector3(0, 0.05f * Color, 0);
	}

	public void Init(int x, int y) {
		Color = (int)DiskColor.NONE;
		if(x < 0 || x >= BoardScript.LONGITUDE_RANGE || y < 0 || y >= BoardScript.LATITUDE_RANGE) {
			Debug.Log("Disk Init false");
		}
		XPoint = x;
		YPoint = y;
		transform.root.gameObject.transform.rotation = Quaternion.Euler((YPoint - 3) * 22.5f + 90 - 22.5f * (BoardScript.LATITUDE_RANGE - YPoint) / 8, XPoint * 45f + 22.5f, 0.0f);
	}
}
