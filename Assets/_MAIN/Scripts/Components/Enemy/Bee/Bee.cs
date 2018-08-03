using UnityEngine;

public enum BeeState
{
	Idle,
	Patrol,
	Chase,
	Attack,
	Damaged,
	Die
}

public class Bee : MonoBehaviour {
	[HeaderAttribute("Current")]
	public BeeState beeState;
	
}
