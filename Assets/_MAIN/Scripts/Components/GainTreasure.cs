using UnityEngine;

public class GainTreasure : MonoBehaviour {
	public Player player;
	public QuestTrigger questTrigger;
	public Transform liftingTreasureParent;

	[HeaderAttribute ("Current")]
	public Lootable lootable;
	public Transform lootableTransform;
	public bool isInitGainTreasure = false;
	public bool isLootingStandard = false;
	public bool isLootingTreasure = false;
	
	void OnTriggerStay (Collider col) {
		PlayerState state = player.state;

		if (col.tag == Constants.Tag.LOOTABLE) {
			lootable = col.GetComponent<Lootable>();

			if (lootable.treasureType == TreasureType.NONE) {
				if (!isLootingStandard) {
					isLootingStandard = true;
				}
			} else {
				if (!isLootingTreasure && (state == PlayerState.IDLE || state == PlayerState.MOVE)) {
					lootableTransform = lootable.transform;
					isLootingTreasure = true;
				}
			}
		} 
	}
}
