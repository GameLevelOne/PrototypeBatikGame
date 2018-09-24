using UnityEngine;
using UnityEngine.SceneManagement;

public class SystemManager : MonoBehaviour {
	public bool isChangeScene;
	// public bool isDoneInitDisabledSystem;
	
	public int currentMapIdx;

	void OnEnable () {
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	void OnDisable () {
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	void OnSceneLoaded (Scene scene, LoadSceneMode mode) {
		// Debug.Log("Scene "+scene.name+" Loaded!");
		isChangeScene = true;
		currentMapIdx = SceneManager.GetActiveScene().buildIndex -1;
		GameStorage.Instance.CurrentScene = SceneManager.GetActiveScene().name;

	}
}
