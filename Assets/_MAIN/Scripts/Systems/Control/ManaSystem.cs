using UnityEngine;
using Unity.Entities;

public class ManaSystem : ComponentSystem {
	public struct ManaData {
		public readonly int Length;
		public ComponentArray<Player> Player;
		public ComponentArray<Mana> Mana;
	}
	[InjectAttribute] ManaData manaData;

	Mana mana;
	Player player;

	float deltaTime;
	float regenTime;
	int currMana;
	int maxMana;

	bool isManaFull = false;
	bool isCheckingMana = false;

	protected override void OnUpdate () {
		if (manaData.Length == 0) return;

		deltaTime = Time.deltaTime;

		for (int i=0; i<manaData.Length; i++) {
			mana = manaData.Mana[i];
			player = manaData.Player[i];

			currMana = mana.PlayerMana;
			maxMana = player.MaxMana;

			if (!isManaFull && !isCheckingMana) {
				RegenMana();
			}
		}
	}

	public void CheckMana (int manaCost) { //WHEN USING STAND
		isCheckingMana = true;
		
		if (currMana >= manaCost) {
			mana.PlayerMana -= manaCost;
			player.isUsingStand = true;
			isManaFull = false;
		} else {
			player.isUsingStand = false;
		}

		isCheckingMana = false;
		PrintMana();
	}

	void RegenMana () {
		if (currMana < maxMana) {
			if (regenTime <= 1f) {
				regenTime += deltaTime;
			} else {
				mana.PlayerMana += mana.manaRegenPerSecond;
				regenTime = 0f;
			}
		} else if (currMana > maxMana) {
			mana.PlayerMana = maxMana;
			isManaFull = true;
		}

		PrintMana();
	}

	void PrintMana () {
		mana.textMana.text = currMana.ToString();
	}
}
