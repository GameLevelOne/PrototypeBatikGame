using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {
	public int[] idleAnimValue = new int[2]{0, 1};
	public int[] moveAnimValue = new int[3]{-1, 0, 1};
	// public int[] attackAnimValue = new float[3]{-1f, 0f, 1f};	
	public float chargeAttackThreshold = 1f;
	public float beforeChargeDelay = 0.3f;
	public float attackAwayDelay =  0.5f;
	public float guardParryDelay = 0.5f;
	public float bulletTimeDelay = 0.3f;
	public float bulletTimeDuration = 0.3f;
	public float dodgeCooldown = 1f;

	public List<int> slashComboVal;

	Vector2 currentDir = Vector2.zero;
	[SerializeField] int currentAttack = 0;
	[SerializeField] int currentBulletTimeAttack = 0;
	// [SerializeField] int currentChargeAttack = 0;
	[SerializeField] int currentSteady = 0;
	[SerializeField] int currentMove = 0;
	[SerializeField] bool currentIsDodging = false;
	[SerializeField] bool isReadyForDodging = true;

	[SerializeField] bool isUsingTool;


	public Vector2 MoveDir {
		get {return currentDir;}
		set {
			if (currentDir == value) return;

			currentDir = value;
		}
	}

	//Run 0, WALK -1 (Not Yet), CHARGE 1, GUARD 2
	public int MoveMode {
		get {return currentMove;}
		set {
			if (currentMove == value) return;

			currentMove = value;
		}
	}

	public bool isDodging {
		get {return currentIsDodging;}
		set {
			if (currentIsDodging == value) return;

			if (value && isReadyForDodging && (MoveDir != Vector2.zero)) {
				currentIsDodging = true;
				isReadyForDodging = false;
				Invoke("ResetDodge", dodgeCooldown);
			} else {
				currentIsDodging = false;
			}
		}
	}

	//STAND 0, DIE -1, CHARGE 1, GUARD 2
	public int SteadyMode {
		get {return currentSteady;}
		set {
			if (currentSteady == value) return;

			currentSteady = value;
		}
	}

	//SLASH 1-3, CHARGE -1, COUNTER -2, MINATO -3
	public int AttackMode {  
		get {return currentAttack;}
		set {
			if (currentAttack == value) return;
			
			currentAttack = value;
			
			if (value >= 1 && slashComboVal.Count < 3) { //SLASH
				slashComboVal.Add(currentAttack);
			}
		}
	}

	public int BulletTimeAttackQty {
		get {return currentBulletTimeAttack;}
		set {
			if (currentBulletTimeAttack == value) return;
			
			currentBulletTimeAttack = value;
			Debug.Log("Rasengan Power " + currentBulletTimeAttack);
		}
	}

	public bool IsUsingTool
	{
		get{return isUsingTool;}
		set{isUsingTool = value;}
	}

	void ResetDodge () {
		isReadyForDodging = true;
	}
}
