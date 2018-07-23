using UnityEngine;
public static class Data {

	static bool currentIsEnemyHit = false;
	static bool currentIsPlayerHit = false;

	public static bool isEnemyHit {
		get {return currentIsEnemyHit;}
		set {
			if (currentIsEnemyHit == value) return;

			currentIsEnemyHit = value;
		}
	}

	public static bool isPlayerHit {
		get {return currentIsEnemyHit;}
		set {
			if (currentIsPlayerHit == value) return;

			currentIsPlayerHit = value;
		}
	}
}
