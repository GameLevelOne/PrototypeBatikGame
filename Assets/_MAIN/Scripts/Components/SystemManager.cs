using UnityEngine;
using UnityEngine.SceneManagement;

public class SystemManager : MonoBehaviour {
	public bool isChangeScene;
	public bool isDoneInitSystem;	
	public int currentMapIdx;
	
	public string menuSceneName;

	void OnEnable () {
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	void OnDisable () {
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	void OnSceneLoaded (Scene scene, LoadSceneMode mode) {
		if (!isChangeScene) {
			// Debug.Log("Scene "+scene.name+" Loaded!");
			isChangeScene = true;
			currentMapIdx = SceneManager.GetActiveScene().buildIndex - 1;
			if (currentMapIdx<0) 
				currentMapIdx = 0;
			if (SceneManager.GetActiveScene().name!=menuSceneName)
				GameStorage.Instance.CurrentScene = SceneManager.GetActiveScene().name;

		}

	}
}
