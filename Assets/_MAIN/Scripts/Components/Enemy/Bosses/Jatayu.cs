using UnityEngine;

// ASSET JATAYU yang IDLE itu adalah MOVE!
public enum JatayuState{
	Entrance,
	Move,
	CloseWings,
	Attack1,
	FlapFast,
	Attack2,
	Attack3,
	HP50,
	Hit,
	Burned,
	Die
}

public class Jatayu : MonoBehaviour {
	public GameObject attack1Object;
	public GameObject attack2Object;
	public GameObject attack3Object;
	public Collider jatayuCollider;
	public SpriteRenderer headSprite;
	public Transform playerTransform;

	[SpaceAttribute(10f)]
	public float verticalSpeed;
	public float horizontalSpeed;

	[SpaceAttribute(10f)]
	public float idleDuration;
	public float closeWingsDuration;
	public float flapFastDuration;
	public float moveDuration;

	[HeaderAttribute("Current")]
	public bool invulnerable = false;
	public JatayuState state;
	public bool initEntrance = false;
	public bool initMove = false;
	public bool initCloseWings = false;
	public bool initFlapFast = false;
	public bool initAttack1 = false;
	public bool initAttack2 = false;
	public bool initAttack3 = false;
	public bool initHP50 = false;
	public bool initHit = false;
	public bool initBurned = false;
	public bool initDie = false;

	[SpaceAttribute(10f)]
	public float tDelayFace = 1f;
	

#region animation event
	void OnEndEntranceAnim(){}
	void OnEndMoveAnim(){}
	void OnEndAttack1Anim(){}
	void OnEndAttack2Anim(){}
	void OnEndAttack3Anim(){}
	void OnEndCloseWingsAnim(){}
	void OnEndFlapFastAnim(){}
	void OnEndHP50Anim(){}
	void OnEndHitAnim(){}
	void OnEndBurnedAnim(){}
	void OnEndDieAnim(){}
#endregion
}
