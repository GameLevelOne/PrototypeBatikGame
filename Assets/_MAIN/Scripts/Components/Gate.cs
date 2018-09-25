using UnityEngine;

public class Gate : MonoBehaviour {
	public Collider gateCol;
	public SpriteRenderer gateSpriteRen;
	
	public Sprite closedGateSprite;
	public Sprite openedGateSprite;

	[HeaderAttribute("Saved ID")]
	public int gateID;

	[HeaderAttribute("Current")]
	public bool isInitGate = false;
	public bool isOpened = false;
	public bool isSelected = false;
	
	// [HeaderAttribute("Testing")]
	// public bool resetPrefKey;

	void Awake () {
		// if (resetPrefKey) {
		// 	PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_SAVED_KEY + gateID, 1);
		// 	PlayerPrefs.SetInt(Constants.EnvirontmentPrefKey.GATES_STATE + gateID, 0);
		// }
	}
}
