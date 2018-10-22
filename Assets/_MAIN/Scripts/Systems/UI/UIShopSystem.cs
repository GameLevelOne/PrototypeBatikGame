using UnityEngine;
using UnityEngine.EventSystems;
using Unity.Entities;

public class UIShopSystem : ComponentSystem {
	public struct InputData {
		public readonly int Length;
		public ComponentArray<PlayerInput> PlayerInput;
	}
	[InjectAttribute] InputData inputData;
	public struct UIShopData {
		public readonly int Length;
		public ComponentArray<UIShop> UIShop;
	}
	[InjectAttribute] UIShopData uiShopData;
	public struct ContainerData {
		public readonly int Length;
		public ComponentArray<Container> container;
	}
	[InjectAttribute] ContainerData containerData;
	UIShop uiShop;
	LootableType[] playerContainer;
	PlayerInput playerInput;
	[InjectAttribute] UIToolsSelectionSystem uiToolsSelectionSystem;

	protected override void OnUpdate () {
		for (int i=0; i<uiShopData.Length; i++) {
			uiShop = uiShopData.UIShop[i];
			for (int j=0;j<inputData.Length;j++) {
				playerInput = inputData.PlayerInput[j];
			}
			
			if (!uiShop.isInitShop) {
				InitShop ();

			} else {
				CheckShowingShop ();	
				CheckInput ();
			}
		}
	}

	void InitShop () {
		uiShop.isInitShop = true;
		uiShop.isOpeningShop = false;
		uiShop.isShopFade = false;

		uiShop.healthPriceLabel.text = ""+uiShop.healthPrice;
		uiShop.manaPriceLabel.text = ""+uiShop.manaPrice;
		uiShop.curType = LootableType.NONE;

		if (Time.timeScale == 0) {
			Time.timeScale = 1;
		}

		// uiShop.canvasShopGroup.alpha = 0f;
		// uiShop.animator.Play(Constants.AnimationName.CANVAS_INVISIBLE);
		uiShop.panelUIShop.SetActive(false);
	}

	void CheckInput () {
		if (uiShop.isOpeningShop) {
			playerInput.isUIOpen = true;
			
			//EXIT SHOP
			if (GameInput.IsDodgePressed || GameInput.IsInventoryPressed) {
				uiShop.isOpeningShop = false;
				playerInput.isUIOpen = false;
				uiShop.uiAudio.PlayOneShot(uiShop.backClip);			
			}

			//SELECT LEFT
			if (!uiShop.leftPressed && GameInput.IsLeftDirectionHeld) {
				uiShop.leftPressed = true;

				uiShop.curType = LootableType.HP_POTION;
				ShowCurrentlySelected();
				uiShop.uiAudio.PlayOneShot(uiShop.selectClip);			
			} else if (!GameInput.IsLeftDirectionHeld) {
				uiShop.leftPressed = false;
			}

			//SELECT RIGHT
			if (!uiShop.rightPressed && GameInput.IsRightDirectionHeld) {
				uiShop.rightPressed = true;

				uiShop.curType = LootableType.MANA_POTION;
				ShowCurrentlySelected();
				uiShop.uiAudio.PlayOneShot(uiShop.selectClip);			
			} else if (!GameInput.IsRightDirectionHeld) {
				uiShop.rightPressed = false;
			}

			//BUY ITEM
			if (IsReadyToBuy() && (GameInput.IsAttackPressed || GameInput.IsActionPressed)) {
				BuyItemShop ();
			}
		}
	}

	bool IsReadyToBuy() {
		if (uiShop.curType==LootableType.HP_POTION && uiShop.handL.GetCurrentAnimatorStateInfo(0).IsName("Select")) 
			return true;
		if (uiShop.curType==LootableType.MANA_POTION && uiShop.handR.GetCurrentAnimatorStateInfo(0).IsName("Select")) 
			return true;
		return false;
	}
	void ShowCurrentlySelected() {
		if (uiShop.curType==LootableType.HP_POTION) {
			uiShop.handL.SetBool("Select",true);
			uiShop.handR.SetBool("Select",false);

			uiShop.infoHP.SetActive(true);
			uiShop.infoMP.SetActive(false);
		} else if (uiShop.curType==LootableType.MANA_POTION) {
			uiShop.handL.SetBool("Select",false);
			uiShop.handR.SetBool("Select",true);

			uiShop.infoHP.SetActive(false);
			uiShop.infoMP.SetActive(true);
		} else {
			uiShop.handL.SetBool("Select",false);
			uiShop.handR.SetBool("Select",false);

			uiShop.infoHP.SetActive(false);
			uiShop.infoMP.SetActive(false);
		}
	}

	void BuyItemShop () {
		int containerIdx = getNextEmptyContainer();
		if (uiShop.curType==LootableType.HP_POTION) {
			if (GameStorage.Instance.PlayerCoin >= uiShop.healthPrice && containerIdx>=0) {
				// uiShop.BuyItem(LootableType.HP_POTION, uiShop.healthPrice);
				uiShop.handL.SetTrigger("Buy");

				GameStorage.Instance.PlayerCoin-= uiShop.healthPrice;
				playerContainer[containerIdx] = LootableType.HP_POTION;

				GetPlayerData();
				uiShop.healthContainer[containerIdx].Play("Appear",0,0f);				

				//RESET TOOL
				uiToolsSelectionSystem.InitImages(true);
				uiShop.uiAudio.PlayOneShot(uiShop.chooseClip);			
			} else {
				uiShop.uiAudio.PlayOneShot(uiShop.failClip);			
				uiShop.handL.SetTrigger("Fail");
			}
		} else {
			if (GameStorage.Instance.PlayerCoin >= uiShop.manaPrice && containerIdx>=0) {
				// uiShop.BuyItem(LootableType.MANA_POTION, uiShop.manaPrice);
				uiShop.handR.SetTrigger("Buy");

				GameStorage.Instance.PlayerCoin-= uiShop.manaPrice;
				playerContainer[containerIdx] = LootableType.MANA_POTION;

				GetPlayerData();
				uiShop.manaContainer[containerIdx].Play("Appear",0,0f);				

				//RESET TOOL
				uiToolsSelectionSystem.InitImages(true);
				uiShop.uiAudio.PlayOneShot(uiShop.chooseClip);			
			} else {
				uiShop.uiAudio.PlayOneShot(uiShop.failClip);			
				uiShop.handR.SetTrigger("Fail");
			}
		}

	}

	void CheckShowingShop () {
		if (uiShop.isOpeningShop) {
			ShowShop ();
		} else {
			HideShop ();
		}
	}

	void GetPlayerData(){
		uiShop.playerMoneyLabel.text = ""+GameStorage.Instance.PlayerCoin;
		for(int i=0;i<containerData.Length;i++) {
			playerContainer = containerData.container[i].lootableTypes;
		}
		for (int i=0;i<playerContainer.Length;i++) {
			showPlayerContainer(i);
		}
	}

	void showPlayerContainer(int idx){
		if (playerContainer[idx] == LootableType.NONE) {
			uiShop.healthContainer[idx].gameObject.SetActive(false);
			uiShop.manaContainer[idx].gameObject.SetActive(false);
		} else if (playerContainer[idx] == LootableType.HP_POTION) {
			uiShop.healthContainer[idx].gameObject.SetActive(true);
			uiShop.manaContainer[idx].gameObject.SetActive(false);
		} else if (playerContainer[idx] == LootableType.MANA_POTION) {
			uiShop.healthContainer[idx].gameObject.SetActive(false);
			uiShop.manaContainer[idx].gameObject.SetActive(true);
		}
	}

	int getNextEmptyContainer() {
		int curIdx = -1;
		for (int i=0;i<playerContainer.Length;i++){
			if (playerContainer[i] == LootableType.NONE) {
				curIdx = i;
				break;
			}
		}

		return curIdx;
	}

	void ShowShop () {
		if (!uiShop.isShopFade) {
			Time.timeScale = 0;
			uiShop.panelUIShop.SetActive(true);
			uiShop.isShopFade = true;
			uiShop.curType = LootableType.NONE;
			uiShop.leftPressed = false;
			uiShop.rightPressed = false;			
			GetPlayerData();
			ShowCurrentlySelected();
			uiShop.animator.Play(Constants.AnimationName.FADE_IN);
			uiShop.uiAudio.PlayOneShot(uiShop.openClip);			
		} else {
			uiShop.animator.Play(Constants.AnimationName.CANVAS_VISIBLE);
		}
	}

	void HideShop () {
		if (uiShop.isShopFade) {
			uiShop.animator.Play(Constants.AnimationName.FADE_OUT);
			uiShop.isShopFade = false;
			Time.timeScale = 1;
		} else {
			// uiShop.animator.Play(Constants.AnimationName.CANVAS_INVISIBLE);
			uiShop.panelUIShop.SetActive(false);
		}
	}
}
