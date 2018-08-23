using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour {
	public Button btnStartGame;

	public bool isStartGame = false;

	public void OnClickStartGame () {
		isStartGame = true;
	}
}
