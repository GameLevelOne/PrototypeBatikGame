using UnityEngine;
using Unity.Entities;

public class PlayerSystem : ComponentSystem {
	public struct PlayerData {
		public readonly int Length;
		public ComponentArray<Player> Player;
		public ComponentArray<Facing2D> Facing;
	}
	[InjectAttribute] PlayerData playerData;
	public struct ToolData {
		public readonly int Length;
		// public ComponentArray<PlayerInput> PlayerInput;
		// public ComponentArray<Player> Player;
		public ComponentArray<PlayerTool> PlayerTool;
		// public ComponentArray<Animation2D> Animation;
	}
	[InjectAttribute] ToolData toolData;
	
	Player player;
	Facing2D facing;

	GameObject playerAttackAreaObj;
	AttackAreaDebug attackAreaDebug;

	protected override void OnUpdate () {
		for (int i=0; i<playerData.Length; i++) {
			player = playerData.Player[i];
			facing = playerData.Facing[i];

			if (!player.isInitAttackAreaObj) {
				try {
					InitAttackAreaObj();
					 // Debug.Log("Finish Init Player AttackAreaObj");
				} catch (System.Exception e) {
					 // Debug.Log("PlayerSystem ERROR : "+e);

					GameObject.Destroy(playerAttackAreaObj);
					UpdateInjectedComponentGroups();
				}
			} else {
				// this.Enabled = false;
				playerAttackAreaObj.transform.position = facing.attackArea.transform.position;

				//DEBUGGING
				CheckDebugCanvasAttackArea();
			}
		}
	}
	
	void InitAttackAreaObj () {
		playerAttackAreaObj = GameObject.Instantiate(player.playerAttackAreaObj, player.transform.position, Quaternion.Euler(Vector3.zero));

		PlayerAttackAreaObject playerAttackArea = playerAttackAreaObj.GetComponent<PlayerAttackAreaObject>();

		playerAttackArea.playerInteract.player = player;
		playerAttackArea.fishingRod.player = player;
		playerAttackArea.chestOpener.player = player;
		playerAttackArea.powerBracelet.player = player;
		playerAttackArea.powerBracelet.liftMainObjParent = player.liftingParent;
		playerAttackArea.gateOpener.player = player;
		playerAttackArea.digChecker.player = player;

		player.GetComponent<Attack>().dashAttackArea = playerAttackArea.dashAttackObj;
		toolData.PlayerTool[0].fishingBaitObj = playerAttackArea.fishingRod.gameObject;

		attackAreaDebug = playerAttackAreaObj.GetComponent<AttackAreaDebug>();

		playerAttackAreaObj.SetActive(true);
		player.isInitAttackAreaObj = true;
	}

	public void ResetPlayerHP () { //BUG FIX
		player.health.PlayerHP = 0f;
	}

	void CheckDebugCanvasAttackArea () {
		int isCheckDebugAttackArea = PlayerPrefs.GetInt(Constants.PlayerPrefKey.PLAYER_DEBUG_ATTACK_AREA, 0);
		
		if (isCheckDebugAttackArea != attackAreaDebug.showDebugCanvas) {
			if (isCheckDebugAttackArea > 0) {
				for (int i=0; i<attackAreaDebug.debugCanvases.Length; i++) {
					attackAreaDebug.debugCanvases[i].SetActive(true);
				}
			} else {
				for (int i=0; i<attackAreaDebug.debugCanvases.Length; i++) {
					attackAreaDebug.debugCanvases[i].SetActive(false);
				}
			}

			attackAreaDebug.showDebugCanvas = isCheckDebugAttackArea;
		}
	}
}
