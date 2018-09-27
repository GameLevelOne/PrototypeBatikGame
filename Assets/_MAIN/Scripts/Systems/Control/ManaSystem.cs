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
	float regenRate;
	float maxMana;

	protected override void OnUpdate () {
		deltaTime = Time.deltaTime;
		
		// if (manaData.Length == 0) return;

		for (int i=0; i<manaData.Length; i++) {
			mana = manaData.Mana[i];
			player = manaData.Player[i];

			if (!mana.isInitMana) {
				try {
					Debug.Log("Start init Mana");
					InitMana ();
				} catch (System.Exception e) {
					Debug.Log("Something wrong Mana \n ERROR : "+e);
					return;
				}

				Debug.Log("Finish init Mana");
				mana.isInitMana = true;
			} else if (player.state != PlayerState.DIE) {
				if (!mana.isManaFull && mana.isCheckingMana) {
					RegenMana();
				}
			}
		}
	}

	void InitMana () {
		maxMana = player.MaxMP;
		regenRate = mana.manaRegenPerSecond;
		mana.isCheckingMana = true;
	}

	public bool isHaveEnoughMana (float manaCost, bool isUseMana, bool isUsingStand) { //WHEN USING STAND	
		if (mana.PlayerMP >= manaCost) {
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
		mana.isCheckingMana = true;
		mana.PlayerMP -= manaCost;
		mana.isManaFull = false;

		if (isUsingStand) {
			player.isUsingStand = true;
		}
	}

	void RegenMana () {
		if (mana.PlayerMP < maxMana) {
			if (mana.manaRegenTimer <= 1f) {
				mana.manaRegenTimer += deltaTime;
			} else {
				mana.manaRegenTimer = 0f;
				mana.PlayerMP += regenRate;
		
				if (mana.PlayerMP > maxMana) {
					mana.PlayerMP = maxMana;
					mana.isCheckingMana = false;
				}
			}
		} else {
			mana.isManaFull = true;
		}
	}
}
