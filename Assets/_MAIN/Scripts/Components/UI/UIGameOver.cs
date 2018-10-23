using UnityEngine;
using UnityEngine.UI;

public class UIGameOver : MonoBehaviour {
	public CanvasGroup gameOverPanel;
	public Animator gameOverAnim;
	public Image[] menuButtons;
	public Sprite[] spriteNormal;
	public Sprite[] spritePressed;
	public Sprite[] spriteSelect;
	public GameObject panel;

	[HeaderAttribute("Current")]
	public int btnIndex;
	public bool isInitialized = false;
	public bool isDoingStuff = false;
	public bool isActionPressed = false;
	public bool call = false;
	public bool endShow = false;
	public bool endHide = false;
	public bool doRestart = false;
	public bool doReturnToTitle = false;
	public bool isUpPressed = false;
	public bool isDownPressed = false;
	[HeaderAttribute("Sound")]
	public AudioSource uiAudio;
	public AudioClip selectClip;
	public AudioClip chooseClip;


	#region animation event
	void EndShowing()
	{
		// Debug.Log("ENDHOWING CALLED");
		endShow = true;
		Time.timeScale = 0f;
	}
	void EndHiding()
	{
		endHide = true;
	}
	#endregion
}
