using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

public class ToolSystem : ComponentSystem {

	public struct ToolData {
		public readonly int Length;
		// public ComponentArray<PlayerInput> PlayerInput;
		// public ComponentArray<Player> Player;
		public ComponentArray<PlayerTool> PlayerTool;
		public ComponentArray<Animation2D> Animation;
	}
	[InjectAttribute] ToolData toolData;

	[InjectAttribute] PlayerInputSystem playerInputSystem;
	[InjectAttribute] ContainerSystem containerSystem;
	[InjectAttribute] UIToolsSelectionSystem uiToolsSelectionSystem;
	
	// public struct ToolComponent
	// {
	// 	public PlayerTool playerTool;
	// }
	// PlayerTool playerTool;

	public PlayerTool tool;

	PlayerInput input;
	Player player;

	PlayerState state;

	// int toolType;

	protected override void OnUpdate()
	{
		if (toolData.Length == 0) return;
		
		if (input == null || player == null) {
			input = playerInputSystem.input;
			player = playerInputSystem.player;

			return;
		} 
		
		for (int i=0; i<toolData.Length; i++) {
			state = player.state;
			tool = toolData.PlayerTool[i];
			Animation2D anim = toolData.Animation[i];
			
			if (!tool.isInitCurrentTool) {
				InitTool();
			}

			if ((state == PlayerState.USING_TOOL) || (state == PlayerState.HOOK) || (state == PlayerState.FISHING) || (state == PlayerState.BOW)) {
				if (tool.isActToolReady) {
					UseTool ();
					tool.isActToolReady = false;
				}
			} 
			// else if (tool.isPowerBraceletSelected) {
			// 	tool.isPowerBraceletSelected = false;
			// } 
			// else if (tool.isFlipperSelected) {
			// 	tool.isFlipperSelected = false;
			// }
		}
	}

	void InitTool () {
		// CheckSavedTool ();
		// Debug.Log("InitTool");
		for (int i=1; i<=(int)ToolType.Boots; i++) {
			if (CheckIfToolHasBeenUnlocked(i)) {
				tool.currentTool = (ToolType) i;
				uiToolsSelectionSystem.SetPrintedTool();

				tool.isInitCurrentTool = true;
				return;
			}
		}

		if (tool.currentTool == 0) {
			Debug.Log("No Tools Unlocked");
			//
		}

		tool.isInitCurrentTool = true;
	}

	void CheckSavedTool () {
		tool.Bow = tool.Bow == 1? 1 : 0;
	}

	public void NextTool ()
	{
		if (uiToolsSelectionSystem.isChangingTool) return;

		int current = (int) tool.currentTool;
		
		if(current >= ((int)ToolType.Boots)){
			current = 1;
		}else{
			current++;
		}
		tool.currentTool = (ToolType) current;

		if (!CheckIfToolHasBeenUnlocked(current)) {
			NextTool ();
		}
		
		uiToolsSelectionSystem.uiToolsSelection.OnClickNextToolsSelection();
	}

	public void PrevTool () 
	{
		if (uiToolsSelectionSystem.isChangingTool) return;

		int current = (int) tool.currentTool;
		
		if(current <= ((int)ToolType.None)){
			current = (int)ToolType.Boots; //Current tool length
		}else{
			current--;
		}
		tool.currentTool = (ToolType) current;
		
		if (!CheckIfToolHasBeenUnlocked((int) tool.currentTool)) {
			PrevTool ();
		}
		
		uiToolsSelectionSystem.uiToolsSelection.OnClickPrevToolsSelection();
	}

	bool CheckIfToolHasBeenUnlocked (int type) {
		if (tool.CheckIfToolHasBeenUnlocked(type) > 0) {
			// toolType = type;
			// tool.textToolName.text = ((ToolType) toolType).ToString();
			// CheckPowerBracelet();
			// CheckFlipper();
			return true;
		} else {
			return false;
		}
	}

	// void CheckPowerBracelet () {
	// 	if ((ToolType) toolType == ToolType.PowerBracelet) {
	// 		tool.isPowerBraceletSelected = true;
	// 		UsePowerBracelet (true);
	// 	} else {
	// 		tool.isPowerBraceletSelected = false;
	// 		UsePowerBracelet (false);
	// 	}
	// }

	// void CheckFlipper () {
	// 	if ((ToolType) toolType == ToolType.Flippers) {
	// 		tool.isFlipperSelected = true;
	// 		UseFlippers (true);
	// 	} else {
	// 		tool.isFlipperSelected = false;
	// 		UseFlippers (false);
	// 	}
	// }

	public void UseTool ()
	{
		switch (tool.currentTool) {
			case ToolType.Bow:
				UseBow();
				break;
			// case ToolType.Hook:
			// 	UseHook();
			// 	break;
			case ToolType.Bomb:
				UseBomb();
				break;
			case ToolType.Hammer:
				UseHammer();
				break;
			// case ToolType.Net:
			// 	UseNet();
			// 	break;
			case ToolType.FishingRod:
				UseFisingRod();
				break;
			case ToolType.Container1:
				UseContainer(0);
				break;
			case ToolType.Container2:
				UseContainer(1);
				break;
			case ToolType.Container3:
				UseContainer(2);
				break;
			case ToolType.Container4:
				UseContainer(3);
				break;
			case ToolType.Shovel:
				UseShovel();
				break;
			// case ToolType.Lantern:
			// 	UseLantern();
			// 	break;
			// case ToolType.InvisibilityCloak:
			// 	UseInvisibilityCloak();
			// 	break;
			case ToolType.MagicMedallion:
				UseMagicMedallion();
				break;
			// case ToolType.FastTravel:
			// 	UseFastTravel();
			// 	break;
			// case ToolType.PowerBracelet:
			// 	UsePowerBracelet();
			// 	break;
			// case ToolType.Flippers:
			// 	UseFlippers();
			// 	break;
			// case ToolType.Boots:
			// 	UseBoots();
			// 	break;
			default :
				Debug.Log("Player using unknown tool : "+tool.currentTool);
				break;
		}
	}

	void UseBow()
	{
		Debug.Log("Using Bow");

		//shoots projectile (arrow, limited amount).
		//shoots immediately on face direction
		//projectiles flies until hit obstacle
		//can break certain objects.

		//medium damage to enemies
		SpawnSlashEffect((int) tool.currentTool);
	}
	
	void UseHook()
	{
		// Debug.Log("Using Hook");
		// //shoots projectile with rope. shoot in face direction. has range.
		// //if the projectiles reaches something, do thigs:
		// //1. enemy: pull the enemy towards player
		// //2. solid objects: pull player towards the object
		
		// //small damage to enemies
		// SpawnSlashEffect(toolType);
		// player.SetPlayerState(PlayerState.HOOK);
	}
	
	void UseBomb()
	{
		Debug.Log("Using Bomb");

		//plant a bomb that explodes after certain time
		//can be dropped or thrown
		//limited amount

		//big damage to enemies
		SpawnSlashEffect((int) tool.currentTool);
		player.SetPlayerIdle();
	}
	
	void UseHammer()
	{
		Debug.Log("Using Hammer");
		//hammer destroy stones or destroyable solid object
		//break some enemy armor
		
		//small damage to enemy
		SpawnSlashEffect((int) tool.currentTool);
	}
	
	void UseNet()
	{
		// Debug.Log("Using Net");
		// //catch certain objects (land/air)
		// SpawnSlashEffect(toolType);
	}
	
	void UseFisingRod()
	{
		Debug.Log("Using Fishing Rod");
		//catch water object
		//mini game fishing when triggered
		FishingRod fishingRod = tool.GetObj((int) tool.currentTool).GetComponent<FishingRod>();
		FishingRodState fishingState = fishingRod.state;

		if (!fishingRod.isBaitLaunched && fishingState == FishingRodState.IDLE) {
			player.SetPlayerState(PlayerState.FISHING);
			fishingRod.state = FishingRodState.THROW;
		} else if (fishingState == FishingRodState.STAY) {
			fishingRod.state = FishingRodState.RETURN;
		}
	}
	

	//containers can store certain enemies or items. (caught enemies will be stored in containers)
	void UseContainer(int containerType)
	{
		Debug.Log("Using Container " + containerType++);
		containerSystem.UseCollectibleInContainer(containerType);
	}
	
	// void UseContainer1()
	// {
	// 	Debug.Log("Using Container1");
	// 	containerSystem.UseCollectibleInContainer(0);
	// }

	// void UseContainer2()
	// {
	// 	Debug.Log("Using Container2");
	// 	containerSystem.UseCollectibleInContainer(1);
	// }
	
	// void UseContainer3()
	// {
	// 	Debug.Log("Using Container3");
	// 	containerSystem.UseCollectibleInContainer(2);
	// }
	
	// void UseContainer4()
	// {
	// 	Debug.Log("Using Container4");
	// 	containerSystem.UseCollectibleInContainer(3);
	// }

	void UseShovel()
	{
		Debug.Log("Using Shovel");
		//dig items from ground
		SpawnSlashEffect((int) tool.currentTool);
	}
	
	void UseLantern()
	{
		// Debug.Log("Using Lantern");

		// Lantern lantern = tool.GetObj(toolType).GetComponent<Lantern>();
			
		// if (!lantern.isLightOn) {
		// 	lantern.isLightOn = true;
		// } else {
		// 	lantern.isLightOn = false;
		// }
	}
	
	void UseInvisibilityCloak()
	{
		// Debug.Log("Using Invisibility Cloak");

		// Cloak cloak = tool.GetObj(toolType).GetComponent<Cloak>();

		// if (!player.isInvisible) {
		// 	player.isInvisible = true;
		// 	cloak.isInvisible = true;
		// } else {
		// 	player.isInvisible = false;	
		// 	cloak.isInvisible = false;
		// }
	}
	
	void UseMagicMedallion()
	{
		Debug.Log("Using Magic Medallion");

		//big AoE Damage to all enemies on screen (?) 
		//affect some objects (destroy objects)

		//big damage
		SpawnSlashEffect((int) tool.currentTool);
	}
	
	void UseFastTravel()
	{
		// Debug.Log("Using Fast Travel");
	}
	
	// void UsePowerBracelet(bool value)
	// {
	// 	//allow to lift objects
	// 	// tool.SpawnSlashEffect(toolType);
	// 	int type = (int) ToolType.PowerBracelet;

	// 	PowerBracelet powerBracelet = tool.GetObj(type).GetComponent<PowerBracelet>();
	// 	// Debug.Log(value);
	// 	powerBracelet.isColliderOn = value;
	// }
	
	// void UseFlippers(bool value)
	// {
	// 	//allow to swim on water
	// 	int type = (int) ToolType.Flippers;

	// 	Flippers flippers = tool.GetObj(type).GetComponent<Flippers>();
	// 	// Debug.Log(value);
	// 	flippers.isEquipped = value;
	// }
	
	// void UseBoots()
	// {
		// Debug.Log("Using Boots");
		
		//allow dash attack
		//dash straight until hit an obstacle
		//bounce back on impact
		
		// tool.SpawnSlashEffect(toolType);
		// player.SetPlayerState(PlayerState.DASH);
	// }

	void SpawnSlashEffect (int toolType) {
        switch (toolType) {
            case 1: //BOW
				SpawnNormalToolObj (toolType);
                break;
            case 3: //BOMB
                SpawnSpecialToolObj (toolType);
                break;
            case 4: //HAMMER
                SpawnSpecialToolObj (toolType);
                break;
            case 11: //SHOVEL
                SpawnSpecialToolObj (toolType);
                break;
            case 14:
                SpawnBigSummonObj (toolType);
                break;
            case 16:
                // SpawnObj (powerBraceletAreaEffectObj, false, false);
                break;
            case 17:
                // Flippers
                break;
            case 18:
                // Boots
                break;
        }
    }

    void SpawnNormalToolObj (int toolType) {
		Vector3 targetPos = tool.areaSpawnPos.position;
        Vector3 initPos = tool.transform.position;

		Vector3 deltaPos = Vector3.zero;
		float localTargetPosX = tool.areaSpawnPos.localPosition.x;

		if (localTargetPosX != 0) {
			deltaPos = new Vector3 (targetPos.x - initPos.x, 0f, 0f); 
		} else {
			deltaPos = targetPos - initPos;
		}
		
        GameObject spawnedObj = GameObject.Instantiate(tool.GetObj(toolType), tool.areaSpawnPos.position, SetFacingParent(deltaPos));
       	// spawnedBullet.transform.SetParent(this.transform); //TEMPORARY

	   	spawnedObj.transform.GetChild(0).rotation = SetFacingChild(deltaPos);

        spawnedObj.SetActive(true);
    }

	void SpawnSpecialToolObj (int toolType) {
        GameObject spawnedObj = GameObject.Instantiate(tool.GetObj(toolType), tool.areaSpawnPos.position, Quaternion.identity);
       	// spawnedBullet.transform.SetParent(this.transform); //TEMPORARY

        spawnedObj.SetActive(true);
	}

	void SpawnBigSummonObj (int toolType) {	
		// Quaternion spawnRot = Quaternion.Euler(new Vector3 (40f, 0f, 0f));	
        GameObject spawnedObj = GameObject.Instantiate(tool.GetObj(toolType), tool.transform.root.position, Quaternion.identity);
       	// spawnedBullet.transform.SetParent(this.transform); //TEMPORARY
		   
		Time.timeScale = 0;
        spawnedObj.SetActive(true);
    }
	
	Quaternion SetFacingParent (Vector3 resultPos) {
        float angle = Mathf.Atan2 (resultPos.x, resultPos.z) * Mathf.Rad2Deg;
        Quaternion targetRot = Quaternion.Euler (new Vector3 (0f, angle - 90f, 0f));

        return targetRot;
	}

    Quaternion SetFacingChild (Vector3 resultPos) {
        float angle = Mathf.Atan2 (resultPos.z, resultPos.x) * Mathf.Rad2Deg;
        Quaternion targetRot = Quaternion.Euler (new Vector3 (40f, 0f, angle));

        return targetRot;
    }
}