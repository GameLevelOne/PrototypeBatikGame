using UnityEngine;

public class Facing2D : MonoBehaviour {
	public Transform[] attackDirections;
	public bool isNotPlayerNorEnemy = false;
	public GameObject attackArea;
	public GameObject blindArea;
	public GameObject attackDirParent;
	
	[HeaderAttribute("1 Down, 2 Left, 3 Up, 4 Right")]
	public int initFacingDirID = 1;

	[HeaderAttribute("Current")]
	[SerializeField] int curDirID;

	void Awake () {
		if (isNotPlayerNorEnemy) return;

		if (attackDirParent != null) {
			attackDirections = attackDirParent.GetComponentsInChildren<Transform>();
		}

// 		#region 4 Direction
// 		SetAreaPos (attackArea, 1);
// 		SetAreaPos (blindArea, 3);
// 		#endregion

// #region 8 Direction
// 		// attackArea.transform.position = attackDirections[1].position;
// 		// blindArea.transform.position = attackDirections[5].position;
// #endregion
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
		get {return curDirID;}
		set {
			if (value == 0) return;
			
			curDirID = value;
			
			if (isNotPlayerNorEnemy) return;

			SetAreaPos (attackArea, curDirID);

			int tempDirID = curDirID;
			if (tempDirID >= 3 ) {
				SetAreaPos (blindArea, tempDirID - 2);
			} else {
				SetAreaPos (blindArea, tempDirID + 2);
			}
		}
	}
	#endregion

	void SetAreaPos (GameObject areaObj, int posIndex) {
		if (areaObj != null) {
			areaObj.transform.position = attackDirections[posIndex].position;
		}
	}

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
