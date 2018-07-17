using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {
	public int[] idleAnimValue = new int[2]{0, 1};
	public int[] moveAnimValue = new int[3]{-1, 0, 1};
	// public int[] attackAnimValue = new float[3]{-1f, 0f, 1f};	
	public float chargeAttackThreshold = 1f;
	public List<int> slashComboVal;

	Vector2 currentDir = Vector2.zero;
	[SerializeField] int currentAttack = 0;
	[SerializeField] int currentChargeAttack = 0;
	[SerializeField] int currentSteady = 0;
	[SerializeField] int currentMove = 0;

	public Vector2 MoveDir {
		get {return currentDir;}
		set {
			if (currentDir == value) return;

			currentDir = value;
		}
	}

	//Run 0, CHARGE 1
	public int MoveMode {
		get {return currentMove;}
		set {
			if (currentMove == value) return;

			currentMove = value;
		}
	}

	//STAND 0, DIE -1, CHARGE 1
	public int SteadyMode {
		get {return currentSteady;}
		set {
			if (currentSteady == value) return;

			currentSteady = value;
		}
	}

	//SLASH 1-3, CHARGE -1, SHOT -2
	public int AttackMode {  
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
