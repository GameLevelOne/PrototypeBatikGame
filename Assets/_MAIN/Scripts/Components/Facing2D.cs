using UnityEngine;

public class Facing2D : MonoBehaviour {
	public Transform[] attackDirections;
	public GameObject attackArea;
	public GameObject attackDirParent;

	int CurDirID = 0;

	void Awake () {
		attackDirections = attackDirParent.GetComponentsInChildren<Transform>();
		
		attackArea.transform.position = attackDirections[1].position;
	}

	public int DirID {
		get {return CurDirID;}
		set {
			if (value == 0) return;
			
			CurDirID = value;
			attackArea.transform.position = attackDirections[CurDirID].position;
		}
	}
}
