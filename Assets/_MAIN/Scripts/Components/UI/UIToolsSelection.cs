using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIToolsSelection : MonoBehaviour {
	public PlayerTool tool;
	
	// public Text textToolName;
	public Sprite[] arrayOfToolSprites;
	public Image[] arrayOfToolImages;

	public bool isToolChange = false;
	
	/// <summary>
    /// <para>Values: <br /></para>
	/// <para>-1 PREV<br /></para>
	/// <para>1 NEXT<br /></para>
    /// </summary>
	public int changeIndex = 0; 

	public List<int> toolIndexes;

	public void OnClickNextToolsSelection () {
		changeIndex = 1; 
		isToolChange = true;
	}

	public void OnClickPrevToolsSelection () {
		changeIndex = -1; 
		isToolChange = true;
	}
}
