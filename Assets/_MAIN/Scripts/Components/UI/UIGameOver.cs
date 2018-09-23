using UnityEngine;
using UnityEngine.UI;

public class UIGameOver : MonoBehaviour {
	public CanvasGroup gameOverPanel;
	public Animator gameOverAnim;
	public Image buttonRestart;
	public Image buttonReturnToTitle;
	public GameObject black;

	[HeaderAttribute("Current")]
	public int selected = 0;
	public bool call = false;
	public bool endShow = false;
	public bool endHide = false;
	public bool doRestart = false;
	public bool doReturnToTitle = false;

	public void ButtonRestartOnClick()
	{
		doRestart = true;
	}
	public void ButtonReturnToTitleOnClick()
	{
		doReturnToTitle = true;
	}

	#region animation event
	void EndShowing()
	{
		endShow = true;
	}
	void EndHiding()
	{
		endHide = true;
	}
	#endregion
}
