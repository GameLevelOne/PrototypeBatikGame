using UnityEngine;

public class UVAnimationControl : MonoBehaviour {
	public delegate void ControllingUVAnimation();
	public event ControllingUVAnimation OnStartAnimation;
	public event ControllingUVAnimation OnSpawnSomethingOnAnimation;
	public event ControllingUVAnimation OnEndAnimation;

	public void StartAnim () {
		if (OnStartAnimation != null) {
			OnStartAnimation();
		}
	}

	public void EndAnim () {
		if (OnEndAnimation != null) {
			OnEndAnimation();
		}
	}

	public void SpawnSomethingAnim () {
		if (OnSpawnSomethingOnAnimation != null) {
			OnSpawnSomethingOnAnimation();
		}
	}
}
