// using System.Collections.Generic;
using UnityEngine;

public enum PlayerInputAudio {
	BOW_AIM,
	NO_MANA,
	PICK_UP,
	LOOT,
	FISHING_RETURN
}

public class PlayerInput : MonoBehaviour {
	// public int[] idleAnimValue = new int[2]{0, 1};
	// public int[] moveAnimValue = new int[3]{-1, 0, 1};
	// public int[] attackAnimValue = new float[3]{-1f, 0f, 1f};
	public AudioSource audioSource;
	public AudioClip[] audioClip;

	// public bool isLockDir = false;
	// public bool isButtonHold = false;
	// public Vector3 initMoveDir = -Vector3.forward;
	public bool isInitPlayerInput = false;
	public bool isInitChargeAttack = false;

	public bool isUIOpen = false;

	/// <summary>
    /// <para>Values: <br /></para>
	/// <para>0 or 1<br /></para>
    /// <para>Indeks: <br /></para>
	/// <para>0 Down<br /></para>
	/// <para>1 Left<br /></para>
	/// <para>2 Up<br /></para>
	/// <para>3 Right<br /></para>
	/// </summary>
	// public List<int> dirButtons	= new List<int>(4);  

	// public List<int> slashComboVal;

	// [SerializeField] int attackMode = 0;
	[SerializeField] Vector3 currMoveDir = Vector3.zero;

	// public Vector3 moveDir = Vector3.down; //TEMP Set to Down Direction		
	public Vector3 moveDir {  
		get {return currMoveDir;}
		set {
			Vector3 calculatedMoveDir = Vector3.zero;

			if (value != Vector3.zero) {
				if (value.z != 0f ) {
					calculatedMoveDir = new Vector3 (value.x, value.y, value.z * GameStorage.Instance.settings.verticalMultiplier);
				} else { 
					calculatedMoveDir = value;
				}
			}

			currMoveDir = calculatedMoveDir;
		}
	}

	/// <summary>
    /// <para>Values: <br /></para>
	/// <para>0 Down<br /></para>
	/// <para>1 Left<br /></para>
	/// <para>2 Up<br /></para>
	/// <para>3 Right<br /></para>
	/// </summary>
	public int direction = 1;
	
	/// <summary>
    /// <para>Values: <br /></para>
	/// <para>0 Run<br /></para>
	/// <para>1 CHARGE<br /></para>
	/// <para>2 GUARD<br /></para>
	/// <para>3 STEADY FOR BULLET TIME<br /></para>
	/// </summary>
	public int moveMode = 0;
	
	// / <summary>
    // / <para>Values: <br /></para>
	// / <para>0 STAND<br /></para>
	// / <para>1 CHARGE<br /></para>
	// / <para>2 GUARD<br /></para>
	// / <para>3 STEADY FOR BULLET TIME<br /></para>
	// / </summary>
	// public int steadyMode = 0;
	
	/// <summary>
    /// <para>Values: <br /></para>
	/// <para>-4 BOW<br /></para>
	/// <para>-3 RAPID SLASH<br /></para>
	/// <para>-2 COUNTER<br /></para>
	/// <para>-1 CHARGE<br /></para>
	/// <para>1 SLASH1<br /></para>
	/// <para>2 SLASH2<br /></para>
	/// <para>3 SLASH3<br /></para>
	/// </summary>
	public int attackMode = 0;
	// public int AttackMode {  
	// 	get {return attackMode;}
	// 	set {
	// 		if (attackMode == value) return;
			
	// 		attackMode = value;
			
	// 		if (value >= 1 && slashComboVal.Count < 3) { //SLASH
	// 			Debug.Log("ComboAdd "+ attackMode);
	// 			slashComboVal.Add(attackMode);
	// 		}
	// 	}
	// }
	
	public int bulletTimeAttackQty = 0;
		
	/// <summary>
    /// <para>Values: <br /></para>
	/// <para>-4 OPENCHEST<br /></para>
	/// <para>-3 FISHINGFAIL<br /></para>
	/// <para>-2 GET HURT (USELESS)<br /></para>
	/// <para>-1 BLOCK<br /></para>
	/// <para>0 DODGE<br /></para>
	/// <para>1 DASH<br /></para>
	/// <para>2 BRAKING<br /></para>
	/// <para>3 POWERBRACELET<br /></para>
	/// <para>4 FISHINGROD<br /></para>
	/// <para>5 BOW<br /></para>
	/// <para>6 GETSMALLTREASURE<br /></para>
	/// <para>7 GETBIGTREASURE<br /></para>
    /// </summary>
	public int interactMode = 0;
	
	/// <summary>
    /// <para>Values: <br /></para>
	/// <para>-1 CANCEL / STOP / FAIL<br /></para>
	/// <para>0 GRABBING / TAKEAIM / DASHCHARGE<br /></para>
	/// <para>1 LIFT / SWEATING / GRAB / AIMING / DASH<br /></para>
	/// <para>2 THROW / RELEASE / RETURN / SHOTBOW / BRAKE<br /></para>
    /// </summary>
	public int interactValue = 0;
	
	/// <summary>
    /// <para>Values: <br /></para>
	/// <para>-3 STARTLIFT<br /></para>
	/// <para>-2 MOVELIFT<br /></para>
	/// <para>-1 LIFTING<br /></para>
	/// <para>0 SWEATING<br /></para>
	/// <para>1 IDLEPUSH<br /></para>
	/// <para>2 MOVEPUSH<br /></para>
    /// </summary>
	public float liftingMode = 0;
	
	public float chargeAttackThreshold = 1f;
	public float beforeChargeDelay = 0.3f;
	public float attackAwayDelay =  0.5f;
	public float guardParryDelay = 0.5f;
	public float bulletTimeDelay = 0.3f;
	public float bulletTimeDuration = 0.3f;
	public float dodgeCooldown = 1f;
}
