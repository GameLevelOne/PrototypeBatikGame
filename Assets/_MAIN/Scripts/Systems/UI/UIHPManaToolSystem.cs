using UnityEngine;
using UnityEngine.UI;
using Unity.Entities;
using UnityEngine;

public class UIHPManaToolSystem : ComponentSystem {
	public struct UIHPManaToolData {
		public readonly int Length;
		public ComponentArray<UIHPManaTool> UIHPManaTool;
	}
	[InjectAttribute] UIHPManaToolData uiHPManaToolData;

	UIHPManaTool uiHPManaTool;

	bool isInitHPManaImage = false;

	float maxHP;
	float maxMP;

	protected override void OnUpdate () {
		if (uiHPManaToolData.Length == 0) return;

		for (int i=0; i<uiHPManaToolData.Length; i++) {
			uiHPManaTool = uiHPManaToolData.UIHPManaTool[i];
			maxHP = uiHPManaTool.player.MaxHP;
			maxMP = uiHPManaTool.player.MaxMP;

			if (!isInitHPManaImage) {
				PrintHP ();
				PrintMana ();

				isInitHPManaImage = true;
			}

			if (uiHPManaTool.isHPChange) {
				PrintHP();
			}

			if (uiHPManaTool.isMPChange) {
				PrintMana();
			}
		}
	}

	public void PrintHP () {
		float playerHP = uiHPManaTool.playerHealth.PlayerHP;
		Debug.Log(playerHP);
		uiHPManaTool.isHPChange = false;
	}

	public void PrintMana () {
		float playerMP = uiHPManaTool.playerMana.PlayerMP;
		Debug.Log("maxMP "+maxMP);
		Debug.Log("playerMP "+playerMP);

		uiHPManaTool.imageMana.fillAmount = playerMP/maxMP;

		uiHPManaTool.isMPChange = false;
	}

	public void PrintTool (Sprite toolSprite) {
		uiHPManaTool.imageTool.sprite = toolSprite;
		// Debug.Log(toolSprite.name);
	}
}
