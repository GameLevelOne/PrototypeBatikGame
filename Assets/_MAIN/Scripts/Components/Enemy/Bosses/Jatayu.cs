using UnityEngine;

public enum JatayuState{
	Appear,
	Idle,
	Attack1,
	Attack2,
	Die
}

public class Jatayu : MonoBehaviour {
	public GameObject attack1Object;
	public GameObject attack2Object;
	public Collider jatayuCollider;
	[SpaceAttribute(10f)]
	public JatayuState state;
	[SpaceAttribute(15f)]
	public float idleDuration;
	[HeaderAttribute("Current")]
	public bool initAppear = false;
	public bool initIdle = false;
	public bool initAttack1 = false;
	public bool initAttack2 = false;
	public bool initDie = false;
	
	
}
