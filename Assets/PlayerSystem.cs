using UnityEngine;
using Unity.Entities;

public class PlayerSystem : ComponentSystem {
	public struct PlayerData {
		public readonly int Length;
		public ComponentArray<Player> Player;
		public ComponentArray<Facing2D> Facing;
	}
	[InjectAttribute] PlayerData playerData;
	
	Player player;
	Facing2D facing;

	GameObject playerAttackAreaObj;

	protected override void OnUpdate () {
		for (int i=0; i<playerData.Length; i++) {
			player = playerData.Player[i];
			facing = playerData.Facing[i];

			if (!player.isInitAttackAreaObj) {
				InitAttackAreaObj();
			} else {
				// this.Enabled = false;
				playerAttackAreaObj.transform.position = facing.attackArea.transform.position;
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

		player.GetComponent<Attack>().dashAttackArea = playerAttackArea.dashAttackObj;

		playerAttackAreaObj.SetActive(true);

		player.isInitAttackAreaObj = true;
	}
}
