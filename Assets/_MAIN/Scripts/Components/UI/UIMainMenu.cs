using UnityEngine;
using UnityEngine.UI;

public enum UITitleState {
	NONE,
	NEWGAME,
	CONTINUE
}

public class UIMainMenu : MonoBehaviour {
	public UIFader fader;
	public Button btnStartGame;
	public Button btnContinue;

	public bool init = false;
	public UITitleState titleState = UITitleState.NONE;

	public void OnClickStartGame ()
	{
		titleState = UITitleState.NEWGAME;
		DisableButtons();
	}

	public void OnClickContinue()
	{
		titleState = UITitleState.CONTINUE;
		DisableButtons();
	}

	void DisableButtons() {
		btnStartGame.interactable = false;
		btnContinue.interactable = false;
	}
}
