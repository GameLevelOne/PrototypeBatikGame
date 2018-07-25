using UnityEngine;

public class Enemy : MonoBehaviour {
	public Player playerThatHitsEnemy;

	[SerializeField] bool isEnemyHit = false;
    [SerializeField] bool isEnemyGetHurt = false;
    [SerializeField] bool isEnemyDie = false;

	public bool IsEnemyHit {
		get {return isEnemyHit;}
		set {
			if (isEnemyHit == value) return;

			isEnemyHit = value;
		}
	}

	public bool IsEnemyGetHurt {
		get {return isEnemyGetHurt;}
		set {
			if (isEnemyGetHurt == value) return;

			isEnemyGetHurt = value;
		}
	}

	public bool IsEnemyDie {
		get {return isEnemyDie;}
		set {
			if (isEnemyDie == value) return;

			isEnemyDie = value;
		}
	}
}
