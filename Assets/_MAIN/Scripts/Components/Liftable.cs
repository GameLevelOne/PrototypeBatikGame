using UnityEngine;

public enum LiftableType {
	LIFTABLE,
	UNLIFTABLE,
	GRABABLE
}

public class Liftable : MonoBehaviour {
	public LiftableType liftableType;

	// public float weight;
}
