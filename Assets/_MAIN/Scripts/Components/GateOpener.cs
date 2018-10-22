using UnityEngine;

public class GateOpener : MonoBehaviour {
	public Player player;

	[HeaderAttribute("Current")]
	public Gate gate;
	public bool isInteracting = false;

	void OnTriggerStay (Collider col) {
		if (col.tag == Constants.Tag.GATE && player.isHitGateObject) {
			if (!col.GetComponent<Gate>().isOpened) {
				gate = col.GetComponent<Gate>();

				if (!gate.isSelected) {
					gate.isSelected = true;
					isInteracting = true;
					
					//SET UI INTERACTION HINT
					player.ShowInteractionHint(HintMessage.OPEN);
				}
			}
		} 
	}

	void OnTriggerExit (Collider col) {
		if (col.tag == Constants.Tag.GATE) {
			Gate newGate = col.GetComponent<Gate>();

			if (gate == newGate && !newGate.isOpened && newGate.isSelected) {
				gate.isSelected = false;
				gate = null;
				isInteracting = false;

				//SET UI INTERACTION HINT
				player.HideHint();
			}
		}
	}
}
