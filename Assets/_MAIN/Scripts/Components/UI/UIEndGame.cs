using UnityEngine.UI;
using UnityEngine;

public class UIEndGame : MonoBehaviour {
	public Button buttonBackToMainMenu;
	public bool call = false;
	public bool endShow = false;
	public bool backToMainMenu = false;

	public void ButtonBackToMainMenuOnClick()
	{
		backToMainMenu = true;
	}
	
}
