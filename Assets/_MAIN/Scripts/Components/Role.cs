using UnityEngine;

public enum GameRole {
	Enemy,
	Player,
	Boss
}

public class Role : MonoBehaviour {
	public GameRole gameRole;
}
