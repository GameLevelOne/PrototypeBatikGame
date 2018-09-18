﻿using UnityEngine;
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
		deltaTime = Time.deltaTime;
		
		if (manaData.Length == 0) return;

		for (int i=0; i<manaData.Length; i++) {
			mana = manaData.Mana[i];
			player = manaData.Player[i];

			currMana = mana.PlayerMP;
			maxMana = player.MaxMP;

			if (!isManaFull && !isCheckingMana) {
				RegenMana();
			}
		}
	}

	public bool isHaveEnoughMana (float manaCost, bool isUseMana, bool isUsingStand) { //WHEN USING STAND	
		if (currMana >= manaCost) {
			if (isUseMana) {
				UseMana(manaCost, isUsingStand);
			}

			return true;
		} else {
			player.isUsingStand = false;
			return false;
		}
	}

	public void UseMana (float manaCost, bool isUsingStand) {
		isCheckingMana = true;
		mana.PlayerMP -= manaCost;
		isManaFull = false;
		isCheckingMana = false;

		if (isUsingStand) {
			player.isUsingStand = true;
		}
	}

	void RegenMana () {
		if (currMana < maxMana) {
			if (regenTime <= 1f) {
				regenTime += deltaTime;
			} else {
				mana.PlayerMP += mana.manaRegenPerSecond;
				regenTime = 0f;
		
				if (mana.PlayerMP > maxMana) {
					mana.PlayerMP = maxMana;
				}
			}
		} else {
			isManaFull = true;
		}
	}
}
