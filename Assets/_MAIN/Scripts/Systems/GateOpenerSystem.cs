using UnityEngine;
using Unity.Entities;

public class GateOpenerSystem : ComponentSystem {
	public struct GateOpenerData {
		public readonly int Length;
		public ComponentArray<GateOpener> GateOpener;
	}
	[InjectAttribute] GateOpenerData gateOpenerData;
	GateOpener gateOpener;

	public struct GateData {
		public readonly int Length;
		public ComponentArray<Gate> Gate;
	}
	[InjectAttribute] GateData gateData;
	Gate gate;

	public struct UINotifData {
		public readonly int Length;
		public ComponentArray<UINotif> uiNotif;

	}
	[InjectAttribute] UINotifData uiNotifData;
	UINotif uiNotif;

	protected override void OnUpdate () {
		// if (GateOpenerData.Length == 0) return;
		for (int i=0; i<gateData.Length; i++) {
			gate = gateData.Gate[i];

			if (!gate.isInitGate) {
				InitGate ();
			}
		}

		for (int i=0; i<gateOpenerData.Length; i++) {
			gateOpener = gateOpenerData.GateOpener[i];

			if (gateOpener.isInteracting) {
				if (gateOpener.gate.isInitGate && !gateOpener.gate.isOpened) {
					gateOpener.player.isCanOpenGate = true;
				} else {
					gateOpener.player.isCanOpenGate = false;
				}
			} else {
				gateOpener.player.isCanOpenGate = false;
			}
		}
	}

	void InitGate () {
		//LOAD GATE STATE
		gate.isOpened = PlayerPrefs.GetInt(Constants.EnvirontmentPrefKey.GATES_STATE + gate.gateID, 0) == 1 ? true : false;

		//UNLOCKED GATE SPRITE BY SAVED GATE STATE
		if (gate.isOpened) {
			gate.gateSpriteRen.sprite = null;
			gate.gateCol.enabled = false;
		} else {
			gate.gateSpriteRen.sprite = gate.closedGateSprite;
			gate.gateCol.enabled = true;
		}

		gate.isInitGate = true;
	}

	public void CheckAvailabilityGateKey () {
		if (PlayerPrefs.GetInt(Constants.PlayerPrefKey.PLAYER_SAVED_KEY + gateOpener.gate.gateID, 0) == 1) {
			OpenGate();
		} else {
			Debug.Log("You do not have key for this gate with ID : "+gateOpener.gate.gateID);
			
			gateOpener.gate.animator.Play(Constants.AnimationName.GATE_LOCKED);

			for (int i=0;i<uiNotifData.Length;i++)
			{
				UINotif uiNotif = uiNotifData.uiNotif[i];
				uiNotif.TextToShow = gateOpener.gate.textLockedInfo;
				uiNotif.call = true;
			}
		}
	}

	void OpenGate () {
		//SAVE GATE STATE
		PlayerPrefs.SetInt(Constants.EnvirontmentPrefKey.GATES_STATE + gate.gateID, 1);

		//DELETE SAVED KEY
		PlayerPrefs.SetInt(Constants.PlayerPrefKey.PLAYER_SAVED_KEY + gateOpener.gate.gateID, 0);

		gateOpener.gate.isOpened = true;
		gateOpener.gate.gateSpriteRen.sprite = null;
		gateOpener.gate.gateCol.enabled = false;
		gateOpener.gate.isSelected = false;
		gateOpener.gate = null;
		// gateOpener.gate.gateAnimator.Play(Constants.AnimationName.GATE_OPEN);

		gateOpener.isInteracting = false;
		gateOpener.player.isCanOpenGate = false;
	}
}
