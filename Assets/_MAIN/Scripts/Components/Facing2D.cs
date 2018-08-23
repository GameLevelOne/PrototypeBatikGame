using UnityEngine;

public class Facing2D : MonoBehaviour {
	public Transform[] attackDirections;
	public bool isNotPlayerNorEnemy = false;
	public GameObject attackArea;
	public GameObject blindArea;
	public GameObject attackDirParent;

	int CurDirID = 1;

	void Awake () {
		if (isNotPlayerNorEnemy) return;

		attackDirections = attackDirParent.GetComponentsInChildren<Transform>();

		#region 4 Direction
		if (attackArea != null) {
			attackArea.transform.position = attackDirections[1].position;
		}
		
		blindArea.transform.position = attackDirections[3].position;
		#endregion

		#region 8 Direction
		// attackArea.transform.position = attackDirections[1].position;
		// blindArea.transform.position = attackDirections[5].position;
		#endregion
	}

	#region 4 Direction
	/// <summary>
    /// <para>Values: <br /></para>
	/// <para>1. Bottom<br /></para>
	/// <para>2. Left<br /></para>
	/// <para>3. Top<br /></para>
	/// <para>4. Right<br /></para>
	/// </summary>
	public int DirID {
		get {return CurDirID;}
		set {
			if (value == 0) return;
			
			CurDirID = value;
			
			if (isNotPlayerNorEnemy) return;

			if (attackArea != null) {
				attackArea.transform.position = attackDirections[CurDirID].position;
			}

			int tempDirID = CurDirID;
			if (tempDirID >= 3 ) {
				blindArea.transform.position = attackDirections[tempDirID - 2].position;
			} else {
				blindArea.transform.position = attackDirections[tempDirID + 2].position;
			}
		}
	}
	#endregion

	#region 8 Direction
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
	// public int DirID {
	// 	get {return CurDirID;}
	// 	set {
	// 		if (value == 0) return;
			
	// 		CurDirID = value;
			
	// 		if (isNotPlayerNorEnemy) return;

	// 		attackArea.transform.position = attackDirections[CurDirID].position;

	// 		int tempDirID = CurDirID;
	// 		if (tempDirID >= 5 ) {
	// 			blindArea.transform.position = attackDirections[tempDirID - 4].position;
	// 		} else {
	// 			blindArea.transform.position = attackDirections[tempDirID + 4].position;
	// 		}
	// 	}
	// }
	#endregion
}
