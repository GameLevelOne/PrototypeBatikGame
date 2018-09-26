using UnityEngine.UI;
using UnityEngine;

public class UIEndGame : MonoBehaviour {
	public UIFader uiFader;
	public bool call = false;
	public bool endShow = false;
	public bool backToMainMenu = false;

	void OnEndShowing()
	{
		endShow = true;
	}	
}
