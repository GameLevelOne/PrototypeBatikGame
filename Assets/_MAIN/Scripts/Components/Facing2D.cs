using UnityEngine;

public class Facing2D : MonoBehaviour {
	public Transform[] attackDirections;
	public Transform[] weakDirections = new Transform[]{};
	public GameObject attackArea;
	public GameObject blindArea;
	public GameObject attackDirParent;

	int CurDirID = 0;

	void Awake () {
		attackDirections = attackDirParent.GetComponentsInChildren<Transform>();

		// weakDirections = attackDirections;
		
		attackArea.transform.position = attackDirections[1].position;
		blindArea.transform.position = attackDirections[5].position;
	}

	/// <summary>
    /// <para>Values: <br /></para>
	/// <para>1. Bottom<br /></para>
	/// <para>2. Bottom Left<br /></para>
	/// <para>3. Left<br /></para>
	/// <para>4. TopLeft<br /></para>
	/// <para>5. Top<br /></para>
	/// <para>6. Top Right<br /></para>
	/// <para>7. Right<br /></para>
	/// <para>8. BottomRight<br /></para>
    /// </summary>
	public int DirID {
		get {return CurDirID;}
		set {
			if (value == 0) return;
			
			CurDirID = value;
			attackArea.transform.position = attackDirections[CurDirID].position;

			int tempDirID = CurDirID;
			if (tempDirID >= 4 ) {
				blindArea.transform.position = attackDirections[tempDirID - 4].position;
			} else {
				blindArea.transform.position = attackDirections[tempDirID + 4].position;
			}
		}
	}
}
