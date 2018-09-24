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
	public Button btnStartGame;
	public Button btnContinue;

	public BoxCollider portal11;
	public Portal nextPortal;

	public bool init = false;
	// public UITitleState titleState = UITitleState.NONE;

	void Start() {
		// PlayerPrefs.DeleteAll();
		// btnStartGame.Select();
		if(PlayerPrefs.HasKey(Constants.PlayerPrefKey.LEVEL_CURRENT)){
			btnContinue.gameObject.SetActive(true);
		}else{
			btnContinue.gameObject.SetActive(false);
		}
	}

	public void OnClickStartGame ()
	{
		// titleState = UITitleState.NEWGAME;
		// OpenScene("SceneLevel_1-1");
		// StartCoroutine(LoadingScene("SceneLevel_1-1"));	
		PlayerPrefs.DeleteAll();	
		portal11.enabled = true;
		DisableButtons();
	}

	public void OnClickContinue()
	{
		// titleState = UITitleState.CONTINUE;
		// 	OpenScene(PlayerPrefs.GetString(Constants.PlayerPrefKey.LEVEL_CURRENT));
		nextPortal.sceneDestination = PlayerPrefs.GetString(Constants.PlayerPrefKey.LEVEL_CURRENT);
		nextPortal.GetComponent<BoxCollider>().enabled = true;
		DisableButtons();
	}

	void DisableButtons() {
		btnStartGame.interactable = false;
		btnContinue.interactable = false;
	}
	void OpenScene (string sceneName) {
		Debug.Log("Trying to load scene: "+sceneName);
		StartCoroutine(LoadingScene(sceneName));
	}


	IEnumerator LoadingScene(string sceneName) {
		Debug.Log("YIIIIHAAAA!!!!!");
		yield return null;
		AsyncOperation asop = SceneManager.LoadSceneAsync(sceneName);
		asop.allowSceneActivation = false;
		while (asop.progress<0.9f) {
			Debug.Log("Loading Progress..."+(asop.progress * 100) + "%");
			yield return null;
		}
		asop.allowSceneActivation = true;
		yield return null;

	}


}
