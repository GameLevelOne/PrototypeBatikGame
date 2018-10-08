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

	bool isReducingCloth = false;

	// float playerHP;
	// float playerMP;
	float maxHP;
	float maxMP;
	float maxClothWidth;
	float manaReduceValue;
	// float initClothPos;
	// float currentClothWidth;
	// float healthThreshold;
	// float healthReduceValue;
	float deltaTime;

	protected override void OnUpdate () {
		deltaTime = Time.deltaTime;
		// if (uiHPManaToolData.Length == 0) return;

		for (int i=0; i<uiHPManaToolData.Length; i++) {
			uiHPManaTool = uiHPManaToolData.UIHPManaTool[i];
			
			
			if (!uiHPManaTool.isInitHPManaImage) {
				try {
					// Debug.Log("Start init UI HP MP Tool");
					CheckInit ();
				} catch (System.Exception e) {
					// Debug.Log("Something wrong UI HP MP Tool \n ERROR : "+e);
					return;
				}

				// Debug.Log("Finish init UI HP MP Tool");
				uiHPManaTool.isInitHPManaImage = true;
			} else {	
				if (uiHPManaTool.isHPChange) {
					CheckHP ();
					uiHPManaTool.reducedHPFX.Play();
				}

				if (uiHPManaTool.isMPChange) {
					CheckMP ();
				} else if (uiHPManaTool.isReducingShadowMP) {
					float shadowFillAmount = uiHPManaTool.imageShadowMP.fillAmount;
					float mainFillAmount = uiHPManaTool.imageMP.fillAmount;

					if (shadowFillAmount != mainFillAmount) {
						uiHPManaTool.imageShadowMP.fillAmount += (mainFillAmount - shadowFillAmount) * manaReduceValue * deltaTime;
					} else {
						uiHPManaTool.isReducingShadowMP = false;
					}
				}
			}
		}
	}

	void CheckInit () {
		maxHP = uiHPManaTool.player.MaxHP;
		maxMP = uiHPManaTool.player.MaxMP;
		maxClothWidth = uiHPManaTool.clothHPMask.rectTransform.sizeDelta.x;
		manaReduceValue = uiHPManaTool.manaReduceValue;
		// Debug.Log("Fill Amount "+uiHPManaTool.imageMP.fillAmount);
		// Debug.Log("Fill Amount "+uiHPManaTool.imageShadowMP.fillAmount);
		CheckHP();
		CheckMP ();
		// initClothPos = maxClothWidth;
		// healthThreshold = (playerHP/maxHP) * maxClothWidth;
		// healthReduceValue = uiHPManaTool.healthReduceValue;
		// ReduceCloth ();
		// // PrintHP ();
		// DrawClothHP ();
		// PrintMana ();
	}

	void CheckHP () {
		float resultHP = uiHPManaTool.playerHealth.PlayerHP / maxHP;
		Vector2 clothRect = uiHPManaTool.clothHPMask.rectTransform.sizeDelta;
		
		uiHPManaTool.clothHPMask.rectTransform.sizeDelta = new Vector2( maxClothWidth * resultHP, clothRect.y);
		uiHPManaTool.reducedHPFX.transform.localPosition = new Vector3 (clothRect.x, 0f, 0f);

		uiHPManaTool.isHPChange = false;
		// Debug.Log("Hp Changed");
	}

	void CheckMP () {
		float resultMP = uiHPManaTool.playerMana.PlayerMP / maxMP;

		uiHPManaTool.imageMP.fillAmount = resultMP;
		
		uiHPManaTool.isReducingShadowMP = true;
		uiHPManaTool.isMPChange = false;
	}

	public void PrintTool (Sprite toolSprite, string toolName) {
		uiHPManaTool.imageTool.sprite = toolSprite;
		// Debug.Log(toolSprite.name);
		// uiPlayerInfoSystem.PrintTool(toolName);
	}

#region OLD
	// void CheckHP () {
	// 	if (uiHPManaTool.isHPChange) {
	// 		ReduceCloth();
	// 	} else if (isReducingCloth) {
	// 		if (healthThreshold > currentClothWidth) {
	// 			healthThreshold -= healthReduceValue * Time.deltaTime;
	// 			DrawClothHP();
	// 		} else {
	// 			isReducingCloth = false;
	// 		}
	// 	}
	// }

	// void CheckMP () {
	// 	if (uiHPManaTool.isMPChange) {
	// 		PrintMana();
	// 	}
	// }

	// public void ReduceCloth () {
	// 	playerHP = uiHPManaTool.playerHealth.PlayerHP;
	// 	// Debug.Log(playerHP);
	// 	currentClothWidth = (playerHP/maxHP) * maxClothWidth;
	// 	isReducingCloth = true;
	// 	uiHPManaTool.isHPChange = false;
	// 	// PrintHP ();
	// }

	// // void PrintHP () {
	// // 	uiPlayerInfoSystem.PrintHP(playerHP.ToString()+" / "+maxHP);
	// // }

	// public void PrintMana () {
	// 	playerMP = uiHPManaTool.playerMana.PlayerMP;
	// 	// Debug.Log("maxMP "+maxMP);
	// 	// Debug.Log("playerMP "+playerMP);
	// 	uiHPManaTool.imageMana.fillAmount = playerMP/maxMP;
	// 	uiHPManaTool.isMPChange = false;
	// 	// uiPlayerInfoSystem.PrintMP(playerMP.ToString()+" / "+maxMP);
	// }

	// void DrawClothHP () {
	// 	//SET CLOTH POS X
	// 	uiHPManaTool.clothHP.rectTransform.localPosition = new Vector2 (healthThreshold - initClothPos, 0f);
	// 	// uiHPManaTool.clothHP.rectTransform.localPosition = new Vector2 (healthThreshold, 0f);
	// 	// Debug.Log(healthThreshold);
	// 	// Debug.Log(initClothPos);

	#region OLD (Using Shader Plugin)
	//SET CLOTH SCALE X
	// Vector2 scale = uiHPManaTool.clothHP.localScale;
	// uiHPManaTool.clothHP.localScale = new Vector2 (healthThreshold, scale.y);

	// //SET CLOTH POS X
	// Vector2 pos = uiHPManaTool.clothHP.localPosition;
	// float newPosX = (healthThreshold - 1f) / 2f;
	// uiHPManaTool.clothHP.localPosition = new Vector2 (newPosX, pos.y);
	#endregion
	// }
#endregion
}
