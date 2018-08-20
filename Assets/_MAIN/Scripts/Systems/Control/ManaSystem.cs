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
	float currMana;
	float maxMana;

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

	public bool isHaveEnoughMana (float manaCost, bool isUseMana) { //WHEN USING STAND	
		if (currMana >= manaCost) {
			if (isUseMana) {
				UseMana(manaCost);
			}

			return true;
		} else {
			player.isUsingStand = false;
			return false;
		}
	}

	public void UseMana (float manaCost) {
		isCheckingMana = true;
		mana.PlayerMana -= manaCost;
		isManaFull = false;
		isCheckingMana = false;
		player.isUsingStand = true;

		PrintMana();
	}

	void RegenMana () {
		if (currMana < maxMana) {
			if (regenTime <= 1f) {
				regenTime += deltaTime;
			} else {
				mana.PlayerMana += mana.manaRegenPerSecond;
				regenTime = 0f;
		
				if (mana.PlayerMana > maxMana) {
					mana.PlayerMana = maxMana;
				}
			}
		} else {
			isManaFull = true;
		}

		PrintMana();
	}

	void PrintMana () {
		mana.textMana.text = currMana.ToString();
	}
}
