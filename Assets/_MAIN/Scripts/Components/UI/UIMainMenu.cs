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
	public GameObject canvas;
	public UIFader fader;
	public Button btnStartGame;
	public Button btnContinue;

	public bool init = false;
	// public UITitleState titleState = UITitleState.NONE;

	void Start() {
		PlayerPrefs.DeleteAll();
		btnStartGame.Select();
		if(PlayerPrefs.HasKey(Constants.PlayerPrefKey.LEVEL_CURRENT)){
			btnContinue.gameObject.SetActive(true);
		}else{
			btnContinue.gameObject.SetActive(false);
		}
	}

	public void OnClickStartGame ()
	{
		// titleState = UITitleState.NEWGAME;
		OpenScene(Constants.SceneName.SCENE_LEVEL_1);
		DisableButtons();
	}

	public void OnClickContinue()
	{
		// titleState = UITitleState.CONTINUE;
		// 	OpenScene(PlayerPrefs.GetString(Constants.PlayerPrefKey.LEVEL_CURRENT));
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
		yield return null;
		AsyncOperation asop = SceneManager.LoadSceneAsync(sceneName);
		while (!asop.isDone) {
			Debug.Log("Loading Progress..."+(asop.progress * 100) + "%");
			yield return null;
		}

	}


}
