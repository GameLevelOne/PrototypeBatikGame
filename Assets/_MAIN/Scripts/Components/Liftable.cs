using UnityEngine;

public enum LiftableType {
	LIFTABLE,
	UNLIFTABLE,
	GRABABLE
}

public enum LiftableState {
	IDLE,
	THROW,
	LAND
}

public class Liftable : MonoBehaviour {
	public LiftableType liftableType;
	public LiftableState state;
	
	public GameObject mainObj;

	// public float weight;
}
