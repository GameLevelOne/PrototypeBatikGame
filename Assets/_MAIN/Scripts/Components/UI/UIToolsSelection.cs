using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIToolsSelection : MonoBehaviour {
	public PlayerTool tool;
	
	// public Text textToolName;
	public AnimationControl animationControl;
	public Animator animator;
	public GameObject panelToolsSelection;
	public Sprite[] arrayOfToolSprites;
	public Image[] arrayOfToolImages;

	public bool isToolChange = false;
	public float showDuration;
	// public float showMultiplier;
	// public float hideMultiplier;
	
	/// <summary>
    /// <para>Values: <br /></para>
	/// <para>-1 PREV<br /></para>
	/// <para>1 NEXT<br /></para>
    /// </summary>
	public int changeIndex = 0; 

	public List<int> toolIndexes;
	public bool[] checker;
	public bool isPlayingAnimation;

	void OnEnable () {
		animationControl.OnExitAnimation += OnExitAnimation;
	}

	void OnDisable () {
		animationControl.OnExitAnimation -= OnExitAnimation;
	}

	void OnExitAnimation () {
		isPlayingAnimation = false;
	}

	public void OnClickNextToolsSelection () {
		changeIndex = 1; 
		isToolChange = true;
	}

	public void OnClickPrevToolsSelection () {
		changeIndex = -1; 
		isToolChange = true;
	}
}
