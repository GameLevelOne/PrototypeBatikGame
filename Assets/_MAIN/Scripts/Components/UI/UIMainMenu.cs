using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

// public enum UITitleState {
// 	NONE,
// 	NEWGAME,
// 	CONTINUE
// }

public class UIMainMenu : MonoBehaviour {
	public Image[] menuButtons;
	public Sprite[] spriteNormal;
	public Sprite[] spritePressed;
	public Sprite[] spriteSelect;

	public BoxCollider portal11;
	public Portal nextPortal;

	public bool isInitialized = false;
	public bool isDoingStuff = false;
	public bool isActionPressed = false;
	public bool isUpPressed = false;
	public bool isDownPressed = false;

	public int btnIndex;
	// public UITitleState titleState = UITitleState.NONE;

}
