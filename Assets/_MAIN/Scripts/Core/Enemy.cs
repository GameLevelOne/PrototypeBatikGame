﻿using UnityEngine;

public class Enemy : MonoBehaviour {
	public Player playerThatHitsEnemy;

	[SerializeField] bool currentIsEnemyHit = false;

	public bool isEnemyHit {
		get {return currentIsEnemyHit;}
		set {
			if (currentIsEnemyHit == value) return;

			currentIsEnemyHit = value;
			Debug.Log("currentIsEnemyHit " + currentIsEnemyHit);
		}
	}
}
