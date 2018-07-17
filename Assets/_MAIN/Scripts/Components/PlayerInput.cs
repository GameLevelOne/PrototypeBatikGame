using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {
	public int[] idleAnimValue = new int[2]{0, 1};
	public int[] moveAnimValue = new int[3]{-1, 0, 1};
	// public int[] attackAnimValue = new float[3]{-1f, 0f, 1f};	
	public float chargeAttackThreshold = 1f;
	public List<int> slashComboVal;

	Vector2 currentMove = Vector2.zero;
	[SerializeField] int currentAttack = 0;
	[SerializeField] int currentChargeAttack = 0;

	public Vector2 MoveMode {
		get {return currentMove;}
		set {
			if (currentMove == value) return;

			currentMove = value;
		}
	}

	public int AttackMode { //SLASH 1-3, 
		get {return currentAttack;}
		set {
			if (currentAttack == value) return;
			
			currentAttack = value;

			if (currentAttack >= 1) { //SLASH
				slashComboVal.Add(currentAttack);
			}
		}
	}
}
