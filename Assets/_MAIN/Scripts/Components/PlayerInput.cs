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

	// [SerializeField] Vector2 currentDir = Vector2.zero;
	[SerializeField] int attackMode = 0;
	// [SerializeField] int bulletTimeAttackQty = 0;
	// [SerializeField] int steadyMode = 0;
	// [SerializeField] int moveMode = 0;
	// [SerializeField] int interactMode = 0;
	// [SerializeField] int interactValue = 0;
	// [SerializeField] float liftingMode = 0f;

	public Vector2 moveDir = Vector2.zero;
	
	/// <summary>
    /// <para>Values: <br /></para>
	/// <para>-1 DIE<br /></para>
	/// <para>0 Run<br /></para>
	/// <para>1 CHARGE<br /></para>
	/// <para>2 GUARD<br /></para>
	/// <para>3 STEADY FOR BULLET TIME<br /></para>
	/// </summary>
	public int moveMode = 0;
	
	/// <summary>
    /// <para>Values: <br /></para>
	/// <para>0 STAND<br /></para>
	/// <para>1 CHARGE<br /></para>
	/// <para>2 GUARD<br /></para>
	/// <para>3 STEADY FOR BULLET TIME<br /></para>
	/// </summary>
	// public int steadyMode = 0;
	
	/// <summary>
    /// <para>Values: <br /></para>
	/// <para>-3 RAPID SLASH<br /></para>
	/// <para>-2 COUNTER<br /></para>
	/// <para>-1 CHARGE<br /></para>
	/// <para>1 SLASH1<br /></para>
	/// <para>2 SLASH2<br /></para>
	/// <para>3 SLASH3<br /></para>
	/// </summary>
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
	
	public int bulletTimeAttackQty = 0;
		
	/// <summary>
    /// <para>Values: <br /></para>
	/// <para>-2 GET HURT<br /></para>
	/// <para>-1 BLOCK<br /></para>
	/// <para>0 DODGE<br /></para>
	/// <para>1 DASH<br /></para>
	/// <para>2 BRAKING<br /></para>
	/// <para>3 POWERBRACELET<br /></para>
	/// <para>4 FISHINGROD<br /></para>
    /// </summary>
	public int interactMode = 0;
	
	/// <summary>
    /// <para>Values: <br /></para>
	/// <para>0 INIT<br /></para>
	/// <para>1 LIFT / SWEATING / GRAB<br /></para>
	/// <para>2 THROW / RELEASE / RETURN<br /></para>
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
}
