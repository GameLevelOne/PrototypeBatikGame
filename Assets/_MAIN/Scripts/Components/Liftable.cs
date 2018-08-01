using UnityEngine;

public enum LiftableType {
	NONE,
	BLOCKADE,
	SECRET,
	OTHER
}

public class Liftable : MonoBehaviour {
	public LiftableType liftableType;

	public float weight;
}
