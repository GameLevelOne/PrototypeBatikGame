using UnityEngine;

public enum FishingRodState {
	IDLE,
	THROW,
	STAY,
	RETURN,
}

public class FishingRod : MonoBehaviour {
	public FishingRodState state;
	public Player player;
	
	public Collider2D baitCol;

	// public Rigidbody2D fishBait;

	// public Vector2 baitInitPos;
	// public Vector2 targetBaitPos;
	// public int dirID;
	// public float speed;
	public float fishingRange;

	[SerializeField] bool isBaitLaunched = false;

	public bool IsBaitLaunched {
		get {return isBaitLaunched;}
		set { 
			if (isBaitLaunched == value) return;

			isBaitLaunched = value;
		}
	}
}
