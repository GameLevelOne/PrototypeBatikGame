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
	[SerializeField] int attackMode = 0;
	[SerializeField] int bulletTimeAttackQty = 0;
	// [SerializeField] int currentChargeAttack = 0;
	[SerializeField] int steadyMode = 0;
	[SerializeField] int moveMode = 0;
	[SerializeField] bool isDodging = false;
	[SerializeField] bool isChangeTool = false;
	[SerializeField] bool isUsingTool = false;
	[SerializeField] bool isReadyForDodging = true;

	public Vector2 MoveDir {
		get {return currentDir;}
		set {
			if (currentDir == value) return;

			currentDir = value;
		}
	}

	//Run 0, WALK -1 (Not Yet), CHARGE 1, GUARD 2
	public int MoveMode {
		get {return moveMode;}
		set {
			if (moveMode == value) return;

			moveMode = value;
		}
	}

	public bool IsDodging {
		get {return isDodging;}
		set {
			if (isDodging == value) return;

			if (value && isReadyForDodging && (MoveDir != Vector2.zero)) {
				isDodging = true;
				isReadyForDodging = false;
				Invoke("ResetDodge", dodgeCooldown);
			} else {
				isDodging = false;
			}
		}
	}

	//STAND 0, DIE -1, CHARGE 1, GUARD 2, BULLET TIME -2
	public int SteadyMode {
		get {return steadyMode;}
		set {
			if (steadyMode == value) return;

			steadyMode = value;
		}
	}

	//SLASH 1-3, CHARGE -1, COUNTER -2, MINATO -3
	public int AttackMode {  
		get {return attackMode;}
		set {
			if (attackMode == value) return;
			
			attackMode = value;
			
			if (value >= 1 && slashComboVal.Count < 3) { //SLASH
				slashComboVal.Add(attackMode);
			}
		}
	}

	public int BulletTimeAttackQty {
		get {return bulletTimeAttackQty;}
		set {
			if (bulletTimeAttackQty == value) return;
			
			bulletTimeAttackQty = value;
		}
	}

	public bool IsChangeTool
	{
		get {return isChangeTool;}
		set {
			if (isChangeTool == value) return;
			
			isChangeTool = value;
		}
	}

	public bool IsUsingTool
	{
		get {return isUsingTool;}
		set {
			if (isUsingTool == value) return;
			
			isUsingTool = value;
		}
	}

	void ResetDodge () {
		isReadyForDodging = true;
	}
}
