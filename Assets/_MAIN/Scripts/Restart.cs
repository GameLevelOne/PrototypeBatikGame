using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour {
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			SceneManager.LoadScene("SceneMapChunk");
		}		
	}
}
