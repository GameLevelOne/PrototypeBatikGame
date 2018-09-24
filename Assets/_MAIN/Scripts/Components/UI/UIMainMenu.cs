using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour {
	public Button btnStartGame;
	public Button btnContinue;

	public bool init = false;
	public bool isStartGame = false;
	public bool isContinue = true;

	public void OnClickStartGame ()
	{
		isStartGame = true;
	}

	public void OnClickContinue()
	{
		isContinue = true;
	}
}
