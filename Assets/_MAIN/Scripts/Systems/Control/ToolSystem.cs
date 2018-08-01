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
			// Player player = toolData.Player[i];
			state = player.state;
			tool = toolData.PlayerTool[i];
			Animation2D anim = toolData.Animation[i];
			
			// bool isChangeTool = input.IsChangeTool;
			// bool isUsingTool = input.IsUsingTool;

			StandAnimationState standAnimState = anim.standAnimState;
			toolType = (int)tool.currentTool;

			// if (isChangeTool) {
			// 	ChangeTool();
			// 	input.IsChangeTool = false;
			// } 
			
			if ((state == PlayerState.USING_TOOL) || (state == PlayerState.HOOK)) {
				if (tool.IsActToolReady) {
					UseTool ();
					tool.IsActToolReady = false;
				}
			}
		}

		//if player input button action, do use tool.
		// if(Input.GetKeyDown(KeyCode.Space)){
		// 	foreach(var e in GetEntities<ToolComponent>()){
		// 		if(!e.playerTool.isUsingTool){
		// 			e.playerTool.isUsingTool = true;
		// 			UseTool(e);
		// 		}
		// 	}
		// }

		// if(Input.GetKeyDown(KeyCode.C)){
		// 	foreach(var e in GetEntities<ToolComponent>()){
		// 		if(!e.playerTool.isUsingTool){
		// 			ChangeTool(e);
		// 		}
		// 	}
		// }
	}

	public void NextTool ()
	{
		int current = (int) tool.currentTool;
		
		if(current >= ((int)ToolType.Boots)){
			current = 1;
		}else{
			current++;
		}

		PrintToolName ((ToolType) current);
	}

	public void PrevTool () 
	{
		int current = (int) tool.currentTool;
		
		if(current <= ((int)ToolType.None)){
			current = 18; //Current tool length
		}else{
			current--;
		}

		PrintToolName ((ToolType) current);
	}

	void PrintToolName (ToolType type) {
		tool.currentTool = type;
		tool.textToolName.text = type.ToString();
	}

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
		} else if (tool.currentTool == ToolType.PowerBracelet){
			UsePowerBracelet();
		} else if (tool.currentTool == ToolType.Flippers){
			UseFlippers();
		} else if (tool.currentTool == ToolType.Boots){
			UseBoots();
		}
		
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
		tool.SpawnSlashEffect(toolType);
	}
	
	void UseHook()
	{
		Debug.Log("Using Hook");
		//shoots projectile with rope. shoot in face direction. has range.
		//if the projectiles reaches something, do thigs:
		//1. enemy: pull the enemy towards player
		//2. solid objects: pull player towards the object
		
		//small damage to enemies
		tool.SpawnSlashEffect(toolType);
		player.SetPlayerState(PlayerState.HOOK);
	}
	
	void UseBomb()
	{
		Debug.Log("Using Bomb");

		//plant a bomb that explodes after certain time
		//can be dropped or thrown
		//limited amount

		//big damage to enemies
		tool.SpawnSlashEffect(toolType);
	}
	
	void UseHammer()
	{
		Debug.Log("Using Hammer");
		//hammer destroy stones or destroyable solid object
		//break some enemy armor
		
		//small damage to enemy
		tool.SpawnSlashEffect(toolType);
	}
	
	void UseNet()
	{
		Debug.Log("Using Net");
		//catch certain objects (land/air)
		tool.SpawnSlashEffect(toolType);
	}
	
	void UseFisingRod()
	{
		Debug.Log("Using Fishing Rod");
		//catch water object
		//mini game fishing when triggered
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
		tool.SpawnSlashEffect(toolType);
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

		// Cloak cloak = tool.GetObj(toolType).GetComponent<Cloak>();

		if (player.IsInvisible) {
			player.IsInvisible = false;
			//
		} else {
			player.IsInvisible = true;							
			//
		}
	}
	
	void UseMagicMedallion()
	{
		Debug.Log("Using Magic Medallion");

		//big AoE Damage to all enemies on screen (?) 
		//affect some objects (destroy objects)

		//big damage
		tool.SpawnSlashEffect(toolType);
	}
	
	void UseFastTravel()
	{
		Debug.Log("Using Fast Travel");
	}
	
	void UsePowerBracelet()
	{
		Debug.Log("Using Power Bracelet");

		//allow to lift objects
		tool.SpawnSlashEffect(toolType);
	}
	
	void UseFlippers()
	{
		Debug.Log("Using Flippers");

		//allow to swim on water
	}
	
	void UseBoots()
	{
		Debug.Log("Using Boots");
		
		//allow dash attack
		//dash straight until hit an obstacle
		//bounce back on impact
		
		// tool.SpawnSlashEffect(toolType);
		player.SetPlayerState(PlayerState.DASH);
	}
}