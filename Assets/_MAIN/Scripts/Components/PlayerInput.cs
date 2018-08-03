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

	[SerializeField] Vector2 currentDir = Vector2.zero;
	[SerializeField] int attackMode = 0;
	[SerializeField] int bulletTimeAttackQty = 0;
	[SerializeField] int steadyMode = 0;
	[SerializeField] int moveMode = 0;
	[SerializeField] int interactMode = 0;
	[SerializeField] int interactValue = 0;
	[SerializeField] float liftingMode = 0f;
	
	bool isReadyForDodging = true;

	public Vector2 MoveDir {
		get {return currentDir;}
		set {
			if (currentDir == value) return;

			currentDir = value;
		}
	}
	
	/// <summary>
    /// <para>Values: <br /></para>
	/// <para>-2. BLOCK<br /></para>
	/// <para>-1. -<br /></para>
	/// <para>0. Run<br /></para>
	/// <para>1. CHARGE<br /></para>
	/// <para>2. GUARD<br /></para>
	/// <para>3. DASH<br /></para>
	//Run 0, WALK -1 (Not Yet), CHARGE 1, GUARD 2, DASH 3, BLOCK -2
	public int MoveMode {
		get {return moveMode;}
		set {
			if (moveMode == value) return;

			moveMode = value;
		}
	}
	
	/// <summary>
    /// <para>Values: <br /></para>
	/// <para>-2. BLOCK<br /></para>
	/// <para>-1. DIE<br /></para>
	/// <para>0. STAND<br /></para>
	/// <para>1. CHARGE<br /></para>
	/// <para>2. GUARD<br /></para>
	//STAND 0, DIE -1, CHARGE 1, GUARD 2, BLOCK -2
	public int SteadyMode {
		get {return steadyMode;}
		set {
			if (steadyMode == value) return;

			steadyMode = value;
		}
	}
	
	/// <summary>
    /// <para>Values: <br /></para>
	/// <para>-3. RAPID SLASH<br /></para>
	/// <para>-2. COUNTER<br /></para>
	/// <para>-1. CHARGE<br /></para>
	/// <para>1. SLASH1<br /></para>
	/// <para>2. SLASH2<br /></para>
	/// <para>3. SLASH3<br /></para>
	//SLASH 1-3, CHARGE -1, COUNTER -2, RAPID SLASH -3
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
		
	/// <summary>
    /// <para>Values: <br /></para>
	/// <para>-2. GET HURT<br /></para>
	/// <para>-1. BLOCK<br /></para>
	/// <para>0. DODGE<br /></para>
	/// <para>1. DASH<br /></para>
	/// <para>2. BRAKING<br /></para>
	/// <para>3. POWERBRACELET<br /></para>
    /// </summary>
	//BLOCK -1, DODGE 0, DASH 1, BRAKING 2. GET HURT -2, POWERBRACELET 3
	public int InteractMode { 
		get {return interactMode;}
		set {
			if (interactMode == value) return;
			
			interactMode = value;
		}
	}
	
	/// <summary>
    /// <para>Values: <br /></para>
	/// <para>0. SWEATING<br /></para>
	/// <para>1. LIFT / SWEATING / GRAB<br /></para>
	/// <para>2. THROW / RELEASE<br /></para>
    /// </summary>
	//INIT 0, LIFT / SWEATING / GRAB 1, THROW / RELEASE 2
	public int InteractValue { 
		get {return interactValue;}
		set {
			if (interactValue == value) return;
			
			interactValue = value;
		}
	}
	
	/// <summary>
    /// <para>Values: <br /></para>
	/// <para>-3. STARTLIFT<br /></para>
	/// <para>-2. MOVELIFT<br /></para>
	/// <para>-1. LIFTING<br /></para>
	/// <para>0. SWEATING<br /></para>
	/// <para>1. IDLEPUSH<br /></para>
	/// <para>2. MOVEPUSH<br /></para>
    /// </summary>
	//LIFTING -1, SWEATING 0, IDLEPUSH 1, MOVELIFT -2, MOVEPUSH 2, STARTLIFT -3
	public float LiftingMode { 
		get {return liftingMode;}
		set {
			if (liftingMode == value) return;
			
			liftingMode = value;

			if (liftingMode == -3) Debug.Log("THIS -3");
		}
	}
}
