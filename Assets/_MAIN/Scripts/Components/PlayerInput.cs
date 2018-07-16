using UnityEngine;

public class PlayerInput : MonoBehaviour {
	public int[] idleAnimValue = new int[2]{0, 1};
	public int[] moveAnimValue = new int[3]{-1, 0, 1};
	// public int[] attackAnimValue = new float[3]{-1f, 0f, 1f};	

	Vector2 currentMove = Vector2.zero;
	int currentAttack = 0;

	public Vector2 Move {
		get {return currentMove;}
		set {
			if (currentMove == value) return;

			currentMove = value;
		}
	}

	public int Attack {
		get {return currentAttack;}
		set {
			if (currentAttack == value) return;
			
			currentAttack = value;
		}
	}
}
