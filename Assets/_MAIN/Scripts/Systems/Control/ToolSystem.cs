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
	
	// public struct ToolComponent
	// {
	// 	public PlayerTool playerTool;
	// }
	// PlayerTool playerTool;

	public PlayerTool tool;

	PlayerInput input;
	Player player;

	PlayerState state;

	int toolType;

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
			
			if ((state == PlayerState.USING_TOOL) || (state == PlayerState.HOOK) || (state == PlayerState.FISHING) || (state == PlayerState.BOW)) {
				if (tool.IsActToolReady) {
					UseTool ();
					tool.IsActToolReady = false;
				}
			} else if (tool.IsPowerBraceletSelected) {
				tool.IsPowerBraceletSelected = false;
			} 
			// else if (tool.IsFlipperSelected) {
			// 	tool.IsFlipperSelected = false;
			// }
		}
	}

	public void NextTool ()
	{
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
	}

	public void PrevTool () 
	{
		int current = (int) tool.currentTool;
		
		if(current <= ((int)ToolType.None)){
			current = (int)ToolType.Boots; //Current tool length
		}else{
			current--;
		}
		
		tool.currentTool = (ToolType) current;
		
		if (!CheckIfToolHasBeenUnlocked(current)) {
			PrevTool ();
		}
	}

	bool CheckIfToolHasBeenUnlocked (int type) {
		if (tool.CheckIfToolHasBeenUnlocked(type) > 0) {
			toolType = type;
			tool.textToolName.text = ((ToolType) toolType).ToString();
			CheckPowerBracelet();
			// CheckFlipper();
			return true;
		} else {
			return false;
		}
	}

	void CheckPowerBracelet () {
		if ((ToolType) toolType == ToolType.PowerBracelet) {
			tool.IsPowerBraceletSelected = true;
			UsePowerBracelet (true);
		} else {
			tool.IsPowerBraceletSelected = false;
			UsePowerBracelet (false);
		}
	}

	// void CheckFlipper () {
	// 	if ((ToolType) toolType == ToolType.Flippers) {
	// 		tool.IsFlipperSelected = true;
	// 		UseFlippers (true);
	// 	} else {
	// 		tool.IsFlipperSelected = false;
	// 		UseFlippers (false);
	// 	}
	// }

	public void UseTool ()
	{
		if (tool.currentTool == ToolType.Bow){
			UseBow();
		} else if (tool.currentTool == ToolType.Hook){
			UseHook();
		} else if (tool.currentTool == ToolType.Bomb){
			UseBomb();
		} else if (tool.currentTool == ToolType.Hammer){
			UseHammer();
		} else if (tool.currentTool == ToolType.Net){
			UseNet();
		} else if (tool.currentTool == ToolType.FishingRod){
			UseFisingRod();
		} else if (tool.currentTool == ToolType.Container1){
			UseContainer1();
		} else if (tool.currentTool == ToolType.Container2){
			UseContainer2();
		} else if (tool.currentTool == ToolType.Container3){
			UseContainer3();
		} else if (tool.currentTool == ToolType.Container4){
			UseContainer4();
		} else if (tool.currentTool == ToolType.Shovel){
			UseShovel();
		} else if (tool.currentTool == ToolType.Lantern){
			UseLantern();
		} else if (tool.currentTool == ToolType.InvisibilityCloak){
			UseInvisibilityCloak();
		} else if (tool.currentTool == ToolType.MagicMedallion){
			UseMagicMedallion();
		} else if (tool.currentTool == ToolType.FastTravel){
			UseFastTravel();
		} 
		// else if (tool.currentTool == ToolType.PowerBracelet){
		// 	UsePowerBracelet();
		// } 
		// else if (tool.currentTool == ToolType.Flippers){
		// 	UseFlippers();
		// } 
		// else if (tool.currentTool == ToolType.Boots){
		// 	UseBoots();
		// }
		
		// input.IsUsingTool = false;
	}

	void UseBow()
	{
		Debug.Log("Using Bow");

		//shoots projectile (arrow, limited amount).
		//shoots immediately on face direction
		//projectiles flies until hit obstacle
		//can break certain objects.

		//medium damage to enemies
		SpawnSlashEffect(toolType);
	}
	
	void UseHook()
	{
		Debug.Log("Using Hook");
		//shoots projectile with rope. shoot in face direction. has range.
		//if the projectiles reaches something, do thigs:
		//1. enemy: pull the enemy towards player
		//2. solid objects: pull player towards the object
		
		//small damage to enemies
		SpawnSlashEffect(toolType);
		player.SetPlayerState(PlayerState.HOOK);
	}
	
	void UseBomb()
	{
		Debug.Log("Using Bomb");

		//plant a bomb that explodes after certain time
		//can be dropped or thrown
		//limited amount

		//big damage to enemies
		SpawnSlashEffect(toolType);
		player.SetPlayerIdle();
	}
	
	void UseHammer()
	{
		Debug.Log("Using Hammer");
		//hammer destroy stones or destroyable solid object
		//break some enemy armor
		
		//small damage to enemy
		SpawnSlashEffect(toolType);
	}
	
	void UseNet()
	{
		Debug.Log("Using Net");
		//catch certain objects (land/air)
		SpawnSlashEffect(toolType);
	}
	
	void UseFisingRod()
	{
		Debug.Log("Using Fishing Rod");
		//catch water object
		//mini game fishing when triggered
		FishingRod fishingRod = tool.GetObj(toolType).GetComponent<FishingRod>();
		FishingRodState fishingState = fishingRod.state;

		if (!fishingRod.isBaitLaunched && fishingState == FishingRodState.IDLE) {
			player.SetPlayerState(PlayerState.FISHING);
			fishingRod.state = FishingRodState.THROW;
		} else if (fishingState == FishingRodState.STAY) {
			fishingRod.state = FishingRodState.RETURN;
		}
	}
	

	//containers can store certain enemies or items. (caught enemies will be stored in containers)
	void UseContainer1()
	{
		Debug.Log("Using Container1");
		containerSystem.UseCollectibleInContainer(0);
	}
	
	void UseContainer2()
	{
		Debug.Log("Using Container2");
		containerSystem.UseCollectibleInContainer(1);
	}
	
	void UseContainer3()
	{
		Debug.Log("Using Container3");
		containerSystem.UseCollectibleInContainer(2);
	}
	
	void UseContainer4()
	{
		Debug.Log("Using Container4");
		containerSystem.UseCollectibleInContainer(3);
	}

	void UseShovel()
	{
		Debug.Log("Using Shovel");
		//dig items from ground
		SpawnSlashEffect(toolType);
	}
	
	void UseLantern()
	{
		Debug.Log("Using Lantern");

		Lantern lantern = tool.GetObj(toolType).GetComponent<Lantern>();
			
		if (!lantern.IsLightOn) {
			lantern.IsLightOn = true;
		} else {
			lantern.IsLightOn = false;
		}
	}
	
	void UseInvisibilityCloak()
	{
		Debug.Log("Using Invisibility Cloak");

		Cloak cloak = tool.GetObj(toolType).GetComponent<Cloak>();

		if (!player.IsInvisible) {
			player.IsInvisible = true;
			cloak.IsInvisible = true;
		} else {
			player.IsInvisible = false;	
			cloak.IsInvisible = false;
		}
	}
	
	void UseMagicMedallion()
	{
		Debug.Log("Using Magic Medallion");

		//big AoE Damage to all enemies on screen (?) 
		//affect some objects (destroy objects)

		//big damage
		SpawnSlashEffect(toolType);
	}
	
	void UseFastTravel()
	{
		Debug.Log("Using Fast Travel");
	}
	
	void UsePowerBracelet(bool value)
	{
		//allow to lift objects
		// tool.SpawnSlashEffect(toolType);
		int type = (int) ToolType.PowerBracelet;

		PowerBracelet powerBracelet = tool.GetObj(type).GetComponent<PowerBracelet>();
		// Debug.Log(value);
		powerBracelet.IsColliderOn = value;
	}
	
	// void UseFlippers(bool value)
	// {
	// 	//allow to swim on water
	// 	int type = (int) ToolType.Flippers;

	// 	Flippers flippers = tool.GetObj(type).GetComponent<Flippers>();
	// 	// Debug.Log(value);
	// 	flippers.isEquipped = value;
	// }
	
	void UseBoots()
	{
		Debug.Log("Using Boots");
		
		//allow dash attack
		//dash straight until hit an obstacle
		//bounce back on impact
		
		// tool.SpawnSlashEffect(toolType);
		player.SetPlayerState(PlayerState.DASH);
	}

	public void SpawnSlashEffect (int toolType) {
        switch (toolType) {
            case 1:
				SpawnObj (toolType, false, false);
                break;
            case 2:
                SpawnObj (toolType, false, false);
                break;
            case 3:
                SpawnObj (toolType, false, true);
                break;
            case 4:
                SpawnObj (toolType, false, false);
                break;
            case 5:
                SpawnObj (toolType, false, false);
                break;
            case 11:
                SpawnObj (toolType, false, false);
                break;
            case 12:
                SpawnObj (toolType, false, false);
                break;
            case 14:
                SpawnObj (toolType, true, true);
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

    void SpawnObj (int toolType, bool isSpawnAtPlayerPos, bool isAlwaysUp) {
        GameObject spawnedBullet = GameObjectEntity.Instantiate(tool.GetObj(toolType), tool.areaSpawnPos.position, SetFacing());
        // spawnedBullet.transform.SetParent(this.transform); //TEMPORARY
		
		if (isSpawnAtPlayerPos) {
			spawnedBullet.transform.position = tool.transform.root.position;
		}

		if (isAlwaysUp) {
			spawnedBullet.transform.eulerAngles = Vector3.zero;
		}

        spawnedBullet.SetActive(true);
    }

    Quaternion SetFacing () {
        Vector2 targetPos = tool.areaSpawnPos.position;
        Vector2 initPos = tool.transform.position; //TEMPORARY

        targetPos.x -= initPos.x;
        targetPos.y -= initPos.y;
        float angle = Mathf.Atan2 (targetPos.y, targetPos.x) * Mathf.Rad2Deg;
        Quaternion targetRot = Quaternion.Euler (new Vector3 (0f, 0f, angle));

        return targetRot;
    }
}