using UnityEngine;
// using UnityEngine.UI;
using Unity.Entities;

public class UIHPManaToolSystem : ComponentSystem {
	public struct UIHPManaToolData {
		public readonly int Length;
		public ComponentArray<UIHPManaTool> UIHPManaTool;
	}
	[InjectAttribute] UIHPManaToolData uiHPManaToolData;

	[InjectAttribute] UIPlayerInfoSystem uiPlayerInfoSystem;

	UIHPManaTool uiHPManaTool;

	bool isInitHPManaImage = false;
	bool isReducingCloth = false;

	float playerHP;
	float playerMP;
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
			
			
			if (!isInitHPManaImage) {
				try {
					CheckInit ();
				} catch {
					return;
				}

				isInitHPManaImage = true;
			}

			CheckHP ();
			CheckMP ();
		}
	}

	void CheckInit () {
		maxHP = uiHPManaTool.player.MaxHP;
		maxMP = uiHPManaTool.player.MaxMP;
		maxClothWidth = uiHPManaTool.clothHP.rectTransform.sizeDelta.x;
		// initClothPos = uiHPManaTool.clothHP.rectTransform.localPosition.x;
		initClothPos = maxClothWidth;
		// initClothPos = maxClothWidth / 2;
		// initClothPos = uiHPManaTool.initClothPosX;
		
		healthThreshold = currentClothWidth;
		healthReduceValue = uiHPManaTool.healthReduceValue;

		ReduceCloth ();
		// PrintHP ();
		DrawClothHP ();
		PrintMana ();
	}

	void CheckHP () {
		if (uiHPManaTool.isHPChange) {
			ReduceCloth();
		} else if (isReducingCloth) {
			if (healthThreshold > currentClothWidth) {
				healthThreshold -= healthReduceValue * Time.deltaTime;
				DrawClothHP();
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
		playerHP = uiHPManaTool.playerHealth.PlayerHP;
		// Debug.Log(playerHP);
		currentClothWidth = (playerHP/maxHP) * maxClothWidth;
		isReducingCloth = true;
		uiHPManaTool.isHPChange = false;
		PrintHP ();
	}

	void PrintHP () {
		uiPlayerInfoSystem.PrintHP(playerHP.ToString()+" / "+maxHP);
	}

	public void PrintMana () {
		playerMP = uiHPManaTool.playerMana.PlayerMP;
		// Debug.Log("maxMP "+maxMP);
		// Debug.Log("playerMP "+playerMP);
		uiHPManaTool.imageMana.fillAmount = playerMP/maxMP;
		uiHPManaTool.isMPChange = false;
		uiPlayerInfoSystem.PrintMP(playerMP.ToString()+" / "+maxMP);
	}

	public void PrintTool (Sprite toolSprite, string toolName) {
		uiHPManaTool.imageTool.sprite = toolSprite;
		// Debug.Log(toolSprite.name);
		uiPlayerInfoSystem.PrintTool(toolName);
	}

	void DrawClothHP () {
		//SET CLOTH POS X
		uiHPManaTool.clothHP.rectTransform.localPosition = new Vector2 (healthThreshold - initClothPos, 0f);
		// uiHPManaTool.clothHP.rectTransform.localPosition = new Vector2 (healthThreshold, 0f);
		// Debug.Log(healthThreshold);
		// Debug.Log(initClothPos);

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
