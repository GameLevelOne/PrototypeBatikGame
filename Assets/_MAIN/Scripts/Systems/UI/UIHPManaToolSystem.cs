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
	bool isReducingCloth = false;

	float maxHP;
	float maxMP;
	float maxClothWidth;
	float initClothPos;
	float currentClothWidth;
	float healthThreshold;
	float healthReduceValue;
	float deltaTime;

	protected override void OnUpdate () {
		if (uiHPManaToolData.Length == 0) return;

		deltaTime = Time.deltaTime;

		for (int i=0; i<uiHPManaToolData.Length; i++) {
			uiHPManaTool = uiHPManaToolData.UIHPManaTool[i];

			CheckInit ();
			CheckHP ();
			CheckMP ();
		}
	}

	void CheckInit () {
		if (!isInitHPManaImage) {
			maxHP = uiHPManaTool.player.MaxHP;
			maxMP = uiHPManaTool.player.MaxMP;
			maxClothWidth = uiHPManaTool.clothHP.rectTransform.sizeDelta.x;
			initClothPos = uiHPManaTool.clothHP.rectTransform.localPosition.x * 3;

			ReduceCloth ();
			
			healthThreshold = currentClothWidth;
			healthReduceValue = uiHPManaTool.healthReduceValue;

			PrintHP ();
			PrintMana ();

			isInitHPManaImage = true;
		}
	}

	void CheckHP () {
		if (uiHPManaTool.isHPChange) {
			ReduceCloth();
		} else if (isReducingCloth) {
			if (healthThreshold > currentClothWidth) {
				healthThreshold -= healthReduceValue * Time.deltaTime;
				PrintHP();
			} else {
				isReducingCloth = false;
			}
		}
	}

	void CheckMP () {
		if (uiHPManaTool.isMPChange) {
			PrintMana();
		}
	}

	public void ReduceCloth () {
		float playerHP = uiHPManaTool.playerHealth.PlayerHP;
		// Debug.Log(playerHP);
		currentClothWidth = (playerHP/maxHP) * maxClothWidth;
		isReducingCloth = true;
		uiHPManaTool.isHPChange = false;
	}

	public void PrintMana () {
		float playerMP = uiHPManaTool.playerMana.PlayerMP;
		// Debug.Log("maxMP "+maxMP);
		// Debug.Log("playerMP "+playerMP);
		uiHPManaTool.imageMana.fillAmount = playerMP/maxMP;
		uiHPManaTool.isMPChange = false;
	}

	public void PrintTool (Sprite toolSprite) {
		uiHPManaTool.imageTool.sprite = toolSprite;
		// Debug.Log(toolSprite.name);
	}

	void PrintHP () {
		//SET CLOTH POS X
		uiHPManaTool.clothHP.rectTransform.localPosition = new Vector2 (healthThreshold + initClothPos, 0f);
		// Debug.Log(uiHPManaTool.clothHP.rectTransform.localPosition);

		#region OLD (Using Shader Plugin)
		//SET CLOTH SCALE X
		// Vector2 scale = uiHPManaTool.clothHP.localScale;
		// uiHPManaTool.clothHP.localScale = new Vector2 (healthThreshold, scale.y);

		// //SET CLOTH POS X
		// Vector2 pos = uiHPManaTool.clothHP.localPosition;
		// float newPosX = (healthThreshold - 1f) / 2f;
		// uiHPManaTool.clothHP.localPosition = new Vector2 (newPosX, pos.y);
		#endregion
	}
}
